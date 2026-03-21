# ПОСТАНОВКА ЗАДАЧІ
> Короткий опис загального завдання та обраних варіантів з поясненням суті кожного методу рефакторингу.
Метою роботи є розробка прототипу текстового редактора мовою програмування C#, призначеного для виконання рефакторингу вихідного коду програм мовою C++.

Для досягнення цієї мети необхідно виконати такі завдання:
- дослідити обрані методи рефакторингу, визначити їх призначення, вхідні та вихідні умови застосування;
- ознайомитися з принципами підходу TDD (Test Driven Development) та технологією створення модульних (unit) тестів;
- спроєктувати загальну архітектуру програми у вигляді UML-діаграми класів, що відображає основні компоненти системи та їх взаємозв’язки;
- створити прототипи класів із оголошенням методів без реалізації їх логіки;
- розробити модульні тести, що описують очікувану поведінку кожного методу рефакторингу (не менше 10 тестів для кожного варіанта).
  
У результаті виконання роботи має бути отримано прототип програмної системи без реалізації логіки методів рефакторингу та набір модульних тестів, які не проходять успішно. Це відповідає першому етапу підходу TDD — Red, на якому формулюються та перевіряються вимоги до майбутньої функціональності.

## Перейменування змінної
Рефакторинг «Перейменування змінної» застосовується у випадках, коли ім’я змінної не відображає її призначення або є недостатньо зрозумілим.
Метою цього методу є покращення читабельності та зрозумілості коду шляхом заміни існуючого імені змінної на більш інформативне. Під час виконання рефакторингу нове ім’я повинно бути замінене в усіх місцях використання змінної без зміни логіки роботи програми.

## Заміна магічного числа символічною константою
Магічні числа — це числові значення, що використовуються в програмному коді без пояснення їх призначення. Використання таких значень ускладнює розуміння програми та її подальшу модифікацію.
Рефакторинг «Заміна магічного числа символічною константою» полягає у створенні іменованої константи, яка відображає зміст відповідного числового значення. Після цього всі входження магічного числа у коді замінюються на створену константу. Такий підхід підвищує зрозумілість коду та полегшує його подальшу підтримку.

## Перейменування методу
Рефакторинг «Перейменування методу» використовується у випадках, коли назва методу не відповідає його фактичному призначенню або втратила актуальність у процесі розвитку програмного коду.
Метою цього методу є надання методу більш зрозумілого та змістовного імені, що відображає виконувану ним функцію. Під час рефакторингу необхідно змінити назву методу та всі його виклики у програмі без зміни функціональної поведінки системи.

## Видалення параметра
Рефакторинг «Видалення параметра» застосовується у випадках, коли параметр методу не використовується в його реалізації або є зайвим.
Наявність зайвих параметрів ускладнює інтерфейс методу та змушує розробників передавати непотрібні дані під час його виклику. Видалення таких параметрів спрощує структуру методу, підвищує зрозумілість коду та зменшує ймовірність помилок під час використання методу.


# 1 КОНЦЕПТУАЛЬНА МОДЕЛЬ СИСТЕМИ
> UML-діаграма класів із поясненнями структури та призначення компонентів.

Посилання для редагування діаграми (https://drive.google.com/file/d/1V6EcfGJ3mPW3xGk00QZrEFD3uISlAHzd/view?usp=sharing)

<img width="965" height="854" alt="UML drawio" src="https://github.com/user-attachments/assets/1a394872-8484-41b0-a792-017f70ef11b5" />


# 2 ПРОТОТИПИ КЛАСІВ
> Вихідний код оголошень класів і методів (без реалізації).
## Перейменування змінної
> <Ім'я>

```cs
<code>
```

## Заміна магічного числа символічною константою
> <Ім'я>

```cs
<code>
```

## Перейменування методу
> <Ім'я>

```cs
<code>
```

## Видалення параметра
> Солод Ігор

```cs
namespace RefactorApp
{
    public class RefactorRemoveParameterController
    {
        public string RemoveParameter(string CodeText, string methodName, string parameter)
        {
            return "";
        }
    }
}
```

# 3 МОДУЛЬНІ ТЕСТИ
> Вихідний код тестів з XML коментарями, що пояснюють, яку саме поведінку перевіряє кожен тест. Вказати розробника в звіті.
## Перейменування змінної
> <Ім'я>

```cs
<code>
```

## Заміна магічного числа символічною константою
> <Ім'я>

```cs
<code>
```

## Перейменування методу
> <Ім'я>

```cs
<code>
```

## Видалення параметра
> Солод Ігор

```cs
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
        /// Перевіряє поведінку системи, якщо параметр для видалення не знайдено.
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
            string code = "public void A(int x, int y) { } " + "public void B(int a, int b) { }";

            string result = refactorController.RemoveParameter(code, "A", "x");

            Assert.AreEqual("public void A(int y) { } public void B(int a, int b) { }", result);
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
        /// Перевіряє поведінку алгоритму, якщо метод не має параметрів.
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
```


# 4 РЕЗУЛЬТАТИ ЗАПУСКУ ТЕСТІВ
> Скриншот, що підтверджує невдале проходження всіх тестів (“червоний” стан).
## Перейменування змінної
> <Ім'я>


## Заміна магічного числа символічною константою
> <Ім'я>



## Перейменування методу
> <Ім'я>




## Видалення параметра
> Солод Ігор

<img width="1699" height="294" alt="image" src="https://github.com/user-attachments/assets/489e3cee-5be0-4e2a-9923-a5139eafba2c" />



# АНАЛІЗ РЕЗУЛЬТАТІВ ТА ВИСНОВКИ
