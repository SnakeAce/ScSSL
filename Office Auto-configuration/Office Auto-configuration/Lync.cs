using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Win32;


namespace Office_Auto_configuration
{
    internal static class Lync
    {

        internal static void Configure()
        {
            Console.Clear();
            Console.WriteLine("Lync Configuration");
            Console.WriteLine(Resources.TextPressAnyKeyToContinue);
            Console.ReadKey();
        }
    

        private static void LyncConfigureUsers()
        {
            
            List<string> emails = new List<string>();

            #region lync 2016

            RegistryKey currentKey = Resources.CurrentUser;
            currentKey = currentKey.OpenSubKey("software")?.OpenSubKey("microsoft")?.OpenSubKey("office");

            List<string> officeVersions =
                currentKey.GetSubKeyNames().Select(x => x).Where(x => x.Contains(".0")).ToList<string>();

            foreach (string version in officeVersions)
            {
                RegistryKey outlookpreKey = currentKey.OpenSubKey(version)?
                    .OpenSubKey("outlook")?.OpenSubKey("profiles");
                string[] outlookKeys = outlookpreKey?.GetSubKeyNames();
                if (outlookKeys == null) continue;
                foreach (string ok in outlookKeys)
                {
                    RegistryKey outlookKey = outlookpreKey?.OpenSubKey(ok);
                    if (outlookKey == null)
                        continue;
                    string[] metadata = outlookKey.GetSubKeyNames();
                    emails.AddRange(
                        metadata.Select(md => outlookKey.OpenSubKey(md))
                            .Where(co => co?.GetValue("001f6641") != null)
                            .Select(
                                co =>
                                    System.Text.Encoding.UTF8.GetString(co.GetValue("001f6641") as byte[])
                                        .Replace("\0", "")));

                    emails.AddRange(
                        metadata.Select(md => outlookKey.OpenSubKey(md))
                            .Where(co => co?.GetValue("001e660b") != null)
                            .Select(
                                co =>
                                    System.Text.Encoding.UTF8.GetString(co.GetValue("001e660b") as byte[])
                                        .Replace("\0", "")));

                }
            }

            #endregion

            #region old lync

            currentKey = Resources.CurrentUser;
            //Windows NT\CurrentVersion\Windows Messaging Subsystem\Profiles
            currentKey =
                currentKey.OpenSubKey("software")?
                    .OpenSubKey("microsoft")?
                    .OpenSubKey("Windows NT")?
                    .OpenSubKey("CurrentVersion")?.OpenSubKey("Windows Messaging Subsystem")?.OpenSubKey("Profiles");

            string[] _outlookKeys = currentKey?.GetSubKeyNames();
            if (_outlookKeys != null)
                foreach (string ok in _outlookKeys)
                {
                    RegistryKey outlookKey = currentKey?.OpenSubKey(ok);
                    if (outlookKey == null)
                        continue;
                    string[] metadata = outlookKey.GetSubKeyNames();

                    emails.AddRange(
                        metadata.Select(md => outlookKey.OpenSubKey(md))
                            .Where(co => co?.GetValue("001f6641") != null)
                            .Select(
                                co =>
                                    System.Text.Encoding.UTF8.GetString(co.GetValue("001f6641") as byte[])
                                        .Replace("\0", "")));

                    emails.AddRange(
                        metadata.Select(md => outlookKey.OpenSubKey(md))
                            .Where(co => co?.GetValue("001e660b") != null)
                            .Select(
                                co =>
                                    System.Text.Encoding.UTF8.GetString(co.GetValue("001e660b") as byte[])
                                        .Replace("\0", "")));
                }

            #endregion

            emails = emails.Distinct().ToList();
            Regex emailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);
            for (int i = 0; i < emails.Count; ++i)
                emails[i] = emailRegex.Match(emails[i])?.Value;

            List<string> nemails = new List<string>();
            foreach (string email in emails)
            {
                foreach (var domainname in Resources.DomainNames)
                {
                    if (emails.Contains(domainname))
                    {
                        nemails.Add(email);
                        break;
                    }
                }
            }
            WriteHover();
            if (nemails.Count > 0)
            {
                Console.WriteLine("Emails: ");
                foreach (string email in nemails)
                    Console.WriteLine(email);
            }
            else
            {
                Console.WriteLine("There are not emails");
            }

            WriteHover();
        }

        private static void LyncConfigureCertificate()
        {
            X509Certificate2 cert = new X509Certificate2(Resources.LyncServerDomainCertificateFileName);
            //Для пользователя
            X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            //Для данного компьютера 
            //?? X509Store store = new X509Store(StoreName.CertificateAuthority, StoreLocation.LocalMachine);
            //?? X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);

            store.Open(OpenFlags.ReadWrite);
            try
            {
                store.Add(cert);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex.Message}");
            }


            WriteHover();
            X509Certificate2Collection certs = store.Certificates.Find(X509FindType.FindBySubjectName, Resources.LyncServerDomainCertificateName, true);
            if (certs.Count > 0)
            {
                // Certificate is found.
                Console.WriteLine($"Certificates: ");
                for (int i = 0; i < certs.Count; ++i)
                {
                    Console.WriteLine($"{i + 1}. {certs[i].Issuer} {certs[i].Thumbprint}");
                }
            }
            else
            {
                Console.WriteLine("No certificates installed");
            }
            WriteHover();
            store.Close();
        }
        private static void LyncConfigureHosts()
        {
            string path = $@"{Environment.GetEnvironmentVariable("SystemRoot")}\System32\drivers\etc\hosts";
            string[] lines = File.ReadAllLines(path);
            List<string> newlines = new List<string>();
            foreach (string line in lines)
            {
                string cline = line.ToLower();
                //TODO: добавить все возможные варианты
                if (cline.Contains(Resources.LyncServerAddressExternal) || cline.Contains(Resources.LyncServerDnsAddsServerIp))
                    continue;
                newlines.Add(line);
            }

            newlines.Add($"# Here DNS records for {Resources.LyncServerAddressExternal}");
            newlines.Add($"{Resources.LyncServerIp} {Resources.LyncServerAddressExternal}");
            newlines.Add($"{Resources.LyncServerIp} lyncdiscover.{Resources.LyncServerAddressExternal}");
            newlines.Add($"{Resources.LyncServerIp} sip.{Resources.LyncServerAddressExternal}");
            newlines.Add($"{Resources.LyncServerIp} dialin.{Resources.LyncServerAddressExternal}");
            newlines.Add($"{Resources.LyncServerIp} meet.{Resources.LyncServerAddressExternal}");
            newlines.Add($"{Resources.LyncServerIp} Lyncwebext.{Resources.LyncServerAddressExternal}");
            newlines.Add($"{Resources.LyncServerIp} AV.{Resources.LyncServerAddressExternal}");
            //SRV ??
            newlines.Add($"{Resources.LyncServerIp} _sip._tls.{Resources.LyncServerAddressExternal}");
            newlines.Add($"{Resources.LyncServerIp} _sipfederationtls.{Resources.LyncServerAddressExternal}");
            newlines.Add($"{Resources.LyncServerIp} _xmpp-server._tcp.{Resources.LyncServerAddressExternal}");
            File.WriteAllLines(path, newlines);

            WriteHover();
            Console.WriteLine($"Hosts:\n{File.ReadAllText(path)}");
            WriteHover();

        }
        private static void WriteHover()
        {
            Console.WriteLine("-----------------------------------------------------------------------");
        }
        private static void LyncConfigureRegistry()
        {
            RegistryKey currentKey = Resources.CurrentUser;
            currentKey = currentKey.OpenSubKey("software")?.OpenSubKey("microsoft")?.OpenSubKey("office");
            if (currentKey == null)
            {
                InstallOffice();
                Console.WriteLine("You should install office before.");
                return;
            }
            List<string> officeVersions = currentKey.GetSubKeyNames().Select(x => x).Where(x => x.Contains(".0")).ToList<string>();
            bool lyncInstalled = false;
            foreach (string version in officeVersions)
            {
                RegistryKey lyncKey = currentKey.OpenSubKey(version)?.OpenSubKey("lync", true);
                if (lyncKey == null)
                    continue;
                lyncInstalled = true;
                WriteHover();
                Console.WriteLine($"Lync Version: {version}");

                //TODO: try-catch?
                lyncKey.SetValue("ConfigurationMode", 1, RegistryValueKind.DWord);
                Console.WriteLine($"ConfigurationMode: {lyncKey.GetValue("ConfigurationMode")}");

                lyncKey.SetValue("ServerAddressInternal", Resources.LyncServerAddressInternal, RegistryValueKind.String);
                Console.WriteLine($"ServerAddressInternal: {lyncKey.GetValue("ServerAddressInternal")}");

                lyncKey.SetValue("ServerAddressExternal", Resources.LyncServerAddressExternal, RegistryValueKind.String);
                Console.WriteLine($"ServerAddressExternal: {lyncKey.GetValue("ServerAddressExternal")}");
                WriteHover();
            }
            if (!lyncInstalled)
            {
                InstallOffice();
                Console.WriteLine("You should install Lync before.");
                return;
            }
        }
        private static void InstallOffice()
        {
            return;
            //TODO:
            if (!File.Exists("office.zip"))
            {
                WebClient Client = new WebClient();
                Client.DownloadFile("OfficeDownloadUrl", @"office.zip");
            }

            DirectoryInfo directorySelected = new DirectoryInfo(Directory.GetCurrentDirectory());

            foreach (FileInfo fileToDecompress in directorySelected.GetFiles("*.zip"))
            {
                Decompress(fileToDecompress);
            }


        }
        public static void Decompress(FileInfo fileToDecompress)
        {
            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                string currentFileName = fileToDecompress.FullName;
                string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                using (FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        Console.WriteLine("Decompressed: {0}", fileToDecompress.Name);
                    }
                }
            }
        }



    }
}
