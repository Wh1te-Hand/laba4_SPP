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

            var classFiles = new List<string>();
            foreach (string Onefile in Directory.GetFiles(path, "*.cs"))
            {
                classFiles.Add(Onefile);
            }

            Console.WriteLine("END");
        }
    }



}