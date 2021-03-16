using FluentAssertions;
using RockLib.Messaging.Testing;
using System;
using Xunit;
using static RockLib.DistributedTracing.Messaging.HeaderNames;

namespace RockLib.Messaging.Tests
{
    public class CorrelationIdExtensionsTests
    {
        private const string TestCorrelationIdHeader = "TestCorrelationId";

        [Fact(DisplayName = "SetCorrelationId sets the correlation id header")]
        public void SetCorrelationIdHappyPath1()
        {
            var message = new SenderMessage("Hello, world!");
            var correlationId = Guid.NewGuid().ToString();

            var returnValue = message.SetCorrelationId(correlationId, TestCorrelationIdHeader);

            message.Headers.Should().ContainKey(TestCorrelationIdHeader)
                .WhichValue.Should().Be(correlationId);

            returnValue.Should().BeSameAs(message);
        }

        [Fact(DisplayName = "SetCorrelationId sets the correlation id header using default currelation id header")]
        public void SetCorrelationIdHappyPath2()
        {
            var message = new SenderMessage("Hello, world!");
            var correlationId = Guid.NewGuid().ToString();

            var returnValue = message.SetCorrelationId(correlationId);

            message.Headers.Should().ContainKey(DefaultCorrelationIdHeader)
                .WhichValue.Should().Be(correlationId);

            returnValue.Should().BeSameAs(message);
        }

        [Fact(DisplayName = "SetCorrelationId throws if message parameter is null")]
        public void SetCorrelationIdSadPath1()
        {
            SenderMessage message = null;
            var correlationId = Guid.NewGuid().ToString();
            var correlationIdHeader = TestCorrelationIdHeader;

            Action act = () => message.SetCorrelationId(correlationId, correlationIdHeader);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*message*");
        }

        [Fact(DisplayName = "SetCorrelationId throws if correlationId parameter is null")]
        public void SetCorrelationIdSadPath2()
        {
            var message = new SenderMessage("Hello, world!");
            string correlationId = null;
            var correlationIdHeader = TestCorrelationIdHeader;

            Action act = () => message.SetCorrelationId(correlationId, correlationIdHeader);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*correlationId*");
        }

        [Fact(DisplayName = "SetCorrelationId throws if correlationIdHeader parameter is null")]
        public void SetCorrelationIdSadPath3()
        {
            var message = new SenderMessage("Hello, world!");
            var correlationId = Guid.NewGuid().ToString();
            string correlationIdHeader = null;

            Action act = () => message.SetCorrelationId(correlationId, correlationIdHeader);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*correlationIdHeader*");
        }

        [Fact(DisplayName = "GetCorrelationId returns the value of the correlation id header if found")]
        public void GetCorrelationIdHappyPath1()
        {
            var message = new FakeReceiverMessage("Hello, world!");
            var expectedCorrelationId = Guid.NewGuid().ToString();
            message.Headers[TestCorrelationIdHeader] = expectedCorrelationId;

            var correlationId = message.GetCorrelationId(TestCorrelationIdHeader);

            correlationId.Should().Be(expectedCorrelationId);
        }

        [Fact(DisplayName = "GetCorrelationId returns the value of the correlation id header if found using default currelation id header")]
        public void GetCorrelationIdHappyPath2()
        {
            var message = new FakeReceiverMessage("Hello, world!");
            var expectedCorrelationId = Guid.NewGuid().ToString();
            message.Headers[DefaultCorrelationIdHeader] = expectedCorrelationId;

            var correlationId = message.GetCorrelationId();

            correlationId.Should().Be(expectedCorrelationId);
        }

        [Fact(DisplayName = "GetCorrelationId returns null if the correlation id header is not found")]
        public void GetCorrelationIdHappyPath3()
        {
            var message = new FakeReceiverMessage("Hello, world!");

            var correlationId = message.GetCorrelationId(TestCorrelationIdHeader);

            correlationId.Should().BeNull();
        }

        [Fact(DisplayName = "GetCorrelationId returns null if the correlation id header is not found using default currelation id header")]
        public void GetCorrelationIdHappyPath4()
        {
            var message = new FakeReceiverMessage("Hello, world!");

            var correlationId = message.GetCorrelationId();

            correlationId.Should().BeNull();
        }

        [Fact(DisplayName = "GetCorrelationId throws if message parameter is null")]
        public void GetCorrelationIdSadPath1()
        {
            IReceiverMessage message = null;
            var correlationIdHeader = TestCorrelationIdHeader;

            Action act = () => message.GetCorrelationId(correlationIdHeader);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*message*");
        }

        [Fact(DisplayName = "GetCorrelationId throws if correlationIdHeader parameter is null")]
        public void GetCorrelationIdSadPath2()
        {
            var message = new FakeReceiverMessage("Hello, world!");
            string correlationIdHeader = null;

            Action act = () => message.GetCorrelationId(correlationIdHeader);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*correlationIdHeader*");
        }
    }
}
