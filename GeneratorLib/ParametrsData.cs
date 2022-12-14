using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorLib
{
    public class ParametrsData
    {
        public string Name { get; set; }
        public TypeSyntax Type { get; }

        public ParametrsData(string name, TypeSyntax type)
        {
            Name = name;
            Type = type;
        }
    }
}
