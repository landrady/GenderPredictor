using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameBayes.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var trainner = GetTrainner();
            string name = string.Empty;

            while (name != "exit")
            {
                Console.WriteLine("Digite um nome: ");
                name = Console.ReadLine();

                Console.WriteLine(trainner.Classify(GetParameter(name, ""), true, 3));
            }


        }

        static Trainner GetTrainner()
        {
            var data = ReadFile("trained.txt");
            Trainner trainner = new Trainner(data, data[0].Length-1);
            trainner.Load(false);
            return trainner;
        }

        static string[][] ReadFile(string path)
        {
            string[] data;
            using (StreamReader sr = new StreamReader(path))
            {
                string all = sr.ReadToEnd();
                data = all.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            }
            string[][] result = new string[data.Length][];
            for (int i = 0; i < data.Length; i++)
            {
                var line = data[i].Split(';');
                string name = line[1];
                result[i] = GetParameter(name, line[2]);
            }
            return result;
        }

        static string[] GetParameter(string name, string gender)
        {
            name = name.ToLowerInvariant();
            return new string[]
                {
                    IsAEIOUY(name),
                    //LastWords(name,1),
                    LastWords(name,3),
                    //name,
                    //LastWords(name,2),
                    gender
                };
        }

        static string IsAEIOUY(string name)
        {
            var last = name.ToLowerInvariant().Last();
            if (last == 'a' || last == 'e' || last == 'i' || last == 'o' || last == 'u' || last == 'y')
            {
                return "S";
            }
            return "N";
        }

        static string LastWords(string name, int i)
        {
            return name.Substring(Math.Max(0, name.Length - i));
        }
    }
}
