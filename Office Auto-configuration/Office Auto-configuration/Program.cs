using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Office_Auto_configuration
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.GetEncoding(1251);
            Menu menu = new Menu();
            menu.Show();
        }
    }
}
