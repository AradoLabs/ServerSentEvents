using System;
using System.IO;
using NUnit.Framework;
using ServerSentEvents.Messaging;

namespace ServerSentEvents.Tests.Messaging
{
    [TestFixture]
    public class MessengerTests
    {
        private Messenger _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new Messenger();
        }

        [TearDown]
        public void Teardown()
        {
            _sut.Dispose();
            _sut = null;
        }

        [Test]
        public void GivenSubscriber_Subscribe_ShouldSubscribeForMessages()
        {
            const string message = "testing";
            var sessionId = Guid.NewGuid().ToString();
            var stream = new MemoryStream();
            ArrangeSubscriber(stream, sessionId);

            _sut.SendMessage(sessionId, message);

            var result = GetMessageFrom(stream);
            Assert.IsTrue(result.Contains(message));
        }
        
        [Test]
        public void GivenSessionId_Unsubscribe_ShouldUnsubscribeForMessages()
        {
            const string message = "testing";
            var sessionId = Guid.NewGuid().ToString();
            var stream = new MemoryStream();
            ArrangeSubscriber(stream, sessionId);

            _sut.Unsubscribe(sessionId);
            _sut.SendMessage(sessionId, message);

            var result = GetMessageFrom(stream);
            Assert.IsFalse(result.Contains(message));
        }
        
        [Test]
        public void GivenSessionAndMessage_SendMessage_ShouldSendMessageInCorrectFormat()
        {
            const string message = "testing";
            const string expectedMessage = "data:testing\n\r\n";
            var sessionId = Guid.NewGuid().ToString();
            var stream = new MemoryStream();
            ArrangeSubscriber(stream, sessionId);

            _sut.SendMessage(sessionId, message);

            var result = GetMessageFrom(stream);
            Assert.IsTrue(result.Contains(expectedMessage));
        }
        
        [Test]
        public void GivenSessionAndMessage_SendMessage_ShouldSendMessageTwice()
        {
            const string message = "testing";
            const string expectedMessage = "data:testing\n\r\ndata:testing\n\r\n";
            var sessionId = Guid.NewGuid().ToString();
            var stream = new MemoryStream();
            ArrangeSubscriber(stream, sessionId);
            
            _sut.SendMessage(sessionId, message);

            var result = GetMessageFrom(stream);
            Assert.AreEqual(expectedMessage, result);
        }

        private void ArrangeSubscriber(MemoryStream stream, string sessionId)
        {
            var streamWriter = new StreamWriter(stream);
            var subscriber = new Subscriber()
            {
                SessionId = sessionId,
                Stream = streamWriter
            };
            
            _sut.Subscribe(subscriber);
        }
        
        private static string GetMessageFrom(Stream stream)
        {
            var streamReader = new StreamReader(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return streamReader.ReadToEnd();
        }
    }
}
