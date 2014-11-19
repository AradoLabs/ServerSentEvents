namespace ServerSentEvents.Messaging
{
    public interface IMessenger
    {
        void Subscribe(Subscriber subscriber);
        void Unsubscribe(string sessionId);
        void SendMessage(string sessionId, string message);
    }
}