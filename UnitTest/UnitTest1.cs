using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WindowsFormsApp6;

namespace UnitTest
{
    [TestClass]
    public class RemoveParameterTests
    {
        private RefactorController refactorController;

        [TestInitialize]
        public void Setup()
        {
            refactorController = new RefactorController();
        }

        [TestMethod]
        public void RemoveFirstParameter()
        {
            string codeText = "public void Calculate(int a, int b) { }";
            string methodName = "Calculate";
            string parameterToRemove = "a";

            string expected = "public void Calculate(int b) { }";
            string actual = refactorController.RemoveParameter(codeText, methodName, parameterToRemove);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveLastParameter()
        {
            string codeText = "public void PrintData(string name, int age) { }";
            string methodName = "PrintData";
            string parameterToRemove = "age";

            string expected = "public void PrintData(string name) { }";
            string actual = refactorController.RemoveParameter(codeText, methodName, parameterToRemove);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveMiddleParameter()
        {
            string codeText = "public void SendMessage(string from, string text, string to) { }";
            string methodName = "SendMessage";
            string parameterToRemove = "text";

            string expected = "public void SendMessage(string from, string to) { }";
            string actual = refactorController.RemoveParameter(codeText, methodName, parameterToRemove);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveOnlyParameter()
        {
            string codeText = "public void Reset(int value) { }";
            string methodName = "Reset";
            string parameterToRemove = "value";

            string expected = "public void Reset() { }";
            string actual = refactorController.RemoveParameter(codeText, methodName, parameterToRemove);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ParameterNotFound()
        {
            string codeText = "public void StartGame(int level, string mode) { }";
            string methodName = "StartGame";
            string parameterToRemove = "score";

            string expected = "public void StartGame(int level, string mode) { }";
            string actual = refactorController.RemoveParameter(codeText, methodName, parameterToRemove);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveDoubleParameter()
        {
            string codeText = "public void Multiply(double x, double y) { }";
            string methodName = "Multiply";
            string parameterToRemove = "x";

            string expected = "public void Multiply(double y) { }";
            string actual = refactorController.RemoveParameter(codeText, methodName, parameterToRemove);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveBoolParameter()
        {
            string codeText = "public void SetVisible(bool isVisible, string name) { }";
            string methodName = "SetVisible";
            string parameterToRemove = "isVisible";

            string expected = "public void SetVisible(string name) { }";
            string actual = refactorController.RemoveParameter(codeText, methodName, parameterToRemove);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveParameterFromStaticMethod()
        {
            string codeText = "public static void Log(string message, int level) { }";
            string methodName = "Log";
            string parameterToRemove = "level";

            string expected = "public static void Log(string message) { }";
            string actual = refactorController.RemoveParameter(codeText, methodName, parameterToRemove);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveParameterWithThreeArguments()
        {
            string codeText = "public void Move(int x, int y, int speed) { }";
            string methodName = "Move";
            string parameterToRemove = "y";

            string expected = "public void Move(int x, int speed) { }";
            string actual = refactorController.RemoveParameter(codeText, methodName, parameterToRemove);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveStringParameterFromThree()
        {
            string codeText = "public void RegisterUser(string login, string password, string email) { }";
            string methodName = "RegisterUser";
            string parameterToRemove = "password";

            string expected = "public void RegisterUser(string login, string email) { }";
            string actual = refactorController.RemoveParameter(codeText, methodName, parameterToRemove);

            Assert.AreEqual(expected, actual);
        }
    }
}