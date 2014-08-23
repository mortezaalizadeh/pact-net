﻿using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using PactNet.Mocks.MockHttpService.Nancy;
using Xunit;

namespace PactNet.Tests.Mocks.MockHttpService.Nancy
{
    public class NancyContextExtensionsTests
    {
        [Fact]
        public void SetMockInteraction_WithInteractionAndNoExistingItemsInNancyContext_AddsInteractionToNancyContextItems()
        {
            var context = new NancyContext();
            
            var interactions = new List<ProviderServiceInteraction>
            {
                new ProviderServiceInteraction { Request = new ProviderServiceRequest(), Response = new ProviderServiceResponse() }
            };

            context.SetMockInteraction(interactions);

            Assert.Equal(1, context.Items.Count);
            Assert.Equal(interactions, context.Items[Constants.PactMockInteractionsKey]);
        }

        [Fact]
        public void SetMockInteraction_WithInteractionAndExistingItemsInNancyContext_AddsInteractionToNancyContextItems()
        {
            var context = new NancyContext();
            context.Items.Add(new KeyValuePair<string, object>("test", "tester"));

            var interactions = new List<ProviderServiceInteraction>
            {
                new ProviderServiceInteraction { Request = new ProviderServiceRequest(), Response = new ProviderServiceResponse() },
                new ProviderServiceInteraction { Request = new ProviderServiceRequest(), Response = new ProviderServiceResponse() }
            };

            context.SetMockInteraction(interactions);

            Assert.Equal(2, context.Items.Count);
            Assert.Equal(interactions, context.Items[Constants.PactMockInteractionsKey]);
        }

        [Fact]
        public void SetMockInteraction_WithInteractionAndExistingInteractionsInNancyContext_OverwritesInteractionsInNancyContextItem()
        {
            var context = new NancyContext();
            context.Items.Add(new KeyValuePair<string, object>(Constants.PactMockInteractionsKey, new List<KeyValuePair<ProviderServiceRequest, ProviderServiceResponse>>
            {
                new KeyValuePair<ProviderServiceRequest, ProviderServiceResponse>(new ProviderServiceRequest(), new ProviderServiceResponse()),
                new KeyValuePair<ProviderServiceRequest, ProviderServiceResponse>(new ProviderServiceRequest(), new ProviderServiceResponse())
            }));

            var interactions = new List<ProviderServiceInteraction>()
            {
                new ProviderServiceInteraction { Request = new ProviderServiceRequest(), Response = new ProviderServiceResponse()},
                new ProviderServiceInteraction { Request = new ProviderServiceRequest(), Response = new ProviderServiceResponse()}
            };

            context.SetMockInteraction(interactions);

            Assert.Equal(1, context.Items.Count);
            Assert.Equal(interactions, context.Items[Constants.PactMockInteractionsKey]);
        }

        [Fact]
        public void GetMatchingInteraction_WithNoInteractions_ThrowsInvalidOperationException()
        {
            var context = new NancyContext();

            Assert.Throws<InvalidOperationException>(() => context.GetMatchingInteraction(HttpVerb.Get, "/events"));
        }

        [Fact]
        public void GetMatchingInteraction_WithInteractionsNull_ThrowsInvalidOperationException()
        {
            var context = new NancyContext();
            context.Items[Constants.PactMockInteractionsKey] = null;

            Assert.Throws<InvalidOperationException>(() => context.GetMatchingInteraction(HttpVerb.Get, "/events"));
        }

        [Fact]
        public void GetMatchingInteraction_WithNoMatchingInteraction_ThrowsInvalidOperationException()
        {
            var interactions = new List<ProviderServiceInteraction>
            {
                new ProviderServiceInteraction { Request = new ProviderServiceRequest { Method = HttpVerb.Get, Path = "/hello" }, Response = new ProviderServiceResponse()}
            }; 

            var context = new NancyContext();
            context.SetMockInteraction(interactions);

            Assert.Throws<InvalidOperationException>(() => context.GetMatchingInteraction(HttpVerb.Get, "/events"));
        }

        [Fact]
        public void GetMatchingInteraction_WithMoreThanOneMatchingInteraction_ThrowsInvalidOperationException()
        {
            var requestResponsePairs = new List<ProviderServiceInteraction>
            {
                new ProviderServiceInteraction { Request = new ProviderServiceRequest { Method = HttpVerb.Get, Path = "/events" }, Response = new ProviderServiceResponse() },
                new ProviderServiceInteraction { Request = new ProviderServiceRequest { Method = HttpVerb.Get, Path = "/events" }, Response = new ProviderServiceResponse()},
            };

            var context = new NancyContext();
            context.SetMockInteraction(requestResponsePairs);

            Assert.Throws<InvalidOperationException>(() => context.GetMatchingInteraction(HttpVerb.Get, "/events"));
        }

        [Fact]
        public void GetMatchingInteraction_WithOneMatchingInteraction_ReturnsInteraction()
        {
            var interactions = new List<ProviderServiceInteraction>
            {
                new ProviderServiceInteraction { Request = new ProviderServiceRequest { Method = HttpVerb.Get, Path = "/events" }, Response = new ProviderServiceResponse()},
                new ProviderServiceInteraction { Request = new ProviderServiceRequest { Method = HttpVerb.Post, Path = "/events" }, Response = new ProviderServiceResponse()},
            };

            var context = new NancyContext();
            context.SetMockInteraction(interactions);

            var result = context.GetMatchingInteraction(HttpVerb.Get, "/events");

            Assert.Equal(interactions.First(), result);
        }
    }
}
