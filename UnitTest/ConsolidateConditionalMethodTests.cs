using Microsoft.VisualStudio.TestTools.UnitTesting;
using RefactoringEditor.Core.Methods;
using System;

namespace UnitTest
{
    [TestClass]
    public class ConsolidateConditionalMethodTests
    {
        private readonly ConsolidateConditionalMethod _refactoring = new ConsolidateConditionalMethod();

        // Перевіряємо базовий випадок: однаковий код в кінці блоків if та else має бути винесений
        [TestMethod]
        public void Apply_ReturnsConsolidatedCode_WhenIdenticalStatementInIfAndElse()
        {
            string source = "if (isSpecial) {\n  total = price * 0.95;\n  SendEmail();\n} else {\n  total = price * 0.98;\n  SendEmail();\n}";
            string expected = "if (isSpecial) {\n  total = price * 0.95;\n} else {\n  total = price * 0.98;\n}\nSendEmail();";
            
            string actual = _refactoring.Apply(source);
            
            Assert.AreEqual(expected, actual);
        }

        // Перевіряємо дублювання на початку блоків - код має бути винесений перед умовою
        [TestMethod]
        public void Apply_ReturnsConsolidatedCode_WhenIdenticalStatementIsAtBeginningOfBlocks()
        {
            string source = "if (a > b) {\n  Log(\"Started\");\n  DoA();\n} else {\n  Log(\"Started\");\n  DoB();\n}";
            string expected = "Log(\"Started\");\nif (a > b) {\n  DoA();\n} else {\n  DoB();\n}";
            
            string actual = _refactoring.Apply(source);
            
            Assert.AreEqual(expected, actual);
        }

        // Перевірка обробки null-значень (захист від випадання програми)
        [TestMethod]
        public void Apply_ThrowsException_IfSourceIsNull()
        {
            var ex = Assert.ThrowsException<ArgumentException>(() =>
            {
                _refactoring.Apply(null);
            });

            Assert.AreEqual("sourceCode", ex.ParamName);
        }

        // Перевірка обробки порожнього рядка
        [TestMethod]
        public void Apply_ThrowsException_IfSourceIsEmpty()
        {
            var ex = Assert.ThrowsException<ArgumentException>(() =>
            {
                _refactoring.Apply("");
            });

            Assert.AreEqual("sourceCode", ex.ParamName);
        }

        // Перевірка ситуації з кількома ідентичними рядками - всі вони мають бути консолідовані
        [TestMethod]
        public void Apply_ConsolidatesMultipleIdenticalLines()
        {
            string source = "if(x) { a=1; b=2; c=3; } else { d=4; b=2; c=3; }";
            string expected = "if(x) { a=1; } else { d=4; }\nb=2;\nc=3;";
            
            string actual = _refactoring.Apply(source);
            
            Assert.AreEqual(expected, actual);
        }

        // Перевірка вірної роботи алгоритму при глибокій вкладеності (nested if-else)
        [TestMethod]
        public void Apply_WorksWithNestedIfElse()
        {
            string source = "if(x) { if(y) { A(); B(); } else { C(); B(); } }";
            string expected = "if(x) { if(y) { A(); } else { C(); }\nB(); }";
            
            string actual = _refactoring.Apply(source);
            
            Assert.AreEqual(expected, actual);
        }

        // Перевірка, що алгоритм розпізнає однаковий код, незважаючи на різницю у пробілах
        [TestMethod]
        public void Apply_IgnoresWhitespaceDifferences()
        {
            string source = "if(x) { A();   Print(); } else { B(); \t Print();\n}";
            string expected = "if(x) { A(); } else { B(); }\nPrint();";
            
            string actual = _refactoring.Apply(source);
            
            Assert.AreEqual(expected, actual);
        }

        // Перевіряє коректне оновлення if-else конструкцій, якщо вони написані без фігурних дужок
        [TestMethod]
        public void Apply_LeavesSingleLineIfElseWithoutBracesProperlyUpdated()
        {
            string source = "if(test)\n  { A(); B(); }\nelse\n  { C(); B(); }";
            string expected = "if(test)\n  { A(); }\nelse\n  { C(); }\nB();";
            
            string actual = _refactoring.Apply(source);
            
            Assert.AreEqual(expected, actual);
        }

        // НОВИЙ ТЕСТ: Перевіряє, що дублюючий код всередині коментарів ігнорується алгоритмом
        [TestMethod]
        public void Apply_IgnoresDuplicationInsideComments()
        {
            // Справжній дублікат має змінитись, а закоментований — ні
            string source = "if(test){ a=1; } else { a=1; }\n/* \n if(x) { a=1; } else { a=1; } \n */";
            string expected = "if(test){ } else { }\na=1;\n/* \n if(x) { a=1; } else { a=1; } \n */"; 
            
            string actual = _refactoring.Apply(source);
            
            Assert.AreEqual(expected, actual);
        }

        // НОВИЙ ТЕСТ: Перевіряє, що умовна конструкція всередині строкового літералу не обробляється
        [TestMethod]
        public void Apply_IgnoresDuplicationInsideStringLiterals()
        {
            // Справжній дублікат має змінитись, а літерал — ні
            string source = "if(test){ b=2; } else { b=2; }\nstring script = \"if(x){ a=1;} else { a=1;}\";";
            string expected = "if(test){ } else { }\nb=2;\nstring script = \"if(x){ a=1;} else { a=1;}\";";
            
            string actual = _refactoring.Apply(source);
            
            Assert.AreEqual(expected, actual);
        }
    }
}
