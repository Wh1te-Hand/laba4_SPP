using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorLib
{
    public class MethodData
    {
        public string Name { get; set; }
        public TypeSyntax ReturnType { get; set; }
        public List<ParametrsData> Parameters { get; set; }

        public MethodData(string name, TypeSyntax returnType, List<ParametrsData> parameters)
        {
            Name = name;
            ReturnType = returnType;
            Parameters = parameters;
        }
    }
}
