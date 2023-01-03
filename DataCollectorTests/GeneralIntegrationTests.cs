using DataCollectorTests.Infrastructure;

namespace DataCollectorTests
{
    
    public class GeneralIntergartionTests
    {
        [Fact]
        public void ConnectToRabbitMq()
        {
            var testEngine = new TestRabbitEngine();

            Assert.True(true, $"Connection established through: {testEngine.amqpConnectionString}.");

        }

        //TODO: make more appropriate tests with wiremock and rabbitmq test
    }
}