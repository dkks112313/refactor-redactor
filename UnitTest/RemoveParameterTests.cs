using Microsoft.VisualStudio.TestTools.UnitTesting;
using RefactorApp;

namespace UnitTest
{
    [TestClass]
    public class RemoveParameterTests
    {
        /// <summary>
        /// Змінна для зберігання екземпляра RefactorController, який буде використовуватися у всіх тестах.
        /// </summary>
        private RefactorRemoveParameterController refactorController;

        /// <summary>
        /// Ініціалізує екземпляр RefactorController перед кожним тестом, щоб забезпечити чистий стан для кожного тесту.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            refactorController = new RefactorRemoveParameterController();
        }

        /// <summary>
        /// Перевіряє, що перший параметр методу коректно видаляється.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ShouldRemoveFirstParameter()
        {
            string code = @"
                class Calculator {
                public:
                    void Calculate(int a, int b) { }
                };";

            string expected = @"
                class Calculator {
                public:
                    void Calculate(int b) { }
                };";


            string result = refactorController.RemoveParameter(code, "Calculate", "a");

            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Перевіряє, що останній параметр методу коректно видаляється.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ShouldRemoveLastParameter()
        {
            string code = @"
                class Printer {
                public:
                    void PrintData(string name, int age) { }
                };";

            string expected = @"
                class Printer {
                public:
                    void PrintData(string name) { }
                };";

            string result = refactorController.RemoveParameter(code, "PrintData", "age");

            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Перевіряє видалення параметра, що знаходиться в середині списку параметрів.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ShouldRemoveMiddleParameter()
        {
            string code = @"
                class Messenger {
                public:
                    void SendMessage(string from, string text, string to) { }
                };";

            string expected = @"
                class Messenger {
                public:
                    void SendMessage(string from, string to) { }
                };";

            string result = refactorController.RemoveParameter(code, "SendMessage", "text");

            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Перевіряє видалення єдиного параметра методу. Після рефакторингу метод повинен залишитися без параметрів.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ShouldHandleSingleParameter()
        {
            string code = @"
                class System {
                public:
                    void Reset(int value) { }
                };";

            string expected = @"
                class System {
                public:
                    void Reset() { }
                };";

            string result = refactorController.RemoveParameter(code, "Reset", "value");

            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Перевіряє поведінку системи, якщо параметр для видалення не знайдено.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ShouldNotChangeCode_WhenParameterNotFound()
        {
            string code = @"
                class Game {
                public:
                    void StartGame(int level, string mode) { }
                };";

            string expected = code;

            string result = refactorController.RemoveParameter(code, "StartGame", "score");

            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Перевіряє, що параметр видаляється лише у зазначеному методі, навіть якщо в коді присутні інші методи.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ShouldAffectOnlySpecifiedMethod()
        {
            string code = @"
                class Linker {
                public:
                    void Start(int x, int y) { }
                    void End(int a, int b) { }
                };";

            string expected = @"
                class Linker {
                public:
                    void Start(int y) { }
                    void End(int a, int b) { }
                };";


            string result = refactorController.RemoveParameter(code, "Start", "x");

            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Перевіряє видалення параметра, який є посиланням.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ShouldWorkWithReferenceParameter()
        {
            string code = @"
                class DataProcessor {
                public:
                    void Process(std::string& data, int count) { }
                };";

            string expected = @"
                class DataProcessor {
                public:
                    void Process(int count) { }
                };";

            string result = refactorController.RemoveParameter(code, "Process", "data");

            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Перевіряє видалення параметра, який є шаблонним типом.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ShouldWorkWithTemplateParameter()
        {
            string code = @"
                #include <vector>

                class Container {
                public:
                    void Add(std::vector<int> items, int count) { }
                };";

            string expected = @"
                #include <vector>

                class Container {
                public:
                    void Add(int count) { }
                };";

            string result = refactorController.RemoveParameter(code, "Add", "items");

            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Перевіряє поведінку алгоритму, якщо метод не має параметрів.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ShouldNotChangeMethodWithoutParameters()
        {
            string code = @"
                class App {
                public:
                    void Enter() { }
                };";

            string expected = code;

            string result = refactorController.RemoveParameter(code, "Enter", "x");

            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Перевіряє видалення параметра у методі з великою кількістю аргументів.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ShouldWorkWithMultipleParameters()
        {
            string code = @"
                class Player {
                public:
                    void Move(int x, int y, int z, int speed, string name) { }
                };";

            string expected = @"
                class Player {
                public:
                    void Move(int x, int y, int speed, string name) { }
                };";

            string result = refactorController.RemoveParameter(code, "Move", "z");

            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Перевіряє, що рефакторинг не змінює тіло методу, а модифікує лише список параметрів.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ShouldNotModifyMethodBody()
        {
            string code = @"
                class Math {
                public:
                    void Add(int a, int b) { int c = a + b; }
                };";

            string expected = @"
                class Math {
                public:
                    void Add(int a) { int c = a + b; }
                };";

            string result = refactorController.RemoveParameter(code, "Add", "b");

            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Перевіряє видалення параметра з методу, який має складну сигнатуру з посиланнями та шаблонними типами.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ShouldHandleComplexSignature()
        {
            string code = @"
                class Network {
                public:
                    void Send(const std::string& host, int port, std::vector<int> data) { }
                };";

            string expected = @"
                class Network {
                public:
                    void Send(const std::string& host, std::vector<int> data) { }
                };";

            string result = refactorController.RemoveParameter(code, "Send", "port");

            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Перевіряє, чи видалення параметра за назвою не призводить до видалення параметрів із схожими, але неідентичними назвами.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ShouldNotRemoveSimilarNames()
        {
            string code = @"
                Class Logger {
                public:
                    void Receive(object sender, EventArgs e) {};
                };";

            string expected = @"
                Class Logger {
                public:
                    void Receive(object sender) {};
                };";

            string result = refactorController.RemoveParameter(code, "Receive", "e");

            Assert.AreEqual(expected, result);
        }
    }
}