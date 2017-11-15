using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameProcessor.Entities
{
    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Char Sex { get; set; }

        public override string ToString()
        {
            return String.Format("{0};{1};{2}", this.Id, this.Name, this.Sex);
        }
    }
}
