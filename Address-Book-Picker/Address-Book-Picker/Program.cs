using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Address_Book_Picker
{


    class Program
    {
        private class Helper
        {

            public enum TransliterationType
            {
                Gost,
                ISO
            }
            public static class Transliteration
            {
                private static Dictionary<string, string> gost = new Dictionary<string, string>(); //ГОСТ 16876-71
                private static Dictionary<string, string> iso = new Dictionary<string, string>(); //ISO 9-95
                public static string Front(string text)
                {
                    return Front(text, TransliterationType.ISO);
                }
                public static string Front(string text, TransliterationType type)
                {
                    string output = text;

                    output = Regex.Replace(output, @"\s|\.|\(", " ");
                    output = Regex.Replace(output, @"\s+", " ");
                    output = Regex.Replace(output, @"[^\s\w\d-]", "");
                    output = output.Trim();

                    Dictionary<string, string> tdict = GetDictionaryByType(type);

                    foreach (KeyValuePair<string, string> key in tdict)
                    {
                        output = output.Replace(key.Key, key.Value);
                    }
                    return output;
                }
                public static string Back(string text)
                {
                    return Back(text, TransliterationType.ISO);
                }
                public static string Back(string text, TransliterationType type)
                {
                    string output = text;
                    Dictionary<string, string> tdict = GetDictionaryByType(type);

                    foreach (KeyValuePair<string, string> key in tdict)
                    {
                        output = output.Replace(key.Value, key.Key);
                    }
                    return output;
                }

                private static Dictionary<string, string> GetDictionaryByType(TransliterationType type)
                {
                    Dictionary<string, string> tdict = iso;
                    if (type == TransliterationType.Gost) tdict = gost;
                    return tdict;
                }

                static Transliteration()
                {
                    gost.Add("Є", "EH");
                    gost.Add("І", "I");
                    gost.Add("і", "i");
                    gost.Add("№", "#");
                    gost.Add("є", "eh");
                    gost.Add("А", "A");
                    gost.Add("Б", "B");
                    gost.Add("В", "V");
                    gost.Add("Г", "G");
                    gost.Add("Д", "D");
                    gost.Add("Е", "E");
                    gost.Add("Ё", "JO");
                    gost.Add("Ж", "ZH");
                    gost.Add("З", "Z");
                    gost.Add("И", "I");
                    gost.Add("Й", "JJ");
                    gost.Add("К", "K");
                    gost.Add("Л", "L");
                    gost.Add("М", "M");
                    gost.Add("Н", "N");
                    gost.Add("О", "O");
                    gost.Add("П", "P");
                    gost.Add("Р", "R");
                    gost.Add("С", "S");
                    gost.Add("Т", "T");
                    gost.Add("У", "U");
                    gost.Add("Ф", "F");
                    gost.Add("Х", "KH");
                    gost.Add("Ц", "C");
                    gost.Add("Ч", "CH");
                    gost.Add("Ш", "SH");
                    gost.Add("Щ", "SHH");
                    gost.Add("Ъ", "'");
                    gost.Add("Ы", "Y");
                    gost.Add("Ь", "");
                    gost.Add("Э", "EH");
                    gost.Add("Ю", "YU");
                    gost.Add("Я", "YA");
                    gost.Add("а", "a");
                    gost.Add("б", "b");
                    gost.Add("в", "v");
                    gost.Add("г", "g");
                    gost.Add("д", "d");
                    gost.Add("е", "e");
                    gost.Add("ё", "jo");
                    gost.Add("ж", "zh");
                    gost.Add("з", "z");
                    gost.Add("и", "i");
                    gost.Add("й", "jj");
                    gost.Add("к", "k");
                    gost.Add("л", "l");
                    gost.Add("м", "m");
                    gost.Add("н", "n");
                    gost.Add("о", "o");
                    gost.Add("п", "p");
                    gost.Add("р", "r");
                    gost.Add("с", "s");
                    gost.Add("т", "t");
                    gost.Add("у", "u");

                    gost.Add("ф", "f");
                    gost.Add("х", "kh");
                    gost.Add("ц", "c");
                    gost.Add("ч", "ch");
                    gost.Add("ш", "sh");
                    gost.Add("щ", "shh");
                    gost.Add("ъ", "");
                    gost.Add("ы", "y");
                    gost.Add("ь", "");
                    gost.Add("э", "eh");
                    gost.Add("ю", "yu");
                    gost.Add("я", "ya");
                    gost.Add("«", "");
                    gost.Add("»", "");
                    gost.Add("—", "-");
                    gost.Add(" ", "-");

                    iso.Add("Є", "YE");
                    iso.Add("І", "I");
                    iso.Add("Ѓ", "G");
                    iso.Add("і", "i");
                    iso.Add("№", "#");
                    iso.Add("є", "ye");
                    iso.Add("ѓ", "g");
                    iso.Add("А", "A");
                    iso.Add("Б", "B");
                    iso.Add("В", "V");
                    iso.Add("Г", "G");
                    iso.Add("Д", "D");
                    iso.Add("Е", "E");
                    iso.Add("Ё", "YO");
                    iso.Add("Ж", "ZH");
                    iso.Add("З", "Z");
                    iso.Add("И", "I");
                    iso.Add("Й", "J");
                    iso.Add("К", "K");
                    iso.Add("Л", "L");
                    iso.Add("М", "M");
                    iso.Add("Н", "N");
                    iso.Add("О", "O");
                    iso.Add("П", "P");
                    iso.Add("Р", "R");
                    iso.Add("С", "S");
                    iso.Add("Т", "T");
                    iso.Add("У", "U");
                    iso.Add("Ф", "F");
                    iso.Add("Х", "X");
                    iso.Add("Ц", "C");
                    iso.Add("Ч", "CH");
                    iso.Add("Ш", "SH");
                    iso.Add("Щ", "SHH");
                    iso.Add("Ъ", "'");
                    iso.Add("Ы", "Y");
                    iso.Add("Ь", "");
                    iso.Add("Э", "E");
                    iso.Add("Ю", "YU");
                    iso.Add("Я", "YA");
                    iso.Add("а", "a");
                    iso.Add("б", "b");
                    iso.Add("в", "v");
                    iso.Add("г", "g");
                    iso.Add("д", "d");
                    iso.Add("е", "e");
                    iso.Add("ё", "yo");
                    iso.Add("ж", "zh");
                    iso.Add("з", "z");
                    iso.Add("и", "i");
                    iso.Add("й", "j");
                    iso.Add("к", "k");
                    iso.Add("л", "l");
                    iso.Add("м", "m");
                    iso.Add("н", "n");
                    iso.Add("о", "o");
                    iso.Add("п", "p");
                    iso.Add("р", "r");
                    iso.Add("с", "s");
                    iso.Add("т", "t");
                    iso.Add("у", "u");
                    iso.Add("ф", "f");
                    iso.Add("х", "x");
                    iso.Add("ц", "c");
                    iso.Add("ч", "ch");
                    iso.Add("ш", "sh");
                    iso.Add("щ", "shh");
                    iso.Add("ъ", "");
                    iso.Add("ы", "y");
                    iso.Add("ь", "");
                    iso.Add("э", "e");
                    iso.Add("ю", "yu");
                    iso.Add("я", "ya");
                    iso.Add("«", "");
                    iso.Add("»", "");
                    iso.Add("—", "-");
                    iso.Add(" ", "-");
                }
            }

        }

        private class Person
        {

            public string LastName { get; private set; }
            public string FirstName { get; private set; }
            public string FatherName { get; private set; }
            public string Email { get; private set; } = "";
            public string Name { get; private set; }
            public Person()
            {

            }

            public Person(string person)
            {
                var source = person.Replace(",\"", ";").Replace("\"", "").Split(';');
                var name = source[4].Split(' ');
                this.LastName = name[0];
                this.FirstName = name.Length > 1 ? name[1] : "";
                this.FatherName = name.Length > 2 ? name[2] : "";
                List<string> emails = new List<string>();
                for (int i = 0; i < source.Length; i++)
                    if (source[i].Contains("@susu.ru"))
                        emails.Add(source[i]);
                if (emails.Count == 0)
                {
                    //TODO: relogic it
                    Console.WriteLine($"Пользователь \"{source[4]}\" не имеет email @susu.ru");                  
                    this.Name = Helper.Transliteration.Front((LastName + FirstName[0] + FatherName[0]).ToLower());
                    this.Email = this.Name + "@susu.ru";
                    return;              
                }
                if (emails.Count == 1)
                {
                    this.Email = emails[0];
                    this.Name = this.Email.Split('@')[0];
                    return;
                }
                else
                {
                    string lname = Helper.Transliteration.Front((LastName + FirstName[0] + FatherName[0]).ToLower());
                    for (int i = 0; i < emails.Count; ++i)
                        if (emails[i].Split('@')[0].Equals(lname))
                        {
                            this.Email = emails[i];
                            this.Name = this.Email.Split('@')[0];
                            return;
                        }
                    this.Email = emails[0];
                    this.Name = this.Email.Split('@')[0];
                }
            }
        }

        static void Parse(string path)
        {
            List<string> source = new List<string>();
            try
            {
                source = File.ReadAllLines(path, Encoding.Default).ToList();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                Console.ReadLine();
                return;
            }
            List<Person> persons = new List<Person>();
            for (int i = 1; i < source.Count; ++i)
                persons.Add(new Person(source[i]));
            List<List<string>> outputList = new List<List<string>>();
            outputList.Add(new List<string>() { "Name", "Firstname", "LastName", "ExternalEmailAddress", "DisplayName" });
          //  persons.Sort((p1, p2) => String.Compare(p1.Name, p2.Name, StringComparison.Ordinal));
            for (int i = 0; i < persons.Count; ++i)
                outputList.Add(new List<string>() { persons[i].Name, persons[i].FirstName + " " + persons[i].FatherName, persons[i].LastName, persons[i].Email, persons[i].LastName + " " + persons[i].FirstName + " " + persons[i].FatherName });
            persons.Clear();
            List<string> outputLines = new List<string>();
            for (int i = 0; i < outputList.Count; ++i)
            {
                StringBuilder tmpLine = new StringBuilder(outputList[i][0]);
                for (int j = 1; j < 5; ++j)
                    tmpLine.Append(";").Append(outputList[i][j]);
                outputLines.Add(tmpLine.ToString());
            }
            outputList.Clear();
            string fileName = Path.GetFileNameWithoutExtension(path);
            string folderPath = Path.GetDirectoryName(path);
            string fullPath = folderPath + "\\" + fileName + "-converted" + Path.GetExtension(path);
            File.WriteAllLines(fullPath, outputLines);
            outputLines.Clear();
        }
        static void Main(string[] args)
        {
#if DEBUG
            Parse("C:\\Job\\ЛСМ\\Козырев\\Задача 2\\susu2908.CSV");
#else
            if(args.Length == 0)
                return;
            for(int i = 0; i < args.Length; ++i)
                Parse(args[i]);
#endif
        }
    }
}
