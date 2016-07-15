using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace Office_Auto_configuration
{
    internal static class Resources
    {
        #region Text
        internal const string TextWelcome = "Вас приветсвует программа для автонастройки продуктов Microsoft для подключения к серверу ssl.local";
        internal const string TextTitleSelection = "Выбирете то, что желаете настроить.";
        internal const string TextSelectionItemFirst = "1. Настройка Lync или Skype for business";
        internal const string TextSelectionItemSecond = "2. Настройка Outlook";
        internal const string TextSelectionItemLast = "0. Выход";
        internal const string TextPressAnyKeyToContinue = "Нажмите любую клавишу для продолжения.";
        #endregion

        #region Lync Fields
        internal static readonly RegistryKey CurrentUser = Registry.CurrentUser;
        internal const string LyncServerAddressInternal = "fe-sfbs.ssl.local";
        internal const string LyncServerAddressExternal = "fe-sfbs.ssl.local";
        internal const string LyncServerDnsAddsServerIp = "33.0.0.1";
        internal const string LyncServerIp = "33.0.0.11";
        internal const string LyncServerDomainCertificateFileName = "ssl-AD01-CA-cert.cer";
        internal const string LyncServerDomainCertificateName = "ssl-AD01-CA";
        internal static readonly string[] DomainNames = new string[2] { "@ssl.local", "@alkapov.ru" };
        internal const string OfficeDownloadUrl = "";
        #endregion

    }
}
