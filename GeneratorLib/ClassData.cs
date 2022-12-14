using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorLib
{
    public class ClassData
    {
        public string Name { get; }
        public List<MethodData> Methods { get; }

        public ClassData(string name, List<MethodData> methods)
        {
            Name = name;
            Methods = methods;
        }
    }
}
