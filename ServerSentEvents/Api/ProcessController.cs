using System.Web.Http;
using ServerSentEvents.Process;

namespace ServerSentEvents.Api
{
    public class ProcessController : ApiController
    {
        public ProcessController(IProcess process)
        {
            _process = process;
        }

        public void Post(Form form)
        {
            _process.Start(form);
        }
        
        private readonly IProcess _process;
    }
}