using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using RefactoringApp;

namespace RefactoringApp
{
    public class RefactorRenameMethodController : RefactoringMethods
    {
        public string Name => "Rename Method";

        public List<RefactorParameter> GetParameters() => new List<RefactorParameter>()
        {
            new RefactorParameter {Name = "Old Name", Value = "oldName"},
            new RefactorParameter {Name = "New Name", Value = "newName"}
        };

        public string RenameMethod(string nameMethod, string newNameMethod, string code)
        {
            if (string.IsNullOrWhiteSpace(nameMethod) ||
                string.IsNullOrWhiteSpace(newNameMethod))
            {
                return "Error: empty parameter";
            }

            if (newNameMethod.Contains(" "))
            {
                return "Error: spaces";
            }

            if (char.IsDigit(newNameMethod[0]))
            {
                return "Error: starts with digit";
            }

            if (!Regex.IsMatch(newNameMethod, @"^[a-zA-Z_][a-zA-Z0-9_]*$"))
            {
                return "Error: invalid characters";
            }

            if (string.IsNullOrEmpty(code))
            {
                code = nameMethod;
            }

            var keywords = new HashSet<string>
            {
                "if", "else", "for", "while", "switch",
                "case", "return", "class", "struct",
                "public", "private", "protected",
                "void", "int", "float", "double",
                "char", "bool", "namespace"
            };

            if (keywords.Contains(nameMethod))
            {
                return code;
            }

            if (!code.Contains(nameMethod))
            {
                return "Error: method name not found";
            }

            string escapedName = Regex.Escape(nameMethod);

            string patternForDeclaration =
            $@"(?<=\b(?:void|int|float|double|string|bool|char)\s+){escapedName}(?=\s*\()";

            code = Regex.Replace(
                code,
                patternForDeclaration,
                newNameMethod
            );

            string patternForCall =
            $@"(?<!\b(if|for|while|switch)\s*)\b{escapedName}(?=\s*\()";

            code = Regex.Replace(
                code,
                patternForCall,
                newNameMethod
            );

            return code;
        }

        public string Execute(string code, Dictionary<string, string> parameters)
        {
            return RenameMethod(
                parameters["oldName"],
                parameters["newName"],
                code
            );
        }
    }
}