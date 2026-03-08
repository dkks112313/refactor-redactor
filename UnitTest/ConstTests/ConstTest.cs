using Microsoft.VisualStudio.TestTools.UnitTesting;
using RefactoringTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.ConstTests
{
    [TestClass]
    public class UnitTest1
    {
        private MagicNumberRefactoring refactoring = new MagicNumberRefactoring();

        /// <summary>
        /// Verifies replacement of a single magic number with a constant.
        /// </summary>
        [TestMethod]
        public void TestReplacementASingleMagicNumber()
        {
            string input = "int x = 10;";
            string result = refactoring.ReplaceMagicNumber(input);
            Assert.AreEqual("const int MAGIC_NUMBER = 10;\nint x = MAGIC_NUMBER;", result);
        }

        /// <summary>
        /// Verifies replacement of a magic number inside an expression.
        /// </summary>
        [TestMethod]
        public void TestReplaceAMagicNumberInExpression()
        {
            string input = "int y = a + 5;";
            string result = refactoring.ReplaceMagicNumber(input);
            Assert.AreEqual("const int MAGIC_NUMBER = 5;\nint y = a + MAGIC_NUMBER;", result);
        }

        /// <summary>
        /// Verifies replacement of a magic number in a conditional statement.
        /// </summary>
        [TestMethod]
        public void TestReplaceAMagicNumberInConditional()
        {
            string input = "if (x > 100)";
            string result = refactoring.ReplaceMagicNumber(input);
            Assert.AreEqual("const int MAGIC_NUMBER = 100;\nif (x > MAGIC_NUMBER)", result);
        }

        /// <summary>
        /// Verifies replacement of a magic number inside a loop.
        /// </summary>
        [TestMethod]
        public void TestReplaceAMagicNumberInsideLoop()
        {
            string input = "for(int i=0;i<10;i++)";
            string result = refactoring.ReplaceMagicNumber(input);
            Assert.AreEqual("const int MAGIC_NUMBER = 10;\nfor(int i=0;i<MAGIC_NUMBER;i++)", result);
        }

        /// <summary>
        /// Verifies replacement of multiple identical magic numbers.
        /// </summary>
        [TestMethod]
        public void TestReplaceAMultipleMagicNumbers()
        {
            string input = "int a = 5; int b = 5;";
            string result = refactoring.ReplaceMagicNumber(input);
            Assert.AreEqual("const int MAGIC_NUMBER = 5;\nint a = MAGIC_NUMBER; int b = MAGIC_NUMBER;", result);
        }

        /// <summary>
        /// Verifies replacement of a magic number in an arithmetic expression.
        /// </summary>
        [TestMethod]
        public void TestReplaceAMagicNumberInArithmeticExpression()
        {
            string input = "int z = 3 * value;";
            string result = refactoring.ReplaceMagicNumber(input);
            Assert.AreEqual("const int MAGIC_NUMBER = 3;\nint z = MAGIC_NUMBER * value;", result);
        }

        /// <summary>
        /// Verifies replacement of a large magic number.
        /// </summary>
        [TestMethod]
        public void TestReplaceALargeMagicNumber()
        {
            string input = "int max = 1000;";
            string result = refactoring.ReplaceMagicNumber(input);
            Assert.AreEqual("const int MAGIC_NUMBER = 1000;\nint max = MAGIC_NUMBER;", result);
        }

        /// <summary>
        /// Verifies replacement of a negative magic number.
        /// </summary>
        [TestMethod]
        public void TestReplaceANegativeMagicNumber()
        {
            string input = "int x = -5;";
            string result = refactoring.ReplaceMagicNumber(input);
            Assert.AreEqual("const int MAGIC_NUMBER = -5;\nint x = MAGIC_NUMBER;", result);
        }

        /// <summary>
        /// Verifies replacement of a magic number in a return statement.
        /// </summary>
        [TestMethod]
        public void TestReplaceAMagicNumberInReturnStatement()
        {
            string input = "return 7;";
            string result = refactoring.ReplaceMagicNumber(input);
            Assert.AreEqual("const int MAGIC_NUMBER = 7;\nreturn MAGIC_NUMBER;", result);
        }

        /// <summary>
        /// Verifies replacement of a magic number appearing in multiple lines of code.
        /// </summary>
        [TestMethod]
        public void TestReplaceAMagicNumberInMultipleLinesOfCode()
        {
            string input = "uint a = 2;\nint b = a * 2;";
            string result = refactoring.ReplaceMagicNumber(input);
            Assert.AreEqual("const int MAGIC_NUMBER = 2;\nuint a = MAGIC_NUMBER;\nint b = a * MAGIC_NUMBER;", result);
        }
    }
}
