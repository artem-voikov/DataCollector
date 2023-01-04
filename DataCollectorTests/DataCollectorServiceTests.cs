using AutoFixture;
using DataCollector;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using RabbitMQ.Client;
using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace DataCollectorTests
{
    public class DataCollectorServiceTests
    {
        private Mock<IOptions<TrackerSettings>> settings = new();
        private Mock<IModel> channel = new();
        private Fixture fixture = new Fixture();
        private DataCollectorService subject;

        public DataCollectorServiceTests()
        {
            settings.Setup(x => x.Value).Returns(new TrackerSettings());
            subject = new DataCollectorService(channel.Object, settings.Object);
        }

        public HttpRequest DefaultRequest => new DefaultHttpContext().Request; 

        //[Fact]
        //public async Task Method_When_Then() 
        //{
        //    Assert.True(false);    
        //}


        [Fact]
        public async Task Method_When_Then()
        {
            var result = await subject.Collect(DefaultRequest, CancellationToken.None);
            Assert.True(false);
        }

    }
}
