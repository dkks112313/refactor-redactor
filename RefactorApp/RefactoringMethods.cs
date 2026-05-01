using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactoringApp
{
    public interface RefactoringMethods
    {
        string Name { get; }
        List<RefactorParameter> GetParameters();
        string Execute(string code, Dictionary<string, string> parameters);
    }
}
