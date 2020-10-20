using System.ComponentModel;
using FluentAssertions;
using Hl7.Fhir.Rest;
using Xunit;
using Task = System.Threading.Tasks.Task;
using static FhirClientExamples.Utilities;
#pragma warning disable 612 - Litterally no other way to check the request sent by the FHIR client

namespace FhirClientExamples
{
    [Description("This section of tests will cover the basics of creating a connection to a FHIR server and " +
                 "authorization mechanisms available to the client.")]
    public class GettingStarted
    {
        [Fact]
        [Description("Setting up a connection to a server.")]
        public void CreateAFhirClient()
        {
            // TODO: Fix this code such that the unit test will pass.
            FhirClient fhirClient = new FhirClient("");
            fhirClient.Endpoint.Should().Be(CrispDevFhirServer);
        }

        [Description("All authentication options in FHIR .NET API are implemented by adding headers before issuing the request.")]
        public class Authorization
        {
            [Fact]
            public async Task AuthenticationWithBasic()
            {
                FhirClient fhirClient = new FhirClient(CrispDevFhirServer);
                fhirClient.OnBeforeRequest += (object sender, BeforeRequestEventArgs e) =>
                {
                    // TODO: Authenticate into the server by adding the basic authorization "username:password"
                };

                await fhirClient.GetAsync("/metadata");
                fhirClient.LastRequest.Headers.Get("Authorization")
                    .Should()
                    .Be("Basic dXNlcm5hbWU6cGFzc3dvcmQ=");
            }

            [Fact]
            [Description("Using Bear Tokens. A Fhir client is not capable of getting a token but can use them.")]
            public async Task AuthenticationWithBearer()
            {
                FhirClient fhirClient = new FhirClient(CrispDevFhirServer);
                fhirClient.OnBeforeRequest += (object sender, BeforeRequestEventArgs e) =>
                {
                    // TODO: Add a header to the request using the bearer token ya29.QQIBibTwvKkE39hY8mdkT_mXZoRh7Ub9cK9hNsqrxem4QJ6sQa36VHfyuBe
                };

                await fhirClient.GetAsync("/metadata");

                fhirClient.LastRequest.Headers.Get("Authorization")
                    .Should()
                    .Be("Bearer ya29.QQIBibTwvKkE39hY8mdkT_mXZoRh7Ub9cK9hNsqrxem4QJ6sQa36VHfyuBe");
            }

            [Fact]
            [Description("Adding your certificate to the client. This is similiar to how our service clients work.")]
            public void AuthenticationWithCert()
            {
                FhirClient fhirClient = new FhirClient(CrispDevFhirServer);
                fhirClient.OnBeforeRequest += (sender, args) =>
                {
                    // TODO: Add a certificate to the client. You may need to write your own method to get it or
                    // use the `Utilities.GetCertByThumbprint` method provided.
                };

                fhirClient.GetAsync("/metadata");
                fhirClient.LastRequest.ClientCertificates.Should().HaveCount(1);
            }
        }
    }
}