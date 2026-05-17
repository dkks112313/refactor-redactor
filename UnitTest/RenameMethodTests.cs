using Microsoft.VisualStudio.TestTools.UnitTesting;
using RefactoringApp;
using RefactorApp;
using RefactoringTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass]
    public class RenameMethodTests
    {
        private RefactorRenameMethodController refactorController =
            new RefactorRenameMethodController();

        /// <summary>
        /// Коректне перейменування методу
        /// </summary>
        [TestMethod]
        public void RenameMethod_ReturnRenameMethod()
        {
            string code = "void OldName() { }";

            var result = this.refactorController.RenameMethod(
                "OldName",
                "NewName",
                code
            );

            Assert.IsTrue(result.Contains("NewName"));
        }

        /// <summary>
        /// Відсутня стара назва методу
        /// </summary>
        [TestMethod]
        public void RenameMethod_EmptyOldName_ReturnRenameMethod()
        {
            string code = "void OldName() { }";

            var result = this.refactorController.RenameMethod(
                "",
                "NewName",
                code
            );

            Assert.AreEqual("Error: empty parameter", result);
        }

        /// <summary>
        /// Відсутня нова назва методу
        /// </summary>
        [TestMethod]
        public void RenameMethod_EmptyNewName_ReturnRenameMethod()
        {
            string code = "void OldName() { }";

            var result = this.refactorController.RenameMethod(
                "OldName",
                "",
                code
            );

            Assert.AreEqual("Error: empty parameter", result);
        }

        /// <summary>
        /// Назви з підкресленням
        /// </summary>
        [TestMethod]
        public void RenameMethod_SpecialCharacters_ReturnRenameMethod()
        {
            string code = "void Old_Method1() { }";

            var result = this.refactorController.RenameMethod(
                "Old_Method1",
                "New_Method2",
                code
            );

            Assert.IsTrue(result.Contains("New_Method2"));
        }

        /// <summary>
        /// Чутливість до регістру
        /// </summary>
        [TestMethod]
        public void RenameMethod_CaseSensitiveChange_ReturnsNewName()
        {
            string code = "void method() { }";

            var result = this.refactorController.RenameMethod(
                "method",
                "Method",
                code
            );

            Assert.IsTrue(result.Contains("Method"));
        }

        /// <summary>
        /// Максимальна довжина назви методу
        /// </summary>
        [TestMethod]
        public void RenameMethod_MaxLengthName_ReturnsNewName()
        {
            var newName = new string('a', 255);

            string code = "void OldName() { }";

            var result = this.refactorController.RenameMethod(
                "OldName",
                newName,
                code
            );

            Assert.IsTrue(result.Contains(newName));
        }

        /// <summary>
        /// Імена з пробілами
        /// </summary>
        [TestMethod]
        public void RenameMethod_NameWithSpaces_ReturnsError()
        {
            string code = "void OldName() { }";

            var result = this.refactorController.RenameMethod(
                "OldName",
                "New Name",
                code
            );

            Assert.AreEqual("Error: spaces", result);
        }

        /// <summary>
        /// Неправильні символи
        /// </summary>
        [TestMethod]
        public void RenameMethod_InvalidCharacters_ReturnsError()
        {
            string code = "void OldName() { }";

            var result = this.refactorController.RenameMethod(
                "OldName",
                "New@Name",
                code
            );

            Assert.AreEqual("Error: invalid characters", result);
        }

        /// <summary>
        /// Старе ім'я не знайдено
        /// </summary>
        [TestMethod]
        public void RenameMethod_OldNameNotFound_ReturnsError()
        {
            string code = "void AnotherMethod() { }";

            var result = this.refactorController.RenameMethod(
                "OldName",
                "NewName",
                code
            );

            Assert.AreEqual("Error: method name not found", result);
        }

        /// <summary>
        /// Назва починається з цифри
        /// </summary>
        [TestMethod]
        public void RenameMethod_StartsWithDigit_ReturnsError()
        {
            string code = "void OldName() { }";

            var result = this.refactorController.RenameMethod(
                "OldName",
                "1NewName",
                code
            );

            Assert.AreEqual("Error: starts with digit", result);
        }
    }
}
