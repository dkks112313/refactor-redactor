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
        private RefactorRenameMethodController refactorController = new RefactorRenameMethodController();

        /// <summary>
        /// Коректна робота тесту
        /// </summary>
        [TestMethod]
        public void RenameMethod_ReturnRenameMethod()
        {
            var result = this.refactorController.RenameMethod("OldName", "NewName", null);
            Assert.AreEqual("NewName", result);
        }

        /// <summary>
        /// Відсутня стара назва методу
        /// </summary>
        [TestMethod]
        public void RenameMethod_EmptyOldName_ReturnRenameMethod()
        {
            var result = this.refactorController.RenameMethod("", "NewName", null);
            Assert.AreEqual("Error: empty parameter", result);
        }

        /// <summary>
        /// Відсутня нова назва тесту
        /// </summary>
        [TestMethod]
        public void RenameMethod_EmptyNewName_ReturnRenameMethod()
        {
            var result = this.refactorController.RenameMethod("OldName", "", null);
            Assert.AreEqual("OldName", result);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void RenameMethod_SpecialCharacters_ReturnRenameMethod()
        {
            var result = this.refactorController.RenameMethod("Old_Method1", "New_Method2", null);
            Assert.AreEqual("New_Method2", result);
        }

        /// <summary>
        /// Чутливість до регістру
        /// </summary>
        [TestMethod]
        public void RenameMethod_CaseSensitiveChange_ReturnsNewName()
        {
            var result = this.refactorController.RenameMethod("method", "Method", null);
            Assert.AreEqual("Method", result);
        }

        /// <summary>
        /// максимальна довжина назви методу
        /// </summary>
        [TestMethod]
        public void RenameMethod_MaxLengthName_ReturnsNewName()
        {
            var newName = new string('a', 255);

            var result = this.refactorController.RenameMethod("OldName", newName, null);

            Assert.AreEqual(newName, result);
        }

        /// <summary>
        /// Імена з пробілами
        /// </summary>
        [TestMethod]
        public void RenameMethod_NameWithSpaces_ReturnsNewName()
        {
            var result = this.refactorController.RenameMethod("OldName", "New Name", null);
            Assert.AreEqual("Error: spaces", result);
        }

        /// <summary>
        /// Неправильні символи
        /// </summary>
        [TestMethod]
        public void RenameMethod_InvalidCharacters_ReturnsNewName()
        {
            var result = this.refactorController.RenameMethod("OldName", "New@Name", null);
            Assert.AreEqual("Error: invalid characters", result);
        }

        /// <summary>
        /// Старе ім'я не знайдено
        /// </summary>
        [TestMethod]
        public void RenameMethod_OldNameNotFound_ReturnsNewName()
        {
            var result = this.refactorController.RenameMethod("Name", "NewName", null);
            Assert.AreEqual("Error: method name not found", result);
        }

        /// <summary>
        /// Назва починається з цифри
        /// </summary>
        [TestMethod]
        public void RenameMethod_StartsWithDigit_ReturnsNewName()
        {
            var result = this.refactorController.RenameMethod("OldName", "1NewName", null);
            Assert.AreEqual("Error: starts with digit", result);
        }
    }
}
