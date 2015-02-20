#region

using System;
using System.Security.Cryptography.X509Certificates;

#endregion

namespace Superfish_Removal_Tool.Removers
{
    internal class CertificateRemover : ISuperfishRemover
    {
        private const string CERT_ISSUER = "Superfish, Inc.";

        #region Implementation of ISuperfishRemover

        public string Name
        {
            get { return "Root Certificate"; }
        }

        public bool NeedsRemoved()
        {
            return GetCertificate() != null;
        }

        public SuperfishRemovalResult Remove()
        {
            try
            {
                var cert = GetCertificate();

                if (cert != null)
                {
                    var store = GetStore();
                    store.Open(OpenFlags.MaxAllowed);
                    store.Remove(cert);
                    store.Close();
                }
            }

            catch (Exception ex)
            {
                return new SuperfishRemovalResult(false, ex);
            }

            return new SuperfishRemovalResult(true);
        }

        #endregion

        private static X509Store GetStore()
        {
            return new X509Store(StoreName.Root, StoreLocation.CurrentUser);
        }

        private static X509Certificate2 GetCertificate()
        {
            X509Certificate2 superfishCert = null;

            var store = GetStore();
            store.Open(OpenFlags.ReadOnly);

            var certs = store.Certificates.Find(X509FindType.FindByIssuerName, CERT_ISSUER, true);

            if (certs.Count > 0)
                superfishCert = certs[0];

            store.Close();

            return superfishCert;
        }
    }
}