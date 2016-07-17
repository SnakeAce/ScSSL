using System;

namespace Office_Auto_configuration
{
    internal class Menu
    {
        internal void Show()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(Resources.TextWelcome);
                Console.WriteLine(Resources.TextTitleSelection);
                Console.WriteLine(Resources.TextSelectionItemFirst);
                Console.WriteLine(Resources.TextSelectionItemSecond);
                Console.WriteLine(Resources.TextSelectionItemLast);

                ConsoleKeyInfo key = Console.ReadKey();
                switch (key.KeyChar)
                {
                    case '0':
                        return;
                    case '1':
                        Lync.Configure();
                        break;
                    case '2':
                        Outlook.Configure();
                        break;
                }
            }
        }
    }
}
