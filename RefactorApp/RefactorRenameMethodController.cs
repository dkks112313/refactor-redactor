using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public string RenameMethod(string nameMethod, string newNameMethod, string _empty)
        {
            throw new NotImplementedException();
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