using GeneratorLib;

namespace lab4_SPP
{

    class Program
    {

        static async Task Main()
        {
            await Method1();
        }


        public static async Task Method1()
        {
            string path = @"..\\..\\..\\Files";
            string resultPath = @"C:\Users\White Hand\source\repos\laba4_SPP\testGenerator\Result\";

            var classFiles = new List<string>();
            foreach (string Onefile in Directory.GetFiles(path, "*.cs"))
            {
                classFiles.Add(Onefile);
            }
            int maxFilesToLoad = 2;
            int maxExecuteTasks = 2;
            int maxFilesToWrite = 2;
            Generator generator = new Generator(classFiles, resultPath, maxFilesToLoad, maxExecuteTasks, maxFilesToWrite);
            await generator.Generate();

            Console.WriteLine("END");
        }
    }



}