using System;

namespace ProjectContext.Providers
{
    public interface ITimeProvider
    {
        float DeltaTime { get; }
        float FixedDeltaTime { get; }
        DateTime GetUtcTime { get; }
    }
}