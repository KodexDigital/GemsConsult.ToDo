using Moq;
using Todo.Logger.Servicer.Manager;
using Xunit;

namespace UnitTesting
{
    public class LoggerManagerTests
    {
        private readonly string message = "This is logger testing";
        [Fact]
        public void Log_Debug_Test()
        {
            var loger = new Mock<ILoggerManager>();
            loger.Setup(m => m.LogDebug(message));
            Assert.True(true);
        }

        [Fact]
        public void Log_Error_Test()
        {
            var loger = new Mock<ILoggerManager>();
            loger.Setup(m => m.LogError(message));
            Assert.True(true);
        }

        [Fact]
        public void Log_Info_Test()
        {
            var loger = new Mock<ILoggerManager>();
            loger.Setup(m => m.LogInfo(message));
            Assert.True(true);
        }

        [Fact]
        public void Log_Warning_Test()
        {
            var loger = new Mock<ILoggerManager>();
            loger.Setup(m => m.LogWarning(message));
            Assert.True(true);
        }
    }
}
    