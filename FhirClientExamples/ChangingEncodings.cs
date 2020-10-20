using System.ComponentModel;
using FluentAssertions;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using Xunit;
using Task = System.Threading.Tasks.Task;
using static FhirClientExamples.Utilities;
#pragma warning disable 612 - Litterally no other way to check the request sent by the FHIR client

namespace FhirClientExamples
{
    [Description("Theses tests will cover various was you can have the client treat incoming messages")]
    public class ChangingEncodings
    {
        [Fact]
        [Description("The header can be changed using the preferred format to accept XML or JSON. This can be useful when talking to a facade and not a server.")]
        public async Task SetResponseToXml()
        {
            FhirClient fhirClient = new FhirClient(CrispDevFhirServer);
            // TODO: Set client to preferred format of the FHIR Client to XML

            await fhirClient.GetAsync("/metadata");
            fhirClient.LastRequest.Accept.Should().Be("application/fhir+xml;charset=utf-8");
        }

        [Fact]
        [Description("Changing the parser settings can help with attempts to deserialize resources that are not " +
                     "totally 'conformant'.")]
        public async Task AllowingPermissiveParsing()
        {
            FhirClient fhirClient = new FhirClient("https://testapi.crisphealth.org/FHIR/");

            fhirClient.OnBeforeRequest += (sender, args) =>
            {
                // TODO: Add your thumbprint
                args.RawRequest.ClientCertificates.Add(GetCertByThumbprint(""));
            };

            // TODO: Set the clients parser to allow permissive parsing and not fail on error.
            fhirClient.PreferredFormat = ResourceFormat.Json;

            // TODO: Set the Parser Settings to be permissive
            fhirClient.ParserSettings = new ParserSettings
            {
            };

            // Don't worry about what is going on here for now.
            try
            {
                var diagnosticReport =
                    await fhirClient.SearchAsync<DiagnosticReport>(new SearchParams().Add("patient", "79559712"));
            }
            catch (FhirOperationException fhirOperationException)
            {
                // Not everything can be saved.
                fhirOperationException.Outcome.Should().NotBeNull();
            }

        }
    }
}