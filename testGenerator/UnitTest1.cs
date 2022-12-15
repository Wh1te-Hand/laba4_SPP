using GeneratorLib;

namespace testGenerator
{
    [TestClass]
    public class UnitTest1
    {
        private string generatedClassFolder;
        string testClassesFolder;
        [TestInitialize]
        public async Task SetupAsync()
        {
            generatedClassFolder = @"..\\..\\..\\..\\Result";
            testClassesFolder = @"..\\..\\..\\..\\laba4_SPP\\Files";
            var allTestClasses = new List<string>();
            foreach (string path in Directory.GetFiles(testClassesFolder, "*.cs"))
            {
                allTestClasses.Add(path);
            }
            Generator generator = new(allTestClasses, generatedClassFolder, 3, 3, 3);
            await generator.Generate();
        }

        [TestMethod]
        public void Dividing_To_OneClassFiles()
        {
            var generatedFiles = new List<string>();
            foreach (string Onefile in Directory.GetFiles(generatedClassFolder, "*.cs"))
            {
                generatedFiles.Add(Onefile);
            }
            var classFiles = new List<string>();
            foreach (string Onefile in Directory.GetFiles(testClassesFolder, "*.cs"))
            {
                classFiles.Add(Onefile);
            }
            Assert.AreEqual(generatedFiles.Count(), 8);
            Assert.AreEqual(classFiles.Count(), 5);
        }

        [TestMethod]
        public void One_class_one_namespace()
        {

            var classCount = Directory.GetFiles(generatedClassFolder)
                .Count(file => file.Contains("extra_"));
            Assert.AreEqual(classCount, 1);
        }

        [TestMethod]
        public void two_class_two_namespace()
        {

            var class4Count = Directory.GetFiles(generatedClassFolder)
                 .Count(file => file.Contains("Class4"));
            var class5Count = Directory.GetFiles(generatedClassFolder)
                .Count(file => file.Contains("Class5"));

            Assert.AreEqual(class4Count, 1);
            Assert.AreEqual(class5Count, 1);
        }
    }
}