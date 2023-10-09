// Copyright (c) 2023 Onyrew <onyrew@gmail.com>

namespace Onyrew.Projects.EventAccessHandler.Interfaces
{
    internal interface IEventAccess<_args_type> : IEventProxy<_args_type>
    {
        IEventAccess<_args_type> Register(Action<object, _args_type?> _event);

        IEventAccess<_args_type> Unregister(Action<object, _args_type?> _event);

        public static IEventAccess<_args_type> operator +(IEventAccess<_args_type> _this, Action<object, _args_type?> _event)
        {
            return _this.Register(_event);
        }

        public static IEventAccess<_args_type> operator -(IEventAccess<_args_type> _this, Action<object, _args_type?> _event)
        {
            return _this.Unregister(_event);
        }
    }
}
