using FluentAssertions;
using RockLib.Messaging;
using System;
using System.Collections.Generic;
using Xunit;

namespace RockLib.DistributedTracing.Messaging.Tests
{
    using static HeaderNames;

    public class CorrelationIdExtensionsTests
    {
        private const string TestCorrelationIdHeader = "TestCorrelationId";

        [Fact(DisplayName = "GetCorrelationId returns the value of the correlation id header if found")]
        public void GetCorrelationIdHappyPath1()
        {
            var message = new SenderMessage("Hello, world!");
            var expectedCorrelationId = Guid.NewGuid().ToString();
            message.Headers[TestCorrelationIdHeader] = expectedCorrelationId;

            var correlationId = message.GetCorrelationId(TestCorrelationIdHeader);

            correlationId.Should().Be(expectedCorrelationId);
        }

        [Fact(DisplayName = "GetCorrelationId returns the value of the correlation id header if found using default currelation id header")]
        public void GetCorrelationIdHappyPath2()
        {
            var message = new SenderMessage("Hello, world!");
            var expectedCorrelationId = Guid.NewGuid().ToString();
            message.Headers[DefaultCorrelationIdHeader] = expectedCorrelationId;

            var correlationId = message.GetCorrelationId();

            correlationId.Should().Be(expectedCorrelationId);
        }

        [Fact(DisplayName = "GetCorrelationId adds correlation id header to message if not found")]
        public void GetCorrelationIdHappyPath3()
        {
            var message = new SenderMessage("Hello, world!");

            var correlationId = message.GetCorrelationId(TestCorrelationIdHeader);

            message.Headers[TestCorrelationIdHeader].Should().Be(correlationId);
        }

        [Fact(DisplayName = "GetCorrelationId adds correlation id header to message if not found using default currelation id header")]
        public void GetCorrelationIdHappyPath4()
        {
            var message = new SenderMessage("Hello, world!");

            var correlationId = message.GetCorrelationId();

            message.Headers[DefaultCorrelationIdHeader].Should().Be(correlationId);
        }

        [Fact(DisplayName = "GetCorrelationId throws if message parameter is null")]
        public void GetCorrelationIdSadPath1()
        {
            SenderMessage message = null;
            var correlationIdHeader = TestCorrelationIdHeader;

            Action act = () => message.GetCorrelationId(correlationIdHeader);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*message*");
        }

        [Fact(DisplayName = "GetCorrelationId throws if correlationIdHeader parameter is null")]
        public void GetCorrelationIdSadPath2()
        {
            var message = new SenderMessage("Hello, world!");
            string correlationIdHeader = null;

            Action act = () => message.GetCorrelationId(correlationIdHeader);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*correlationIdHeader*");
        }

        [Fact(DisplayName = "WithCorrelationId returns the same header dictionary if it contains correlation id")]
        public void WithCorrelationIdHappyPath1()
        {
            var correlationId = Guid.NewGuid().ToString();
            var headers = new Dictionary<string, object> { [TestCorrelationIdHeader] = correlationId };
            var originalHeaderDictionary = new HeaderDictionary(headers);

            var headerDictionary = originalHeaderDictionary.WithCorrelationId(TestCorrelationIdHeader);

            headerDictionary.Should().BeSameAs(originalHeaderDictionary);
        }

        [Fact(DisplayName = "WithCorrelationId returns the same header dictionary if it contains correlation id using default currelation id header")]
        public void WithCorrelationIdHappyPath2()
        {
            var correlationId = Guid.NewGuid().ToString();
            var headers = new Dictionary<string, object> { [DefaultCorrelationIdHeader] = correlationId };
            var originalHeaderDictionary = new HeaderDictionary(headers);

            var headerDictionary = originalHeaderDictionary.WithCorrelationId();

            headerDictionary.Should().BeSameAs(originalHeaderDictionary);
        }

        [Fact(DisplayName = "WithCorrelationId returns copy of header dictionary (with generated correlation id) if it does not contain correlation id")]
        public void WithCorrelationIdHappyPath3()
        {
            var headers = new Dictionary<string, object>();
            var originalHeaderDictionary = new HeaderDictionary(headers);

            var headerDictionary = originalHeaderDictionary.WithCorrelationId(TestCorrelationIdHeader);

            headerDictionary.Should().NotBeSameAs(originalHeaderDictionary);

            headerDictionary.Should().Contain(x => x.Key == TestCorrelationIdHeader)
                .Which.Value.Should().NotBeNull().And.BeOfType<string>();
        }

        [Fact(DisplayName = "WithCorrelationId returns copy of header dictionary (with generated correlation id) if it does not contain correlation id using default currelation id header")]
        public void WithCorrelationIdHappyPath4()
        {
            var headers = new Dictionary<string, object>();
            var originalHeaderDictionary = new HeaderDictionary(headers);

            var headerDictionary = originalHeaderDictionary.WithCorrelationId();

            headerDictionary.Should().NotBeSameAs(originalHeaderDictionary);

            headerDictionary.Should().Contain(x => x.Key == DefaultCorrelationIdHeader)
                .Which.Value.Should().NotBeNull().And.BeOfType<string>();
        }

        [Fact(DisplayName = "WithCorrelationId throws if headers parameter is null")]
        public void WithCorrelationIdSadPath1()
        {
            HeaderDictionary headers = null;
            var correlationIdHeader = TestCorrelationIdHeader;

            Action act = () => headers.WithCorrelationId(correlationIdHeader);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*headers*");
        }

        [Fact(DisplayName = "WithCorrelationId throws if correlationIdHeader parameter is null")]
        public void WithCorrelationIdSadPath2()
        {
            var headers = new HeaderDictionary(new Dictionary<string, object>());
            string correlationIdHeader = null;

            Action act = () => headers.WithCorrelationId(correlationIdHeader);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*correlationIdHeader*");
        }
    }
}
