using System;
using static RockLib.DistributedTracing.Messaging.HeaderNames;

namespace RockLib.Messaging
{
    /// <summary>
    /// Extension methods for getting and setting a message's correlation id header.
    /// </summary>
    public static class CorrelationIdExtensions
    {
        /// <summary>
        /// Sets the correlation id header of the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="correlationIdHeader">The name of the correlation id header.</param>
        /// <returns>The <paramref name="message"/> parameter.</returns>
        public static SenderMessage SetCorrelationId(this SenderMessage message, string correlationId, string correlationIdHeader = DefaultCorrelationIdHeader)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));
            if (correlationId is null)
                throw new ArgumentNullException(nameof(correlationIdHeader));
            if (correlationIdHeader is null)
                throw new ArgumentNullException(nameof(correlationIdHeader));

            message.Headers[correlationIdHeader] = correlationId;

            return message;
        }

        /// <summary>
        /// Gets the value of the correlation id header of the message if it exists.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="correlationIdHeader">The name of the correlation id header.</param>
        /// <returns>
        /// The value of the correlation id header, or <see langword="null"/> if the correlation
        /// id header does not exist.
        /// </returns>
        public static string GetCorrelationId(this IReceiverMessage message, string correlationIdHeader = DefaultCorrelationIdHeader)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));
            if (correlationIdHeader is null)
                throw new ArgumentNullException(nameof(correlationIdHeader));

            if (message.Headers.TryGetValue(correlationIdHeader, out string correlationIdValue))
                return correlationIdValue;

            return null;
        }
    }
}
