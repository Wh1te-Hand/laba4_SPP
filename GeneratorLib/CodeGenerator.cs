using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorLib
{
    public class CodeGenerator
    {
        public static List<FileInfo> Generate(List<NamespaceData> nsInfo, List<UsingDirectiveSyntax> usings)
        {
            var result = new List<FileInfo>();

            var generatedUsingsDeclaration = GenerateUsingsDeclaration(nsInfo, usings);
            foreach (var ns in nsInfo)
            {
                var generatedNsDeclaration = GenerateNsDeclaration(ns);
                foreach (var innerClass in ns.Classes)
                {
                    var generatedClassDeclaration = GenerateClassDeclaration(innerClass);
                    string fileName = ns.Name + "_" + innerClass.Name + ".cs";
                    string fileContent = generatedUsingsDeclaration.NormalizeWhitespace().ToFullString()
                        + "\r\n"
                        + generatedNsDeclaration.WithMembers(generatedClassDeclaration).NormalizeWhitespace().ToFullString();
                    result.Add(new FileInfo(fileName, fileContent));
                }
            }
            return result;
        }
    }
}
