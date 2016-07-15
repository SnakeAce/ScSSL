using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
