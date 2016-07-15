using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace Office_Auto_configuration
{
    
    internal static class Resources
    {
        internal static string AddPath(string name)
        {
            return FilesPath + name;
        }

        internal const string FilesPath = "Files/";

        #region Text
        internal const string TextWelcome = "Вас приветсвует программа для автонастройки продуктов Microsoft для подключения к серверу ssl.local";
        internal const string TextTitleSelection = "Выбирете то, что желаете настроить.";
        internal const string TextSelectionItemFirst = "1. Настройка Lync или Skype for business";
        internal const string TextSelectionItemSecond = "2. Настройка Outlook";
        internal const string TextSelectionItemLast = "0. Выход";
        internal const string TextPressAnyKeyToContinue = "Нажмите любую клавишу для продолжения.";
        internal const string TextCertificateInstallation = "Установка сертификата";
        internal const string TextSuccess = "Success";
        internal const string TextFailed = "Failed";
        internal const string TextHostsFileConfiguration = "Настройка DNS";
        internal const string TextLyncConfigurationTitle = "Настройка Lync или Skype for business";
        internal const string TextRegistryKeysConfiguration = "Конфигурация регистра";
        #endregion

        #region Lync Fields
        internal static readonly RegistryKey CurrentUser = Registry.CurrentUser;
        internal const string LyncServerAddressInternal = "fe-sfbs.ssl.local";
        internal const string LyncServerAddressExternal = "fe-sfbs.ssl.local";
        internal const string LyncServerDnsAddsServerIp = "33.0.0.1";
        internal const string LyncServerIp = "33.0.0.11";
        internal const string LyncServerDomainCertificateFileName = "ssl-AD01-CA-cert.cer";
        internal const string LyncServerDomainCertificateName = "ssl-AD01-CA";
        internal static readonly string[] DomainNames = {"@ssl.local"};
        internal const string OfficeDownloadUrl = "";

        internal static readonly string LyncDnsRecordsTitle = $"# Here DNS records for {Resources.LyncServerAddressExternal}";

        #region DNS Records
        internal static readonly string[] LyncInternalDnsARecordsPrefix = { "lyncdiscoverinternal", "lync", "sip", "dialin", "meet", "Admin"};
        internal static readonly string[] LyncInternalDnsSrvRecordsPrefix = { "_ntp._udp", "_sip._tls", "_sipfederationtls", "_sipinternaltls", "_xmpp-server._tcp"};
        internal static readonly string[] LyncExternalDnsARecordsPrefix = { "lyncdiscover", "Lyncwebext", "sip", "dialin", "meet", "AV", "sipexternal" };
        internal static readonly string[] LyncExternalDnsSrvRecordsPrefix = { "_sip._tls", "_sipfederationtls", "_xmpp-server._tcp" };
        #endregion

        #endregion

    }
}
