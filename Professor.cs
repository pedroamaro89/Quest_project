using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QUEST
{
    public class Professor
    {
        public int ID { get; set; }
        public string Name { get; set; }



        public Professor()
        {
           
        }
        public Professor(int id, string name)
        {
            ID = id;
            Name = name;

        }



    }
}
