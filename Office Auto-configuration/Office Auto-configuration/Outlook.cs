using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Win32;

namespace Office_Auto_configuration
{
    internal static class Outlook
    {
        internal static void Configure()
        {
            Console.Clear();
            Console.WriteLine("Outlook Configuration");
            ConfigureAutodiscover();
            InstallCertificate();
            ConfigureRegistryKeys();
            Console.WriteLine(Resources.TextPressAnyKeyToContinue);
            Console.ReadKey();
        }

        internal static void ConfigureAutodiscover()
        {
            System.IO.File.WriteAllText(Resources.OutlookAutodiscoverDestPath, Resources.OutlookAutodiscoverContent);
            Console.WriteLine(Resources.TextOutlookAutodiscoverExistence + ": " + (System.IO.File.Exists(Resources.OutlookAutodiscoverDestPath) ? Resources.TextSuccess : Resources.TextFailed));
        }

        private static void InstallCertificate()
        {

            X509Certificate2 cert;
            try
            {
                cert = new X509Certificate2(Resources.AddPath(Resources.OutlookServerDomainCertificateFileName));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{Resources.TextCertificateInstallation}: {Resources.TextFailed} - {ex.Message}");
                return;
            }

            //Для пользователя
            //var store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            //Для данного компьютера 
            //?? X509Store store = new X509Store(StoreName.CertificateAuthority, StoreLocation.LocalMachine);
            var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);

            X509Certificate2Collection certs = store.Certificates.Find(X509FindType.FindBySubjectName, Resources.LyncServerDomainCertificateName, true);
            if (certs.Count > 0)
            {
                for (int i = 0; i < certs.Count; ++i)
                {
                    if (certs[i].Thumbprint != cert.Thumbprint) continue;
                    Console.WriteLine($"{Resources.TextCertificateInstallation}: {Resources.TextSuccess}");
                    store.Close();
                    return;
                }
            }

            store.Open(OpenFlags.ReadWrite);
            try
            {
                store.Add(cert);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{Resources.TextCertificateInstallation}: {Resources.TextFailed} - {ex.Message}");
                store.Close();
                return;
            }
            certs = store.Certificates.Find(X509FindType.FindByThumbprint, cert.Thumbprint, true);
            if (certs.Count > 0)
            {
                for (int i = 0; i < certs.Count; ++i)
                {
                    if (certs[i].Thumbprint != cert.Thumbprint) continue;
                    Console.WriteLine($"{Resources.TextCertificateInstallation}: {Resources.TextSuccess}");
                    store.Close();
                    return;
                }
            }
            Console.WriteLine($"{Resources.TextCertificateInstallation}: {Resources.TextFailed}");
            store.Close();
        }

        private static void ConfigureRegistryKeys()
        {
            bool success = true;
            RegistryKey currentKey = Resources.CurrentUser;
            currentKey = currentKey.OpenSubKey("software")?.OpenSubKey("microsoft")?.OpenSubKey("office");
            if (currentKey == null)
            {
                success = false;
                //TODO: Пересмотреть логику.
                //InstallOffice();
                //Console.WriteLine("You should install office before.");
            }
            else
            {
                var officeVersions = new List<string>();
                foreach (string subKeyName in currentKey.GetSubKeyNames())
                {
                    if (subKeyName.Contains(".0"))
                        officeVersions.Add(subKeyName);
                }
                bool outlookInstalled = false;
                foreach (string version in officeVersions)
                {
                    RegistryKey lyncKey = currentKey.OpenSubKey(version)?.OpenSubKey("outlook", true);
                    if (lyncKey != null)
                    {
                        RegistryKey tmp = lyncKey.OpenSubKey("autodiscover", true);
                        if (tmp == null)
                            lyncKey.CreateSubKey("Autodiscover");
                        lyncKey = lyncKey.OpenSubKey("autodiscover", true);
                    }
                    if (lyncKey == null)
                        continue;
                    outlookInstalled = true;
                    lyncKey.SetValue("ExcludeHttpsRootDomain", 1, RegistryValueKind.DWord);
                    //TODO: Проверить на возможность возникновения ошибок

                    foreach (string outlookDomain in Resources.OutlookDomains)
                    {
                        lyncKey.SetValue(outlookDomain, Resources.OutlookAutodiscoverDestPath, RegistryValueKind.String);
                    }          
                }
                if (!outlookInstalled)
                {
                    success = false;
                    //TODO: Пересмотреть логику.
                    //InstallOffice();
                    //Console.WriteLine("You should install Outlook before.");               
                }
            }

            Console.WriteLine(success
                ? $"{Resources.TextRegistryKeysConfiguration}: {Resources.TextSuccess}"
                : $"{Resources.TextRegistryKeysConfiguration}: {Resources.TextFailed}");
        }
    }
}
