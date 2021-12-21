using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Refit; // InterfaceStubGenerator looks for this

using RichardSzalay.MockHttp;
using Xunit;

namespace Refit.Tests
{
    public interface IUrlParameterApi
    {
        [Get("")]
        Task Get([Url] string url);

        [Get("")]
        Task Get([Url] Uri url);

        [Post("")]
        Task Post([Url] string url, [Body] string body);
    }

    public class UrlParameterTests
    {
        [Fact]
        public async Task CanMakeGetRequestWithUrlStringParameter()
        {
            var mockHttp = new MockHttpMessageHandler();
            var settings = new RefitSettings
            {
                HttpMessageHandlerFactory = () => mockHttp
            };

            mockHttp.Expect(HttpMethod.Get, "http://foo")
                .Respond("application/json", "Ok");

            var fixture = RestService.For<IUrlParameterApi>("http://bar", settings);

            await fixture.Get("http://foo");

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task CanMakeGetRequestWithUriParameter()
        {
            var mockHttp = new MockHttpMessageHandler();
            var settings = new RefitSettings
            {
                HttpMessageHandlerFactory = () => mockHttp
            };

            mockHttp.Expect(HttpMethod.Get, "http://foo")
                .Respond("application/json", "Ok");

            var fixture = RestService.For<IUrlParameterApi>("http://bar", settings);

            await fixture.Get(new Uri("http://foo"));

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task CanMakeGetRequestWithAbsolutePathUrlStringParameter()
        {
            var mockHttp = new MockHttpMessageHandler();
            var settings = new RefitSettings
            {
                HttpMessageHandlerFactory = () => mockHttp
            };

            mockHttp.Expect(HttpMethod.Get, "http://foo/bar")
                .Respond("application/json", "Ok");

            var fixture = RestService.For<IUrlParameterApi>("http://foo", settings);

            await fixture.Get("/bar");

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task CanMakeGetRequestWithAbsolutePathUriParameter()
        {
            var mockHttp = new MockHttpMessageHandler();
            var settings = new RefitSettings
            {
                HttpMessageHandlerFactory = () => mockHttp
            };

            mockHttp.Expect(HttpMethod.Get, "http://foo/bar")
                .Respond("application/json", "Ok");

            var fixture = RestService.For<IUrlParameterApi>("http://foo", settings);

            await fixture.Get(new Uri("/bar", UriKind.Relative));

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task CanMakePostRequestWithUrlStringParameter()
        {
            var mockHttp = new MockHttpMessageHandler();
            var settings = new RefitSettings
            {
                HttpMessageHandlerFactory = () => mockHttp
            };

            mockHttp.Expect(HttpMethod.Post, "http://httpbin.org/foo")
                    .WithContent("raw string")
                    .Respond(HttpStatusCode.OK);

            var fixture = RestService.For<IUrlParameterApi>("http://bar", settings);

            await fixture.Post("http://httpbin.org/foo", "raw string");

            mockHttp.VerifyNoOutstandingExpectation();
        }
    }
}
