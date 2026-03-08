using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindowsFormsApp6;

namespace UnitTest
{
    [TestClass]
    public class RemoveParameterTests
    {
        /// <summary>
        /// Змінна для зберігання екземпляра RefactorController, який буде використовуватися у всіх тестах для виклику методу RemoveParameter.
        /// </summary>
        private RefactorController refactorController;

        [TestInitialize]
        public void Setup()
        {
            refactorController = new RefactorController();
        }

        /// <summary>
        /// Перевіряє, що перший параметр методу коректно видаляється.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ShouldRemoveFirstParameter()
        {
            string code = "public void Calculate(int a, int b) { }";

            string result = refactorController.RemoveParameter(code, "Calculate", "a");

            Assert.AreEqual("public void Calculate(int b) { }", result);
        }

        /// <summary>
        /// Перевіряє, що останній параметр методу коректно видаляється.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ShouldRemoveLastParameter()
        {
            string code = "public void PrintData(string name, int age) { }";

            string result = refactorController.RemoveParameter(code, "PrintData", "age");

            Assert.AreEqual("public void PrintData(string name) { }", result);
        }

        /// <summary>
        /// Перевіряє видалення параметра, що знаходиться в середині списку параметрів.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ShouldRemoveMiddleParameter()
        {
            string code = "public void SendMessage(string from, string text, string to) { }";

            string result = refactorController.RemoveParameter(code, "SendMessage", "text");

            Assert.AreEqual("public void SendMessage(string from, string to) { }", result);
        }

        /// <summary>
        /// Перевіряє видалення єдиного параметра методу. Після рефакторингу метод повинен залишитися без параметрів.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ShouldHandleSingleParameter()
        {
            string code = "public void Reset(int value) { }";

            string result = refactorController.RemoveParameter(code, "Reset", "value");

            Assert.AreEqual("public void Reset() { }", result);
        }

        /// <summary>
        /// Перевіряє поведінку системи, якщо параметр для видалення не знайдено. Код методу не повинен змінюватися.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ShouldNotChangeCode_WhenParameterNotFound()
        {
            string code = "public void StartGame(int level, string mode) { }";

            string result = refactorController.RemoveParameter(code, "StartGame", "score");

            Assert.AreEqual("public void StartGame(int level, string mode) { }", result);
        }

        /// <summary>
        /// Перевіряє, що параметр видаляється лише у зазначеному методі, навіть якщо в коді присутні інші методи.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ShouldAffectOnlySpecifiedMethod()
        {
            string code =
                "public void A(int x, int y) { } " +
                "public void B(int a, int b) { }";

            string result = refactorController.RemoveParameter(code, "A", "x");

            Assert.AreEqual(
                "public void A(int y) { } public void B(int a, int b) { }",
                result);
        }

        /// <summary>
        /// Перевіряє коректність роботи рефакторингу для статичного методу.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ShouldWorkForStaticMethod()
        {
            string code = "public static void Log(string message, int level) { }";

            string result = refactorController.RemoveParameter(code, "Log", "level");

            Assert.AreEqual("public static void Log(string message) { }", result);
        }

        /// <summary>
        /// Перевіряє поведінку алгоритму, якщо метод не має параметрів. У такому випадку код не повинен змінюватися.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ShouldNotChangeMethodWithoutParameters()
        {
            string code = "public void Start() { }";

            string result = refactorController.RemoveParameter(code, "Start", "x");

            Assert.AreEqual("public void Start() { }", result);
        }

        /// <summary>
        /// Перевіряє видалення параметра у методі з великою кількістю аргументів.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ShouldWorkWithMultipleParameters()
        {
            string code = "public void Move(int x, int y, int z, int speed) { }";

            string result = refactorController.RemoveParameter(code, "Move", "z");

            Assert.AreEqual("public void Move(int x, int y, int speed) { }", result);
        }

        /// <summary>
        /// Перевіряє, що рефакторинг не змінює тіло методу, а модифікує лише список параметрів.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ShouldNotModifyMethodBody()
        {
            string code = "public void Add(int a, int b) { int c = a + b; }";

            string result = refactorController.RemoveParameter(code, "Add", "b");

            Assert.AreEqual("public void Add(int a) { int c = a + b; }", result);
        }
    }
}