using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Address_Book_Comparator
{
    class Program
    {

        private class Person
        {

            public string LastName { get; private set; }
            public string Firstname { get; private set; }
            public string ExternalEmailAddress { get; private set; } = "";
            public string Name { get; private set; }
            public string DisplayName { get; private set; }
            public Person()
            {

            }
            public Person(string person)
            {
                var source = person.Split(';');
                this.DisplayName = source[4];
                this.ExternalEmailAddress = source[3];
                this.LastName = source[2];
                this.Firstname = source[1];
                this.Name = source[0];
            }
        }

        private static List<Person> GetPersons(string path)
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
                Environment.Exit(0);
            }
            List<Person> persons = new List<Person>();
            for (int i = 1; i < source.Count; ++i)
                persons.Add(new Person(source[i]));
            return persons;
        }
        static void Main(string[] args)
        {
            if (args.Length != 2)
                return;
            List<List<Person>> total = new List<List<Person>>();
            for(int i = 0; i < 2; ++i)
                total.Add(GetPersons(args[i]));
            List<Person> diffDeleted = new List<Person>(), diffAdded = new List<Person>();
            for (int i = 0; i < total[0].Count; ++i)
            {
                bool contains = false;
                for (int j = 0; j < total[1].Count; ++j)
                    if (total[0][i].Name == total[1][j].Name || total[0][i].DisplayName == total[1][j].DisplayName)
                    {
                        contains = true;
                        break;
                    }
                if(!contains)
                    diffDeleted.Add(total[0][i]);
            }
            for (int i = 0; i < total[1].Count; ++i)
            {
                bool contains = false;
                for (int j = 0; j < total[0].Count; ++j)
                    if (total[1][i].Name == total[0][j].Name || total[1][i].DisplayName == total[0][j].DisplayName)
                    {
                        contains = true;
                        break;
                    }
                if (!contains)
                    diffAdded.Add(total[1][i]);
            }

            string filePath = Path.GetDirectoryName(args[0]);
            string file1 = Path.GetFileNameWithoutExtension(args[0]);
            string file2 = Path.GetFileNameWithoutExtension(args[1]);
            string ext = Path.GetExtension(args[0]);

            string fileNameDeleted = $"{file1} (old) compare to {file2} (new) Deleted items{ext}";
            string fileNameAdded = $"{file1} (old) compare to {file2} (new) Added items{ext}";
            string pathD = filePath + "\\" + fileNameDeleted;
            string pathA = filePath + "\\" + fileNameAdded;

            {
                List<string> outputLines = new List<string>();
                outputLines.Add($"Name;Firstname;LastName;ExternalEmailAddress;DisplayName");
                for (int i = 0; i < diffDeleted.Count; ++i)
                    outputLines.Add(
                        $"{diffDeleted[i].Name};{diffDeleted[i].Firstname};{diffDeleted[i].LastName};{diffDeleted[i].ExternalEmailAddress};{diffDeleted[i].DisplayName}");
                File.WriteAllLines(pathD, outputLines);
            }

            {
                List<string> outputLines = new List<string>();
                outputLines.Add($"Name;Firstname;LastName;ExternalEmailAddress;DisplayName");
                for (int i = 0; i < diffAdded.Count; ++i)
                    outputLines.Add(
                        $"{diffAdded[i].Name};{diffAdded[i].Firstname};{diffAdded[i].LastName};{diffAdded[i].ExternalEmailAddress};{diffAdded[i].DisplayName}");
                File.WriteAllLines(pathA, outputLines);
            }

        }
    }
}
