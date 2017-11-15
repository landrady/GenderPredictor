using NameProcessor.Entities;
using NameProcessor.Exceptions;
using NameProcessor.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameProcessor.Services
{
    public class QuizService
    {
        public int CurrentIndex { get { return _currentIndex; } }

        private readonly IndexControlService _indexControlService;
        private readonly FileWrapper _fileWrapper;
        private readonly string _trainedDatabasePath;
        private readonly string[] _database;

        private int _currentIndex;
        public QuizService(string[] database, IndexControlService indexControlService, FileWrapper fileWrapper, string trainedDatabasePath)
        {
            this._indexControlService = indexControlService;
            this._database = database;
            this._trainedDatabasePath = trainedDatabasePath;
            this._fileWrapper = fileWrapper;
            _currentIndex = 0;
        }

        public async Task<Person> GetNext()
        {
            _currentIndex = await _indexControlService.GetLast();
            _currentIndex++;
            return new Person { Id = _currentIndex, Name = _database[_currentIndex - 1] };
        }

        public async Task Process(Person person, char value)
        {
            value = Char.ToLowerInvariant(value);
            Validate(value);
            if (value != 'n')
            {
                person.Sex = Char.ToUpperInvariant(value);
                await this._fileWrapper.Save(person.ToString(), _trainedDatabasePath, true);
            }
            await this._indexControlService.SaveLast(person.Id);
        }

        private void Validate(char value)
        {
            if (value != 'h' && value != 'm' && value != 'n')
            {
                throw new InvalidClassification(value);
            }
        }


    }
}
