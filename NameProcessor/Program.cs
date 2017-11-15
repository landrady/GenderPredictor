using NameProcessor.Entities;
using NameProcessor.Infra;
using NameProcessor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            string controlPath = "controle";
            string databasePath = "AppData/names";
            string trainedFilePath = "trained.txt";
            string[] database;
            FileWrapper fileWrapper = new FileWrapper();
            IndexControlService indexControlService = new IndexControlService(fileWrapper, controlPath);
            TextCleanerService textCleanerService = new TextCleanerService();
            DatabaseCleanerService dbCleanerService = new DatabaseCleanerService(textCleanerService);
            database = fileWrapper.GetDataLines(databasePath).Result;
            database = dbCleanerService.Clean(database);
            QuizService quizService = new QuizService(database, indexControlService, fileWrapper, trainedFilePath);
            UIInteractive(quizService, database);
        }

        static void UIInteractive(QuizService quizService, string[] database)
        {
            Console.Clear();
            Person person = quizService.GetNext().Result;
            Console.WriteLine(string.Format("Treinando nomes {0} de {1}", quizService.CurrentIndex + 1, database.Length));
            Console.WriteLine(string.Format( "Qual o sexo de {0} ?", person.Name));
            var key = Console.ReadKey();
            quizService.Process(person, key.KeyChar).Wait();

            if(quizService.CurrentIndex < database.Length-1)
            {
                UIInteractive(quizService, database);
            }

            Console.WriteLine("Fim!");
        }
    }
}
