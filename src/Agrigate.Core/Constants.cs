namespace Agrigate.Core;

/// <summary>
/// A collection of constants used throughout the Agrigate platform
/// </summary>
public static class Constants
{
    /// <summary>
    /// The max amount of time that should we should wait for an actor to 
    /// provide a response
    /// </summary>
    public static readonly TimeSpan MaxActorWaitTime = TimeSpan.FromSeconds(5);
}