using System.Linq;
using System.Text.RegularExpressions;

namespace RefactoringChange
{
    public class RefactorChangeController
    {
        public string RenameVariable(string sourceCode, string oldName, string newName)
        {

            // Якщо старе та нове ім'я однакові — код не потрібно змінювати
            if (oldName == newName)
                return sourceCode;

            // Тут тимчасово прибираються рядки:
            // "a = 5"
            // щоб змінна всередині них не перейменувалась

            string[] strings;

            // Замінюємо рядки на спеціальні мітки:
            // __STRING0__, __STRING1__ ...
            sourceCode = ProtectStrings(sourceCode, out strings);

            string[] comments;

            // Замінюємо коментарі на:
            // __COMMENT0__, __COMMENT1__ ...
            sourceCode = ProtectComments(sourceCode, out comments);

            // Перейменування змінної

            // \b означає "межа слова"
            // Це потрібно, щоб:
            // a -> b
            // не змінювало:
            // cat -> cbt

            string pattern = $@"\b{Regex.Escape(oldName)}\b";

            // Замінюємо тільки окремі слова
            sourceCode = Regex.Replace(sourceCode, pattern, newName);

            // Повернення коментарів

            for (int i = 0; i < comments.Length; i++)
            {
                sourceCode = sourceCode.Replace(
                    $"__COMMENT{i}__",
                    comments[i]
                );
            }

            // Повернення рядків

            for (int i = 0; i < strings.Length; i++)
            {
                sourceCode = sourceCode.Replace(
                    $"__STRING{i}__",
                    strings[i]
                );
            }

            // Повертаємо готовий код
            return sourceCode;
        }

        // Метод для тимчасового приховування рядків
        private string ProtectStrings(string code, out string[] strings)
        {
            // Знаходимо всі рядки "text"
            var matches = Regex.Matches(code, "\".*?\"");

            strings = new string[matches.Count];

            for (int i = 0; i < matches.Count; i++)
            {
                strings[i] = matches[i].Value;
            }

            // Замінюємо кожен рядок на мітку
            for (int i = 0; i < strings.Length; i++)
            {
                code = code.Replace(
                    strings[i],
                    $"__STRING{i}__"
                );
            }

            return code;
        }

        // Метод для тимчасового приховування коментарів
        private string ProtectComments(string code, out string[] comments)
        {
            // Знаходимо однорядкові коментарі // comment

            var matches = Regex.Matches(code, @"//.*");

            comments = new string[matches.Count];

            for (int i = 0; i < matches.Count; i++)
            {
                comments[i] = matches[i].Value;
            }

            // Замінюємо їх на спеціальні мітки
            for (int i = 0; i < comments.Length; i++)
            {
                code = code.Replace(
                    comments[i],
                    $"__COMMENT{i}__"
                );
            }

            return code;
        }
    }
}