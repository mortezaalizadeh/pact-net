using System;
using PactNet.Verifier.Messaging;

namespace PactNet.Verifier
{
    /// <summary>
    /// Pact verifier
    /// </summary>
    public interface IPactVerifier
    {
        /// <summary>
        /// Set the provider details
        /// </summary>
        /// <param name="providerName">Name of the provider</param>
        /// <param name="pactUri">URI of the running service</param>
        /// <returns>Fluent builder</returns>
        IPactVerifierProvider ServiceProvider(string providerName, Uri pactUri);

        /// <summary>
        /// Set the provider details of a messaging provider
        /// </summary>
        /// <param name="providerName">Name of the provider</param>
        /// <param name="pactUri">URI of the running service</param>
        /// <param name="basePath">Path of the messaging provider endpoint</param>
        /// <returns>Fluent builder</returns>
        IPactVerifierMessagingProvider MessagingProvider(string providerName, Uri pactUri, string basePath);

        /// <summary>
        /// Sets the HTTP request timeout for requests to the target API and for state change requests.
        /// </summary>
        /// <param name="requestTimeout">The timespan represents the request timeout</param>
        /// <returns>The modified PactVerifier instance</returns>
        IPactVerifier WithRequestTimeout(TimeSpan requestTimeout);
    }
}