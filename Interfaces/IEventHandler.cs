// Copyright (c) 2023 Onyrew <onyrew@gmail.com>

namespace Onyrew.Projects.EventAccessHandler.Interfaces
{
    internal interface IEventHandler : IDisposable
    {
        IEventAccessToken<_args_type>? Create<_args_type>(string _key);

        IEventHandler Register<_args_type>(string _key, Action<object, _args_type?> _event);

        IEventHandler Unregister<_args_type>(string _key, Action<object, _args_type?> _event);

        IEventHandler Unregister<_args_type>(Action<object, _args_type?> _event);

        IEventAccess<_args_type> GetAccess<_args_type>(string _key);

        bool Exists(string _key);
    }
}
