using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using ServerSentEvents.Messaging;

namespace ServerSentEvents.Api
{
    public class StatusUpdatesController : ApiController
    {
        public StatusUpdatesController(IMessenger messenger)
        {
            _messenger = messenger;
        }

        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            var response = request.CreateResponse();
            response.Content = new PushStreamContent((stream, content, context) => SubscribeForMessages(request, stream), "text/event-stream");
            return response;
        }

        public void Delete(string id)
        {
            _messenger.Unsubscribe(id);
        }

        private void SubscribeForMessages(HttpRequestMessage request, Stream stream)
        {
            var sessionId = GetSessionIdOrDefaultFrom(request);
            if (string.IsNullOrWhiteSpace(sessionId)) return;

            var subscriber = new Subscriber
            {
                SessionId = sessionId,
                Stream = new StreamWriter(stream)
            };

            _messenger.Unsubscribe(sessionId);
            _messenger.Subscribe(subscriber);
        }

        private static string GetSessionIdOrDefaultFrom(HttpRequestMessage request)
        {
            return request.GetQueryNameValuePairs().ToList().SingleOrDefault(s => s.Key == @"sessionId").Value;
        }

        private readonly IMessenger _messenger;
    }
}
