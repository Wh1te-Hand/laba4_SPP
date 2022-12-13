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
            return null;
        }

    }
}