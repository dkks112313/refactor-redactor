using RefactoringApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RefactoringTool
{
    public class MagicNumberRefactoringController : RefactoringMethods
    {
        public string Name => "Magic Number Method";

        public List<RefactorParameter> GetParameters() => new List<RefactorParameter>()
        {
            new RefactorParameter {Name = "Name of constant", Value = "nameOfConstant"},
            new RefactorParameter {Name = "Value", Value = "value"}
        };

        public string Execute(string code, Dictionary<string, string> parameters)
        {
            return ReplaceMagicNumber(
                code,
                parameters["nameOfConstant"],
                parameters["value"]
            );
        }

        public string ReplaceMagicNumber(string sourceCode, string nameOfConstant, string number)
        {
            string type = ExtractTypeFromCode(sourceCode, number) ?? InferTypeFromLiteral(number);
            string constantDeclaration = $"const {type} {nameOfConstant} = {number};";

            string escapedNumber = Regex.Escape(number);
            string pattern = $@"(?<![.\d]){escapedNumber}(?![.\d])";
            string replacedCode = Regex.Replace(sourceCode, pattern, nameOfConstant);

            var lastIncludeMatch = Regex.Match(replacedCode, @"(#include\s*[<""][^\n]*\n)(?!.*#include)", RegexOptions.Singleline);
            if (lastIncludeMatch.Success)
            {
                int insertPos = lastIncludeMatch.Index + lastIncludeMatch.Length;
                replacedCode = replacedCode.Insert(insertPos, constantDeclaration + "\n");
            }
            else
            {
                replacedCode = $"{constantDeclaration}\n{replacedCode}";
            }

            return replacedCode;
        }

        private string ExtractTypeFromCode(string sourceCode, string number)
        {
            string escapedNumber = Regex.Escape(number);

            var patterns = new[]
            {
                $@"(\b[\w:]+(?:\s*\*+|\s*&+)?)\s+\w+\s*=\s*{escapedNumber}\b",
                $@"\(\s*([\w:]+(?:\s*\*+)?)\s*\)\s*{escapedNumber}\b",
            };

            foreach (var pattern in patterns)
            {
                var match = Regex.Match(sourceCode, pattern);
                if (match.Success)
                {
                    string candidate = match.Groups[1].Value.Trim();
                    if (!IsKeyword(candidate))
                        return candidate;
                }
            }

            return null;
        }

        private string InferTypeFromLiteral(string number)
        {
            if (MatchesSuffix(number, "ull") || MatchesSuffix(number, "ULL")) return "unsigned long long";
            if (MatchesSuffix(number, "ul") || MatchesSuffix(number, "UL")) return "unsigned long";
            if (MatchesSuffix(number, "ll") || MatchesSuffix(number, "LL")) return "long long";
            if (MatchesSuffix(number, "u") || MatchesSuffix(number, "U")) return "unsigned int";
            if (MatchesSuffix(number, "l") || MatchesSuffix(number, "L")) return "long";
            if (MatchesSuffix(number, "f") || MatchesSuffix(number, "F")) return "float";
            if (MatchesSuffix(number, "d") || MatchesSuffix(number, "D")) return "double";

            if (Regex.IsMatch(number, @"^-?0[xX][0-9a-fA-F]+$")) return "int";

            if (number.Contains(".") || Regex.IsMatch(number, @"[eE][+-]?\d")) return "double";

            return "int";
        }

        private bool MatchesSuffix(string number, string suffix) =>
            number.EndsWith(suffix, StringComparison.Ordinal);

        private bool IsKeyword(string word)
        {
            var keywords = new[]
            {
                "return", "if", "else", "for", "while", "do", "switch",
                "case", "break", "continue", "const", "static", "inline",
                "virtual", "explicit", "extern", "register", "volatile"
            };
            return Array.Exists(keywords, k => k == word);
        }
    }
}
