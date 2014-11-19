using System.Threading;
using ServerSentEvents.Messaging;

namespace ServerSentEvents.Process
{
    public class FakeProcess : IProcess
    {
        public FakeProcess(IMessenger messenger)
        {
            _messenger = messenger;
        }

        public void Start(Form form)
        {
            _messenger.SendMessage(form.SessionId, "Process iniated for " + form.Name);

            for (var i = 0; i < 5; i++)
            {
                var message = string.Format("step {0} done", i);

                Thread.Sleep(500);
                _messenger.SendMessage(form.SessionId, message);
                Thread.Sleep(500);
            }

            _messenger.SendMessage(form.SessionId, "Process completed for " + form.Name);
        }

        private readonly IMessenger _messenger;
    }
}