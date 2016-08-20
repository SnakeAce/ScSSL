using System;
using System.Text;

namespace Office_Auto_configuration
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.GetEncoding(1251);
            var menu = new Menu();
            //menu.Show();
            menu.ShowStandaloneOutlook();
        }
    }
}
