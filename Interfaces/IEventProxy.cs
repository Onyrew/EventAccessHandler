// Copyright (c) 2023 Onyrew <onyrew@gmail.com>

namespace Onyrew.Projects.EventAccessHandler.Interfaces
{
    internal interface IEventProxy<_args_type> : IDisposable
    {
        EventAccessHandler? Handler { get; }

        string? Key { get; }

        bool Exists { get; }
    }
}
