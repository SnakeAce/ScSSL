using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Microsoft.Win32;


namespace Office_Auto_configuration
{
    internal static class Lync
    {
        internal static void Configure()
        {
            Console.Clear();
            Console.WriteLine(Resources.TextLyncConfigurationTitle);

            if (Resources.IsAdministrator())
            {
                InstallCertificate();
                ConfigureHostsFile();
            }
            else
            {
                InstallCertificate(1);
            }
            ConfigureRegistryKeys();
       //     ConfigureUsers();

            Console.WriteLine(Resources.TextPressAnyKeyToContinue);
            Console.ReadKey();
        }


        #region private mathods
        //private static void InstallCertificate(int? n = null)
        //{

        //    X509Certificate2 cert;
        //    try
        //    {
        //        cert = new X509Certificate2(Resources.AddPath(Resources.LyncServerDomainCertificateFileName));
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"{Resources.TextCertificateInstallation}: {Resources.TextFailed} - {ex.Message}");
        //        return;
        //    }

        //    //Для пользователя
        //    //var store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
        //    //Для данного компьютера 
        //    //?? X509Store store = new X509Store(StoreName.CertificateAuthority, StoreLocation.LocalMachine);

        //    var store = new X509Store(StoreName.Root, n == null ? StoreLocation.LocalMachine : StoreLocation.CurrentUser);

        //    X509Certificate2Collection certs = store.Certificates.Find(X509FindType.FindBySubjectName, Resources.LyncServerDomainCertificateName, true);
        //    if (certs.Count > 0)
        //    {
        //        for (int i = 0; i < certs.Count; ++i)
        //        {
        //            if (certs[i].Thumbprint != cert.Thumbprint) continue;
        //            Console.WriteLine($"{Resources.TextCertificateInstallation}: {Resources.TextSuccess}");
        //            store.Close();
        //            return;
        //        }
        //    }

        //    store.Open(OpenFlags.ReadWrite);
        //    try
        //    {
        //        store.Add(cert);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"{Resources.TextCertificateInstallation}: {Resources.TextFailed} - {ex.Message}");
        //        store.Close();
        //        return;
        //    }




        //    certs = store.Certificates.Find(X509FindType.FindBySubjectName, Resources.LyncServerDomainCertificateName, true);
        //    if (certs.Count > 0)
        //    {
        //        for (int i = 0; i < certs.Count; ++i)
        //        {
        //            if (certs[i].Thumbprint != cert.Thumbprint) continue;
        //            Console.WriteLine($"{Resources.TextCertificateInstallation}: {Resources.TextSuccess}");
        //            store.Close();
        //            return;
        //        }
        //    }

        //    Console.WriteLine($"{Resources.TextCertificateInstallation}: {Resources.TextFailed}");
        //    store.Close();

        //}


        private static void InstallCertificate(int? t = null)
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
            var store = new X509Store(StoreName.Root, t == null ? StoreLocation.LocalMachine : StoreLocation.CurrentUser);

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

        private static void ConfigureHostsFile()
        {
            string path = $@"{Environment.GetEnvironmentVariable("SystemRoot")}\System32\drivers\etc\hosts";
            if (!File.Exists(path))
                File.Create(path);
            string[] records = File.ReadAllLines(path);
            var newRecords = new List<string>();
            foreach (string record in records)
            {
                string cline = record.ToLower();
                //TODO: Все возможные варианты
                if (!cline.Contains(Resources.LyncServerAddressExternal) && !cline.Contains(Resources.LyncServerDnsAddsServerIp))
                    newRecords.Add(record);
            }
            newRecords.Add(Resources.LyncDnsRecordsTitle);
            //TODO: При необходимости добавить другие записи
            foreach (string prefix in Resources.LyncAllDnsRecords)
            {
                newRecords.Add($"{Resources.LyncServerIp} {prefix}.{Resources.LyncServerDnsAddsServerIp}");
            }
            try
            {
                File.WriteAllLines(path, newRecords.ToArray());
            }
            catch (Exception)
            {
                // ignored
            }

            string text = File.ReadAllText(path);
#if DEBUG
            Console.WriteLine($"Hosts:\n{File.ReadAllText(path)}");
#endif
            Console.WriteLine(text.Contains(Resources.LyncDnsRecordsTitle)
                ? $"{Resources.TextHostsFileConfiguration}: {Resources.TextSuccess}"
                : $"{Resources.TextHostsFileConfiguration}: {Resources.TextFailed}");
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
                bool lyncInstalled = false;
                foreach (string version in officeVersions)
                {
                    RegistryKey lyncKey = currentKey.OpenSubKey(version)?.OpenSubKey("lync", true);
                    if (lyncKey == null)
                        continue;
                    lyncInstalled = true;

                    //TODO: Проверить на возможность возникновения ошибок
#if DEBUG
                    Console.WriteLine($"Lync Version: {version}");
                    lyncKey.SetValue("ConfigurationMode", 1, RegistryValueKind.DWord);
                    Console.WriteLine($"ConfigurationMode: {lyncKey.GetValue("ConfigurationMode")}");

                    lyncKey.SetValue("ServerAddressInternal", Resources.LyncServerAddressInternal, RegistryValueKind.String);
                    Console.WriteLine($"ServerAddressInternal: {lyncKey.GetValue("ServerAddressInternal")}");

                    lyncKey.SetValue("ServerAddressExternal", Resources.LyncServerAddressExternal, RegistryValueKind.String);
                    Console.WriteLine($"ServerAddressExternal: {lyncKey.GetValue("ServerAddressExternal")}");
#else
                    lyncKey.SetValue("ConfigurationMode", 1, RegistryValueKind.DWord);
                    lyncKey.SetValue("ServerAddressInternal", Resources.LyncServerAddressInternal, RegistryValueKind.String);
                    lyncKey.SetValue("ServerAddressExternal", Resources.LyncServerAddressExternal, RegistryValueKind.String);
#endif

                }
                if (!lyncInstalled)
                {
                    success = false;
                    //TODO: Пересмотреть логику.
                    //InstallOffice();
                    //Console.WriteLine("You should install Lync before.");               
                }
            }

            Console.WriteLine(success
                ? $"{Resources.TextRegistryKeysConfiguration}: {Resources.TextSuccess}"
                : $"{Resources.TextRegistryKeysConfiguration}: {Resources.TextFailed}");
        }
        private static void ConfigureUsers()
        {
            var emails = new List<string>();

            #region v2016

            RegistryKey currentKey = Resources.CurrentUser;
            currentKey = currentKey.OpenSubKey("software")?.OpenSubKey("microsoft")?.OpenSubKey("office");

            var officeVersions = new List<string>();

            if (currentKey != null)
            {
                foreach (string subKeyName in currentKey.GetSubKeyNames())
                {
                    if (subKeyName.Contains(".0"))
                        officeVersions.Add(subKeyName);
                }

                foreach (string version in officeVersions)
                {
                    RegistryKey outlookpreKey = currentKey.OpenSubKey(version)?
                        .OpenSubKey("outlook")?.OpenSubKey("profiles");
                    string[] outlookKeys = outlookpreKey?.GetSubKeyNames();
                    if (outlookKeys == null) continue;
                    foreach (string ok in outlookKeys)
                    {
                        RegistryKey outlookKey = outlookpreKey.OpenSubKey(ok);
                        if (outlookKey == null)
                            continue;
                        string[] metadata = outlookKey.GetSubKeyNames();

                        foreach (string md in metadata)
                        {
                            RegistryKey co = outlookKey.OpenSubKey(md);
                            foreach (string lyncOutlookUserKey in Resources.LyncOutlookUserKeys)
                            {
                                if (co?.GetValue(lyncOutlookUserKey) != null)
                                    // ReSharper disable once AssignNullToNotNullAttribute
                                    emails.Add(System.Text.Encoding.UTF8.GetString(co.GetValue(lyncOutlookUserKey) as byte[]).Replace("\0", ""));
                            }
                        }
                    }
                }
            }

            #endregion


            #region vOld

            currentKey = Resources.CurrentUser;
            currentKey = currentKey.OpenSubKey("software")?.OpenSubKey("microsoft")?.OpenSubKey("Windows NT")?.OpenSubKey("CurrentVersion")?.OpenSubKey("Windows Messaging Subsystem")?.OpenSubKey("Profiles");
            string[] oldOutlookSubKeyNames = currentKey?.GetSubKeyNames();
            if (oldOutlookSubKeyNames != null)
                foreach (string ok in oldOutlookSubKeyNames)
                {
                    RegistryKey outlookKey = currentKey.OpenSubKey(ok);
                    if (outlookKey == null)
                        continue;
                    string[] metadata = outlookKey.GetSubKeyNames();

                    foreach (string md in metadata)
                    {
                        RegistryKey co = outlookKey.OpenSubKey(md);
                        foreach (string lyncOutlookUserKey in Resources.LyncOutlookUserKeys)
                        {
                            if (co?.GetValue(lyncOutlookUserKey) != null)
                                // ReSharper disable once AssignNullToNotNullAttribute
                                emails.Add(System.Text.Encoding.UTF8.GetString(co.GetValue(lyncOutlookUserKey) as byte[]).Replace("\0", ""));
                        }
                    }
                }

            #endregion


            var cEmails = new List<string>();
            for (int i = 0; i < emails.Count; ++i)
                if (!cEmails.Contains(emails[i]))
                    cEmails.Add(emails[i]);
            emails = cEmails;
            var emailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);
            for (int i = 0; i < emails.Count; ++i)
                emails[i] = emailRegex.Match(emails[i]).Value;
            var nemails = new List<string>();
            foreach (string email in emails)
            {
                foreach (string domainname in Resources.DomainNames)
                {
                    if (!emails.Contains(domainname)) continue;
                    nemails.Add(email);
                    break;
                }
            }

            if (nemails.Count > 0)
            {
                Console.Write($"{Resources.TextUserAccountsFound}: ");
                for (int i = 0; i < emails.Count - 1; ++i)
                    Console.Write($"{emails[i]}, ");
                Console.WriteLine(emails[emails.Count - 1]);
            }
            else
            {
                Console.WriteLine(Resources.TextUserAccountsNotFound);
            }

        }
        #endregion
    }
}
