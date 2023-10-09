// Copyright (c) 2023 Onyrew <onyrew@gmail.com>

using Onyrew.Projects.EventAccessHandler.Interfaces;
using System.Collections.Concurrent;

namespace Onyrew.Projects.EventAccessHandler
{
    internal class EventAccessHandler : IEventHandler
    {
        private class EventCollection<_args_type> : IEventCollection<_args_type>
        {
            private readonly List<Action<object, _args_type?>> events = new();
            private readonly object sync_object = new();

            public EventCollection() {}

            public IEventCollection<_args_type> Invoke(object _sender, _args_type? _args)
            {
                List<Action<object, _args_type?>> temp;
                lock (sync_object) {
                    temp = new List<Action<object, _args_type?>>(events);
                }
                temp.ForEach(v => v.Invoke(_sender, _args));
                return this;
            }

            public IEventCollection<_args_type> Register(Action<object, _args_type?> _event)
            {
                lock (sync_object) {
                    if (!events.Contains(_event)) {
                        events.Add(_event);
                    }
                }
                return this;
            }

            public IEventCollection<_args_type> Unregister(Action<object, _args_type?> _event)
            {
                lock (sync_object) {
                    events.Remove(_event);
                }
                return this;
            }

            public IEventCollection<_args_type> Clear()
            {
                lock (sync_object) {
                    events.Clear();
                }
                return this;
            }

            public void Dispose()
            {
                Clear();
            }
        }

        private class EventAccess<_args_type> : IEventAccess<_args_type>
        {
            private EventAccessHandler? handler;
            private string? key;

            public EventAccessHandler? Handler => handler;

            public string? Key => key;

            public bool Exists => handler?.Exists(key) ?? false;

            public EventAccess(EventAccessHandler _handler, string _key)
            {
                handler = _handler;
                key = _key;
            }

            public IEventAccess<_args_type> Register(Action<object, _args_type?> _event)
            {
                handler?.Register(key, _event);
                return this;
            }

            public IEventAccess<_args_type> Unregister(Action<object, _args_type?> _event)
            {
                handler?.Unregister(key, _event);
                return this;
            }

            public void Dispose()
            {
                handler = null;
                key = null;
            }
        }

        private class EventAccessToken<_args_type> : IEventAccessToken<_args_type>
        {
            private IEventAccess<_args_type>? event_access;

            public IEventAccess<_args_type>? EventAccess => event_access;

            public EventAccessHandler? Handler => event_access?.Handler;

            public string? Key => event_access?.Key;

            public bool Exists => event_access?.Exists ?? false;

            public EventAccessToken(IEventAccess<_args_type> _event_access)
            {
                event_access = _event_access;
            }

            public IEventAccess<_args_type> Register(Action<object, _args_type?> _event)
            {
                event_access?.Register(_event);
                return this;
            }

            public IEventAccess<_args_type> Unregister(Action<object, _args_type?> _event)
            {
                event_access?.Unregister(_event);
                return this;
            }

            public IEventAccessToken<_args_type> Invoke(object _sender, _args_type? _args)
            {
                Handler?.Invoke(Key, _sender, _args);
                return this;
            }

            public IEventAccessToken<_args_type> Release()
            {
                Dispose();
                return this;
            }

            public IEventAccessToken<_args_type> Clear()
            {
                Handler?.Clear<_args_type>(Key);
                return this;
            }

            public void Dispose()
            {
                Handler?.Release(Key);
                event_access?.Dispose();
                event_access = null;
            }
        }

        private readonly ConcurrentDictionary<string, object> events = new();

        public EventAccessHandler() {}

        private EventAccessHandler Invoke<_args_type>(string _key, object _sender, _args_type? _args)
        {
            ((IEventCollection<_args_type>)events[_key]).Invoke(_sender, _args);
            return this;
        }

        private void Release(string _key)
        {
            events.Remove(_key, out var e);
            (e as IDisposable)?.Dispose();
        }

        private EventAccessHandler Clear<_args_type>(string _key)
        {
            ((IEventCollection<_args_type>)events[_key]).Clear();
            return this;
        }

        public IEventAccessToken<_args_type>? Create<_args_type>(string _key)
        {
            return events.TryAdd(_key, new EventCollection<_args_type>()) ? new EventAccessToken<_args_type>(GetAccess<_args_type>(_key)) : null;
        }

        public IEventHandler Register<_args_type>(string _key, Action<object, _args_type?> _event)
        {
            if (events.TryGetValue(_key, out var e)) {
                ((IEventCollection<_args_type>)e).Register(_event);
            }
            return this;
        }

        public IEventHandler Unregister<_args_type>(string _key, Action<object, _args_type?> _event)
        {
            if (events.TryGetValue(_key, out var e)) {
                ((IEventCollection<_args_type>)e).Unregister(_event);
            }
            return this;
        }

        public IEventHandler Unregister<_args_type>(Action<object, _args_type?> _event)
        {
            foreach (var e in events) {
                ((IEventCollection<_args_type>)e.Value).Unregister(_event);
            }
            return this;
        }

        public IEventAccess<_args_type> GetAccess<_args_type>(string _key)
        {
            return new EventAccess<_args_type>(this, _key);
        }

        public bool Exists(string _key)
        {
            return events.ContainsKey(_key);
        }

        public void Dispose()
        {
            foreach (var e in events) {
                Release(e.Key);
            }
        }
    }
}
