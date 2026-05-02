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
            new RefactorParameter {Name = "OLd", Value = "oldName"},
            new RefactorParameter {Name = "New", Value = "newName"}
        };

        public string RenameMethod(string nameMethod, string newNameMethod, string code)
        {
            if (string.IsNullOrWhiteSpace(nameMethod) || string.IsNullOrWhiteSpace(newNameMethod)) { return code; }

            string patternForCall = $@"\b{nameMethod}\s*\(";
            code = Regex.Replace(code, patternForCall, newNameMethod + "(", RegexOptions.Multiline);

            string patternForAdvert = $@"\b(\w+\s+)+{nameMethod}\s*\(";
            code = Regex.Replace(code, patternForAdvert,
                match => match.Value.Replace(nameMethod, newNameMethod),
                RegexOptions.Multiline);

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