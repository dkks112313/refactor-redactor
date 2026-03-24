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
	public class ChangeTests
	{
		/// <summary>
		/// Змінна для зберігання екземпляра ChangeRefactor.
		/// </summary>
		private RefactorChangeController changeRefactor;

		[TestInitialize]
		public void Setup()
		{
			changeRefactor = new RefactorChangeController();
		}

		/// <summary>
		/// Перевіряє просте перейменування змінної.
		/// </summary>
		[TestMethod]
		public void ChangeVariable_ShouldRenameSimpleVariable()
		{
			string code = "int a = 5;";

			string result = changeRefactor.RenameVariable(code, "a", "b");

			Assert.AreEqual("int b = 5;", result);
		}

		/// <summary>
		/// Перевіряє перейменування змінної у виразі.
		/// </summary>
		[TestMethod]
		public void ChangeVariable_ShouldRenameInExpression()
		{
			string code = "int a = 5; int c = a + 2;";

			string result = changeRefactor.RenameVariable(code, "a", "b");

			Assert.AreEqual("int b = 5; int c = b + 2;", result);
		}

		/// <summary>
		/// Перевіряє, що частини інших слів не змінюються.
		/// </summary>
		[TestMethod]
		public void ChangeVariable_ShouldNotRenamePartOfWord()
		{
			string code = "int cat = 5;";

			string result = changeRefactor.RenameVariable(code, "a", "b");

			Assert.AreEqual("int cat = 5;", result);
		}

		/// <summary>
		/// Перевіряє перейменування у циклі.
		/// </summary>
		[TestMethod]
		public void ChangeVariable_ShouldRenameInLoop()
		{
			string code = "for(int a = 0; a < 10; a++) { }";

			string result = changeRefactor.RenameVariable(code, "a", "b");

			Assert.AreEqual("for(int b = 0; b < 10; b++) { }", result);
		}

		/// <summary>
		/// Перевіряє перейменування у межах методу.
		/// </summary>
		[TestMethod]
		public void ChangeVariable_ShouldRenameInsideMethod()
		{
			string code = "void Test() { int a = 1; }";

			string result = changeRefactor.RenameVariable(code, "a", "b");

			Assert.AreEqual("void Test() { int b = 1; }", result);
		}

		/// <summary>
		/// Перевіряє, що змінні у рядках не змінюються.
		/// </summary>
		[TestMethod]
		public void ChangeVariable_ShouldNotRenameInsideString()
		{
			string code = "Console.WriteLine(\"a = 5\");";

			string result = changeRefactor.RenameVariable(code, "a", "b");

			Assert.AreEqual("Console.WriteLine(\"a = 5\");", result);
		}

		/// <summary>
		/// Перевіряє, що змінні у коментарях не змінюються.
		/// </summary>
		[TestMethod]
		public void ChangeVariable_ShouldNotRenameInsideComment()
		{
			string code = "int a = 5; // a variable";

			string result = changeRefactor.RenameVariable(code, "a", "b");

			Assert.AreEqual("int b = 5; // a variable", result);
		}

		/// <summary>
		/// Перевіряє, що змінна з підкресленням коректно перейменовується.
		/// </summary>
		[TestMethod]
		public void ChangeVariable_ShouldHandleUnderscoreNames()
		{
			string code = "int my_var = 10; int x = my_var + 1;";

			string result = changeRefactor.RenameVariable(code, "my_var", "new_var");

			Assert.AreEqual("int new_var = 10; int x = new_var + 1;", result);
		}

		/// <summary>
		/// Перевіряє, що змінна не змінюється, якщо нове ім'я таке ж саме.
		/// </summary>
		[TestMethod]
		public void ChangeVariable_SameName_ShouldNotChangeCode()
		{
			string code = "int a = 5;";


			string result = changeRefactor.RenameVariable(code, "a", "a");

			Assert.AreEqual(code, result);
		}

		/// <summary>
		/// Перевіряє, що змінна з цифрами в імені коректно перейменовується.
		/// </summary>
		[TestMethod]
		public void ChangeVariable_ShouldHandleNamesWithNumbers()
		{
			string code = "int a1 = 5; int b = a1 + 2;";

			string result = changeRefactor.RenameVariable(code, "a1", "x1");

			Assert.AreEqual("int x1 = 5; int b = x1 + 2;", result);
		}
	}
}
