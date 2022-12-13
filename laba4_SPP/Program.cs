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
            string resultPath = @"..\\..\\..\\..\\TestProject\\ResultTests";

            var classFiles = new List<string>();
            foreach (string Onefile in Directory.GetFiles(path, "*.cs"))
            {
                classFiles.Add(Onefile);
            }
            int maxFilesToLoad = 1;
            int maxExecuteTasks = 1;
            int maxFilesToWrite = 1;
            Generator generator = new Generator(classFiles, resultPath, maxFilesToLoad, maxExecuteTasks, maxFilesToWrite);
            await generator.Generate();

            Console.WriteLine("END");
        }
    }



}