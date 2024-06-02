namespace Agrigate.Core.Actors;
using Akka.Actor;
using Akka.DependencyInjection;

/// <summary>
/// A base actor for the Agrigate platform with common functionality
/// </summary>
public abstract class AgrigateActor : ReceiveActor
{
    public AgrigateActor()
    {
    }

    /// <summary>
    /// Forwards a message to a specified type, TActor. TActor should be short
    /// lived and terminate itself after handling the message
    /// </summary>
    /// <param name="message">The message that should be forwarded to the 
    /// short-lived query actor</param>
    protected static void AskFor<TMessage, TActor>(TMessage message)
        where TMessage : class
        where TActor: ActorBase
    {
        var queryProps = DependencyResolver.For(Context.System).Props<TActor>();
        var queryHandler = Context.ActorOf(queryProps);

        queryHandler.Forward(message);
    }
}