using System.IO;

namespace ServerSentEvents.Messaging
{
    public class Subscriber
    {
        public string SessionId { get; set; }
        public StreamWriter Stream { get; set; }
    }
}