using AutoFixture;
using Moq;
using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataStorageTests
{
    public class TrackRepositoryTests
    {
        Fixture fixture = new Fixture();
        private Mock<IStorage> storage = new Mock<IStorage>();
        private TrackRepository subject;

        public TrackRepositoryTests()
        {
            subject = new TrackRepository(storage.Object);
        }

        //[Fact]
        //public async Task Method_When_Then() 
        //{
        //    Assert.True(false);    
        //}

        public class MessagesData : IEnumerable<object[]>
        {
            Fixture fixture = new Fixture();
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { JsonSerializer.Serialize(fixture.Create<TrackRequest>()), "" };
                yield return new object[] { JsonSerializer.Serialize(fixture.Build<TrackRequest>().Without(x => x.Referer).Create()), "null" }; //TODO: etc
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(MessagesData))]
        public async Task Store_WhenGivenJsonMessage_ThenMakeProperRow(string message, string expected)
        {
            await subject.Store(message);

            string result = string.Empty;
            storage.Setup(x => x.Log(It.IsAny<string>())).Callback<string>(x => result = x);

            Assert.Contains(expected, result);
        }


    }
}