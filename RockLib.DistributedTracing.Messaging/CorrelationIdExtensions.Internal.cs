using RockLib.Messaging;
using System;
using System.Collections.Generic;

namespace RockLib.DistributedTracing.Messaging
{
    using static HeaderNames;

    /// <summary>
    /// Extension methods that ensure message headers contain a value for correlation id.
    /// </summary>
    public static class CorrelationIdExtensions
    {
        /// <summary>
        /// Gets the value of the message's correlation id header. If the message does not have a
        /// correlation id header, one is added to the message and set to a new <see cref="Guid"/>
        /// value.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="correlationIdHeader">The name of the correlation id header.</param>
        /// <returns>The value of the message's correlation id header.</returns>
        public static string GetCorrelationId(this SenderMessage message, string correlationIdHeader = DefaultCorrelationIdHeader)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));
            if (correlationIdHeader is null)
                throw new ArgumentNullException(nameof(correlationIdHeader));

            if (message.Headers.TryGetValue(correlationIdHeader, out object correlationIdValue) && correlationIdValue != null)
                return correlationIdValue.ToString();

            var correlationId = Guid.NewGuid().ToString();
            message.Headers[correlationIdHeader] = correlationId;
            return correlationId;
        }

        /// <summary>
        /// Ensures that the returned <see cref="HeaderDictionary"/> has a correlation id header.
        /// </summary>
        /// <param name="headers">A header dictionary that could have a correlation id header.</param>
        /// <param name="correlationIdHeader">The name of the correlation id header.</param>
        /// <returns>
        /// The <paramref name="headers"/> parameter, if it contains a non-null correlation id
        /// header; otherwise a new <see cref="HeaderDictionary"/> with the same items as the
        /// <paramref name="headers"/> parameter along with a correlation id header set to a new
        /// <see cref="Guid"/> value.
        /// </returns>
        public static HeaderDictionary WithCorrelationId(this HeaderDictionary headers, string correlationIdHeader = DefaultCorrelationIdHeader)
        {
            if (headers is null)
                throw new ArgumentNullException(nameof(headers));
            if (correlationIdHeader is null)
                throw new ArgumentNullException(nameof(correlationIdHeader));

            if (headers.TryGetValue(correlationIdHeader, out string correlationId) && correlationId != null)
                return headers;

            var headerDictionary = new Dictionary<string, object>();
            foreach (var header in headers)
                headerDictionary.Add(header.Key, header.Value);
            headerDictionary[correlationIdHeader] = Guid.NewGuid().ToString();
            return new HeaderDictionary(headerDictionary);
        }
    }
}
