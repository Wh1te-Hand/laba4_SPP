namespace GeneratorLib
{
    public class Generator
    {
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


    }
}