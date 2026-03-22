using Microsoft.VisualStudio.TestTools.UnitTesting;
using RefactoringChange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Rem
{
    [TestClass]
    public class RenamingChangeTests
    {
        /// <summary>
        /// Змінна для зберігання екземпляра RenamingChangeRefactor.
        /// </summary>
        private RefactorRenamingChangeController removeParameterRefactor;

        [TestInitialize]
        public void Setup()
        {
            removeParameterRefactor = new RefactorRenamingChangeController();
        }

        /// <summary>
        /// Перевіряє, що перший параметр методу коректно видаляється.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ShouldRemoveFirstParameter()
        {
            string code = "public void Calculate(int a, int b) { }";

            string result = removeParameterRefactor.RenameVariable(code, "Calculate", "a");

            Assert.AreEqual("public void Calculate(int b) { }", result);
        }

        /// <summary>
        /// Перевіряє, що середній параметр видаляється.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ShouldRemoveMiddleParameter()
        {
            string code = "public void Calculate(int a, int b, int c) { }";

            string result = removeParameterRefactor.RenameVariable(code, "Calculate", "b");

            Assert.AreEqual("public void Calculate(int a, int c) { }", result);
        }

        /// <summary>
        /// Перевіряє, що останній параметр видаляється.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ShouldRemoveLastParameter()
        {
            string code = "public void Calculate(int a, int b) { }";

            string result = removeParameterRefactor.RenameVariable(code, "Calculate", "b");

            Assert.AreEqual("public void Calculate(int a) { }", result);
        }

        /// <summary>
        /// Перевіряє, що єдиний параметр видаляється.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ShouldRemoveOnlyParameter()
        {
            string code = "public void Calculate(int a) { }";

            string result = removeParameterRefactor.RenameVariable(code, "Calculate", "a");

            Assert.AreEqual("public void Calculate() { }", result);
        }

        /// <summary>
        /// Перевіряє, що якщо параметр не існує — код не змінюється.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ParameterNotFound_ShouldNotChangeCode()
        {
            string code = "public void Calculate(int a, int b) { }";

            string result = removeParameterRefactor.RenameVariable(code, "Calculate", "x");

            Assert.AreEqual(code, result);
        }

        /// <summary>
        /// Перевіряє, що інший метод не змінюється.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_WrongMethodName_ShouldNotChangeCode()
        {
            string code = "public void Test(int a, int b) { }";

            string result = removeParameterRefactor.RenameVariable(code, "Calculate", "a");

            Assert.AreEqual(code, result);
        }

        /// <summary>
        /// Перевіряє коректність роботи при нестандартних пробілах.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ShouldHandleSpacesCorrectly()
        {
            string code = "public void Calculate( int a , int b ) { }";

            string result = removeParameterRefactor.RenameVariable(code, "Calculate", "a");

            Assert.AreEqual("public void Calculate(int b) { }", result);
        }

        /// <summary>
        /// Перевіряє, що параметр видаляється коректно, незалежно від типу даних (об'єкти, масиви).
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ShouldHandleDifferentTypes()
        {
            string code = "public void Process(List<string> list, int[] numbers, double value) { }";

            string result = removeParameterRefactor.RenameVariable(code, "Process", "numbers");

            Assert.AreEqual("public void Process(List<string> list, double value) { }", result);
        }

        /// <summary>
        /// Перевіряє, що параметр видаляється лише у вказаному методі, навіть якщо в коді є інший метод з таким самим параметром.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_ShouldOnlyAffectSpecifiedMethod()
        {
            string code = "public void First(int a) { } public void Second(int a) { }";

            string result = removeParameterRefactor.RenameVariable(code, "Second", "a");

            Assert.AreEqual("public void First(int a) { } public void Second() { }", result);
        }

        /// <summary>
        /// Перевіряє поведінку системи, коли метод взагалі не має параметрів.
        /// </summary>
        [TestMethod]
        public void RemoveParameter_EmptyParameters_ShouldNotChangeCode()
        {
            string code = "public void Calculate() { }";

            string result = removeParameterRefactor.RenameVariable(code, "Calculate", "a");

            Assert.AreEqual("public void Calculate() { }", result);

        }
    }
}
