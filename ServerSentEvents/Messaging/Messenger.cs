using System;
using System.Collections.Concurrent;
using System.Linq;
using WebGrease.Css.Extensions;

namespace ServerSentEvents.Messaging
{
    public class Messenger : IMessenger, IDisposable
    {
        public void Dispose()
        {
            Subscribers.ForEach(s => Subscribers.TryDequeue(out s));
        }

        public void Subscribe(Subscriber subscriber)
        {
            Subscribers.Enqueue(subscriber);
        }

        public void Unsubscribe(string sessionId)
        {
            Subscribers
                .Where(subscriber => subscriber.SessionId == sessionId)
                .ForEach(Unsubscribe);
        }

        public void SendMessage(string sessionId, string message)
        {
            Subscribers
                .Where(susbscriber => susbscriber.SessionId == sessionId)
                .ForEach(subscriber => SendMessage(subscriber, sessionId, message));
        }

        private static void Unsubscribe(Subscriber subscriber)
        {
            var result = Subscribers.TryDequeue(out subscriber);
            if (result == false)
            {
                throw new InvalidOperationException(string.Format("Could not unsubscribe subscriber: '{0}'", subscriber.SessionId));
            }
        }

        private void SendMessage(Subscriber subscriber, string sessionId, string message)
        {
            try
            {
                subscriber.Stream.WriteLine("data:" + message + "\n");
                subscriber.Stream.Flush();
                subscriber.Stream.WriteLine("data:" + message + "\n");
                subscriber.Stream.Flush();
                //Bug in the streamwriter. Need to send twice to make 100% sure that the last message is also sent.
            }
            catch (Exception)
            {
                Unsubscribe(sessionId);
            }
        }

        private static readonly ConcurrentQueue<Subscriber> Subscribers = new ConcurrentQueue<Subscriber>();
    }
}