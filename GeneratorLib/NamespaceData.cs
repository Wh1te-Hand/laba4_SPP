using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorLib
{
    public class NamespaceData
    {
        public string Name { get; set; }
        public List<Object> Classes { get; set; }//add class

        public NamespaceData(string name, List<Object> classes)
        {
            Name = name;
            Classes = classes;
        }

    }
}
