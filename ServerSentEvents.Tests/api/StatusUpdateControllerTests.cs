using System;
using System.Net.Http;
using NSubstitute;
using NUnit.Framework;
using ServerSentEvents.Api;
using ServerSentEvents.Messaging;

namespace ServerSentEvents.Tests.api
{
    [TestFixture]
    public class StatusUpdateControllerTests
    {
        private StatusUpdatesController _sut;
        private IMessenger _messenger;

        [SetUp]
        public void Setup()
        {
            _messenger = Substitute.For<IMessenger>();
            _sut = new StatusUpdatesController(_messenger);
        }

        [Test]
        public void GivenSessionId_Delete_ShouldUnsubscribe()
        {
            const string expectedSessionId = "1234";

            _sut.Delete(expectedSessionId);

            _messenger.Received().Unsubscribe(expectedSessionId);
        }
        
        [Test]
        public void GivenSessionIdAsQueryString_Get_ShouldSubscribeForMessages()
        {
            const string expectedSessionId = "1234";
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("http://localhost:9000/api/statusupdate?sessionId=1234"));

            _sut.Get(request).Content.ReadAsStringAsync();

            _messenger.Received().Subscribe(Arg.Is<Subscriber>(subscriber => subscriber.SessionId == expectedSessionId));
        }

        [Test]
        public void GivenSessionIdAsQueryString_Get_ShouldUnsubscribePreviousSubscribtion()
        {
            const string expectedSessionId = "1234";
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("http://localhost:9000/api/statusupdate?sessionId=1234"));

            _sut.Get(request).Content.ReadAsStringAsync();

            _messenger.Received().Unsubscribe(expectedSessionId);
        }
    }
}
