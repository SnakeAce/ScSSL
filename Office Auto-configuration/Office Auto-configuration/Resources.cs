using System;
using System.Collections.Generic;
using System.Security.Principal;
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
        internal const string TextServerName = "susu.ru";
        internal static readonly string TextWelcome = $"Вас приветсвует программа для автонастройки продуктов Microsoft для подключения к серверу {Resources.TextServerName}";
        internal const string TextTitleSelection = "Выбирете то, что желаете настроить.";
        internal const string TextSelectionItemFirst = "1. Настройка Lync или Skype for business";
        internal const string TextSelectionItemSecond = "2. Настройка Outlook";
        internal const string TextSelectionItemLast = "0. Выход";
        internal const string TextPressAnyKeyToContinue = "Нажмите любую клавишу для продолжения.";
        internal const string TextCertificateInstallation = "Установка сертификата";
        internal const string TextSuccess = "Успешно";
        internal const string TextFailed = "Неудачно";
        internal const string TextHostsFileConfiguration = "Настройка DNS";
        internal const string TextLyncConfigurationTitle = "Настройка Lync или Skype for business";
        internal const string TextRegistryKeysConfiguration = "Конфигурация регистра";
        internal const string TextUserAccountsNotFound = "Пользователи Outlook не обнаружены";
        internal const string TextUserAccountsFound = "Найденные пользователи";
        internal const string TextOutlookAutodiscoverExistence = "Корректность Autodiscover";
        #endregion

        #region Lync Fields
        internal static readonly RegistryKey CurrentUser = Registry.CurrentUser;
        internal const string LyncServerAddressInternal = "pvc.susu.ru"; //??
        internal const string LyncServerAddressExternal = "pvc.susu.ru";
        internal const string LyncServerDnsAddsServerIp = "pvc.susu.ru";
        internal const string LyncServerIp = "ERROR";
        internal const string LyncServerDomainCertificateFileName = "pvcadcs2020.cer";
        internal const string LyncServerDomainCertificateName = "pvcadcs2020";
        internal static readonly string[] DomainNames = { "@susu.ru", "@susu.ac.ru", "@pvc.susu.ru" };
        internal const string OfficeDownloadUrl = "ERROR";

        internal static readonly string LyncDnsRecordsTitle = $"# Here DNS records for {Resources.LyncServerAddressExternal}";

        internal static readonly string[] LyncOutlookUserKeys = { "001f6641", "001e660b" };

        #region DNS Records
        internal static readonly string[] LyncInternalDnsARecordsPrefix = { "lyncdiscoverinternal", "lync", "sip", "dialin", "meet", "Admin" };
        internal static readonly string[] LyncInternalDnsSrvRecordsPrefix = { "_ntp._udp", "_sip._tls", "_sipfederationtls", "_sipinternaltls", "_xmpp-server._tcp" };
        internal static readonly string[] LyncExternalDnsARecordsPrefix = { "lyncdiscover", "Lyncwebext", "sip", "dialin", "meet", "AV", "sipexternal" };
        internal static readonly string[] LyncExternalDnsSrvRecordsPrefix = { "_sip._tls", "_sipfederationtls", "_xmpp-server._tcp" };
        #endregion

        #endregion

        #region Outlook Fields

        //%~dp0\autodiscover.xml %appdata% /y
        internal static readonly string OutlookAutodiscoverDestPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\{Resources.OutlookAutodiscoverFilename}";

        internal const string OutlookAutodiscoverFilename = "autodiscover.xml";
        internal const string OutlookServerDomainCertificateFileName = "pvcadcs2020.cer";
        internal const string OutlookAutodiscoverHostName = "mail.pvc.susu.ru";
        internal static readonly string OutlookAutodiscoverContent =
             "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" + Environment.NewLine
            + "<Autodiscover xmlns = \"http://schemas.microsoft.com/exchange/autodiscover/responseschema/2006\">" + Environment.NewLine
            + "<Response xmlns=\"http://schemas.microsoft.com/exchange/autodiscover/outlook/responseschema/2006a\">" + Environment.NewLine
            + "<Account>" + Environment.NewLine
            + "<AccountType>email</AccountType>" + Environment.NewLine
            + "<Action>redirectUrl</Action>" + Environment.NewLine
            + $"<RedirectUrl>https://{OutlookAutodiscoverHostName}/autodiscover/autodiscover.xml</RedirectUrl>" + Environment.NewLine
            + "</Account>" + Environment.NewLine
            + "</Response>" + Environment.NewLine
            + "</Autodiscover>" + Environment.NewLine;

        internal static readonly string[] OutlookDomains = {"susu.ru", "susu.ac.ru", "pvc.susu.ru"};

        #endregion


        internal static bool IsAdministrator()
        {
            return (new WindowsPrincipal(WindowsIdentity.GetCurrent()))
                    .IsInRole(WindowsBuiltInRole.Administrator);
        }

    }
}
