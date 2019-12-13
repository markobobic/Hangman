using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp10
{
  public  class Player
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value == null || string.IsNullOrWhiteSpace(value) ?
                throw new ArgumentException(nameof(value)) : _name = value;
            }
        }

        public Player(string name)
        {
            Name = name ?? throw new ArgumentException(nameof(name));
          
        }
        public Player():this("Guest")
        {
           
        }
       
    }
}
