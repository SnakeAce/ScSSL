using System;
using System.Collections.Generic;
using System.Text;

namespace Office_Auto_configuration
{
    internal static class Outlook
    {
        internal static void Configure()
        {
            Console.Clear();
            Console.WriteLine("Outlook Configuration");
            Console.WriteLine(Resources.TextPressAnyKeyToContinue);
            Console.ReadKey();
        }



        //  copy %~dp0\autodiscover.xml %appdata% /y
        //  certutil -user -addstore "Root" %~dp0\pvcadcs2020.cer
        //  certutil -addstore "Root" %~dp0\pvcadcs2020.cer
        //  reg add HKCU\Software\Microsoft\Office\14.0\Outlook\Autodiscover /v ExcludeHttpsRootDomain /t REG_DWORD /D 1 /f
        //  reg add HKCU\Software\Microsoft\Office\15.0\Outlook\Autodiscover /v ExcludeHttpsRootDomain /t REG_DWORD /D 1 /f
        //  reg add HKCU\Software\Microsoft\Office\16.0\Outlook\Autodiscover /v ExcludeHttpsRootDomain /t REG_DWORD /D 1 /f
        //  reg add HKCU\Software\Microsoft\Office\14.0\Outlook\Autodiscover /v susu.ru /t REG_SZ /D %appdata%\autodiscover.xml /f
        //  reg add HKCU\Software\Microsoft\Office\15.0\Outlook\Autodiscover /v susu.ru /t REG_SZ /D %appdata%\autodiscover.xml /f
        //  reg add HKCU\Software\Microsoft\Office\16.0\Outlook\Autodiscover /v susu.ru /t REG_SZ /D %appdata%\autodiscover.xml /f
        //  reg add HKCU\Software\Microsoft\Office\14.0\Outlook\Autodiscover /v susu.ac.ru /t REG_SZ /D %appdata%\autodiscover.xml /f
        //  reg add HKCU\Software\Microsoft\Office\15.0\Outlook\Autodiscover /v susu.ac.ru /t REG_SZ /D %appdata%\autodiscover.xml /f
        //  reg add HKCU\Software\Microsoft\Office\16.0\Outlook\Autodiscover /v susu.ac.ru /t REG_SZ /D %appdata%\autodiscover.xml /f
        //  pause

    }
}
