using NSubstitute;
using NUnit.Framework;
using ServerSentEvents.Api;
using ServerSentEvents.Process;

namespace ServerSentEvents.Tests.api
{
    [TestFixture]
    public class ProcessControllerTests
    {
        private ProcessController _sut;
        private IProcess _process;

        [SetUp]
        public void Setup()
        {
            _process = Substitute.For<IProcess>();
            _sut = new ProcessController(_process);
        }

        [Test]
        public void GivenForm_Post_ShouldStartProcess()
        {
            var form = new Form();

            _sut.Post(form);

            _process.Received().Start(form);
        }
    }
}
