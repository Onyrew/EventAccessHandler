// Copyright (c) 2023 Onyrew <onyrew@gmail.com>

namespace Onyrew.Projects.EventAccessHandler.Interfaces
{
    internal interface IEventAccessToken<_args_type> : IEventAccess<_args_type>, IEventProxy<_args_type>
    {
        IEventAccess<_args_type>? EventAccess { get; }

        IEventAccessToken<_args_type> Invoke(object _sender, _args_type? _args);

        IEventAccessToken<_args_type> Release();

        IEventAccessToken<_args_type> Clear();
    }
}
