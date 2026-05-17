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

            if (!sourceCode.Contains(oldName))
            {
                return "Error: variable not found";
            }

            string[] strings;
            sourceCode = ProtectStrings(sourceCode, out strings);

            string[] comments;
            sourceCode = ProtectComments(sourceCode, out comments);

            string pattern =
                $@"\b{Regex.Escape(oldName)}\b";

            sourceCode = Regex.Replace(
                sourceCode,
                pattern,
                newName
            );

            for (int i = 0; i < comments.Length; i++)
            {
                sourceCode = sourceCode.Replace(
                    $"COMMENT{i}",
                    comments[i]
                );
            }

            for (int i = 0; i < strings.Length; i++)
            {
                sourceCode = sourceCode.Replace(
                    $"STRING{i}",
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

            for (int i = 0; i < strings.Length; i++)
            {
                code = code.Replace(
                    strings[i],
                    $"STRING{i}"
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

            for (int i = 0; i < comments.Length; i++)
            {
                code = code.Replace(
                    comments[i],
                    $"COMMENT{i}"
                );
            }

            return code;
        }
    }
}