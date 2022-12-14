using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading.Tasks.Dataflow;

namespace GeneratorLib
{
    public class Generator
    {
        private List<string> sourceFiles;
        private string destFolder;
        private ExecutionDataflowBlockOptions loadFileBO;
        private ExecutionDataflowBlockOptions taskCountBO;
        private ExecutionDataflowBlockOptions writeFileBO;

        public Generator(List<string> _sourceFiles, string _destFolder, int maxFilesToLoadCount, int maxExecuteTasksCount, int maxFilesToWriteCount)
        {
            sourceFiles = _sourceFiles;
            destFolder = _destFolder;
            loadFileBO = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = maxFilesToLoadCount };
            taskCountBO = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = maxExecuteTasksCount };
            writeFileBO = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = maxFilesToWriteCount };
        }
        private async Task<FileInfo> ReadFiles(string sourceFile)
        {
            Console.WriteLine("Reading.....");
            string content;
            using (var reader = new StreamReader(new FileStream(sourceFile, FileMode.Open)))
            {
                content = await reader.ReadToEndAsync();
            }
            return new FileInfo(sourceFile, content);
        }

        private async Task WriteToFile(List<FileInfo> fileInfo)
        {
            Console.WriteLine("Writing.....");
            foreach (var fi in fileInfo)
            {
                using var writer = new StreamWriter(
                        new FileStream(Path.Combine(destFolder, fi.Name), FileMode.Create));
                await writer.WriteAsync(fi.Code);
            }
        }

        public Task Generate()
        {
            var loadClasses = new TransformBlock<string, FileInfo>(new Func<string, Task<FileInfo>>(ReadFiles), loadFileBO);
            var generateTestClasses = new TransformBlock<FileInfo, List<FileInfo>>(new Func<FileInfo, Task<List<FileInfo>>>(GenerateTests), taskCountBO);
            var writeToFile = new ActionBlock<List<FileInfo>>(async input => { await WriteToFile(input); }, writeFileBO);

            var linkOptions = new DataflowLinkOptions() { PropagateCompletion = true };
            loadClasses.LinkTo(generateTestClasses, linkOptions);
            generateTestClasses.LinkTo(writeToFile, linkOptions);

            foreach (var sourceFile in sourceFiles)
            {
                loadClasses.Post(sourceFile);
            }
            loadClasses.Complete();

            return writeToFile.Completion;
        }

        private async Task<List<FileInfo>> GenerateTests(FileInfo fi)
        {
            Console.WriteLine("Generating.....");
            return await GenerateCode(fi);
        }
        private async Task<List<FileInfo>> GenerateCode(FileInfo fi)
        {
            var root = await CSharpSyntaxTree.ParseText(fi.Code).GetRootAsync();
            return GenerateCodeFromTree(root);
        }


        private List<FileInfo> GenerateCodeFromTree(SyntaxNode root)
        {
            var usingDirectives = new List<UsingDirectiveSyntax>(root
                .DescendantNodes()
                .OfType<UsingDirectiveSyntax>());
            var namespaces = new List<NamespaceDeclarationSyntax>(root
                .DescendantNodes()
                .OfType<NamespaceDeclarationSyntax>());

            var nsInfo = new List<NamespaceData>();
            foreach (var ns in namespaces)
            {
                var innerClasses = ns.DescendantNodes().OfType<ClassDeclarationSyntax>();
                var innerNsClasses = new List<ClassData>();
                foreach (var innerNsClass in innerClasses)
                {
                    innerNsClasses.Add(new ClassData(innerNsClass.Identifier.ToString(),
                        GetMethods(innerNsClass)));
                }
                nsInfo.Add(new NamespaceData(ns.Name.ToString(), innerNsClasses));
            }
            return CodeGenerator.Generate(nsInfo, usingDirectives);
        }

        private List<MethodData> GetMethods(ClassDeclarationSyntax innerNsClass)
        {
            var methods = innerNsClass
                .DescendantNodes()
                .OfType<MethodDeclarationSyntax>();
            var result = new List<MethodData>();
            foreach (var method in methods)
            {
                result.Add(new MethodData(method.Identifier.ToString(),
                    method.ReturnType, GetParameters(method)));
            }
            return result;
        }


        private List<ParametrsData> GetParameters(MethodDeclarationSyntax method)
        {
            return method.ParameterList.Parameters
                .Select(param => new ParametrsData(param.Identifier.Value.ToString(), param.Type))
                .ToList();
        }

    }
}