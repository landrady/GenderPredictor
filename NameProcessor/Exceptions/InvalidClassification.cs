using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameProcessor.Exceptions
{
    public class InvalidClassification : Exception
    {
        const string message = "Caracter invalido {0}, apenas permitido h,m ou n";
        public InvalidClassification(char value) : base(string.Format(message,value.ToString()))
        {

        }
    }
}
