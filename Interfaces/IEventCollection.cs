// Copyright (c) 2023 Onyrew <onyrew@gmail.com>

namespace Onyrew.Projects.EventAccessHandler.Interfaces
{
    internal interface IEventCollection<_args_type> : IDisposable
    {
        IEventCollection<_args_type> Invoke(object _sender, _args_type? _args);

        IEventCollection<_args_type> Register(Action<object, _args_type?> _event);

        IEventCollection<_args_type> Unregister(Action<object, _args_type?> _event);

        IEventCollection<_args_type> Clear();
    }
}
