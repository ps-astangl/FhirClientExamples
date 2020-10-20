using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Hl7.Fhir.Rest;

namespace FhirClientExamples
{
    public static class Utilities
    {
        public const string CrispDevFhirServer = "https://fhirsql.crisphealth-build.org/";
        public static X509Certificate2 GetCertByThumbprint(string certThumbPrint)
        {
            X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            certStore.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, certThumbPrint, false);
            return certCollection[0];
        }
    }
}