using RefactoringApp;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RefactoringChange
{
    public class RefactorRenameVariableController : RefactoringMethods
    {
        public string Name => "Rename Variable";

        public List<RefactorParameter> GetParameters()
        {
            return new List<RefactorParameter>
            {
                new RefactorParameter
                {
                    Name = "Old Name",
                    Value = "oldName"
                },

                new RefactorParameter
                {
                    Name = "New Name",
                    Value = "newName"
                }
            };
        }

        public string Execute(
            string code,
            Dictionary<string, string> parameters)
        {
            return RenameVariable(
                code,
                parameters["oldName"],
                parameters["newName"]
            );
        }

        public string RenameVariable(
            string sourceCode,
            string oldName,
            string newName)
        {
            if (string.IsNullOrWhiteSpace(oldName) ||
                string.IsNullOrWhiteSpace(newName))
            {
                return "Error: empty parameter";
            }

            if (oldName == newName)
            {
                return sourceCode;
            }

            // ФИКС: используем \b вместо Contains,
            // чтобы не находить oldName внутри других слов
            string checkPattern = $@"\b{Regex.Escape(oldName)}\b";
            if (!Regex.IsMatch(sourceCode, checkPattern))
            {
                return sourceCode;
            }

            string[] strings;
            sourceCode = ProtectStrings(sourceCode, out strings);

            string[] comments;
            sourceCode = ProtectComments(sourceCode, out comments);

            sourceCode = Regex.Replace(
                sourceCode,
                checkPattern,
                newName
            );

            // ФИКС: плейсхолдеры с __обёрткой__,
            // чтобы не пересекаться с именами переменных
            for (int i = 0; i < comments.Length; i++)
            {
                sourceCode = sourceCode.Replace(
                    $"__COMMENT{i}__",
                    comments[i]
                );
            }

            for (int i = 0; i < strings.Length; i++)
            {
                sourceCode = sourceCode.Replace(
                    $"__STRING{i}__",
                    strings[i]
                );
            }

            return sourceCode;
        }

        private string ProtectStrings(
            string code,
            out string[] strings)
        {
            var matches =
                Regex.Matches(code, "\".*?\"");

            strings = new string[matches.Count];

            for (int i = 0; i < matches.Count; i++)
            {
                strings[i] = matches[i].Value;
            }

            // ФИКС: плейсхолдер __STRING0__ вместо STRING0
            for (int i = 0; i < strings.Length; i++)
            {
                code = code.Replace(
                    strings[i],
                    $"__STRING{i}__"
                );
            }

            return code;
        }

        private string ProtectComments(
            string code,
            out string[] comments)
        {
            var matches =
                Regex.Matches(code, @"//.*");

            comments = new string[matches.Count];

            for (int i = 0; i < matches.Count; i++)
            {
                comments[i] = matches[i].Value;
            }

            // ФИКС: плейсхолдер __COMMENT0__ вместо COMMENT0
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