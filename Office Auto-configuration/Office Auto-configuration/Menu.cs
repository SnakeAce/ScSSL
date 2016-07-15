using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Office_Auto_configuration
{
    internal class Menu
    {
        internal void Show()
        {
            Console.Clear();
            Console.WriteLine(Resources.TextWelcome);
            Console.WriteLine(Resources.TextTitleSelection);
            Console.WriteLine(Resources.TextSelectionItemFirst);
            Console.WriteLine(Resources.TextSelectionItemSecond);
            Console.WriteLine(Resources.TextSelectionItemLast);

            ConsoleKeyInfo key = Console.ReadKey();
            if (key.KeyChar == '0')
                return;
            if (key.KeyChar == '1')
                Lync.Configure();
            else if (key.KeyChar == '2')
                Outlook.Configure();
            Show();
        }
    }
}
