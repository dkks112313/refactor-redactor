using RefactoringApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp6
{
    internal class AdapterCondolidateConditionalController : RefactoringMethods
    {
        private readonly ConsolidateConditionalMethod _method = new ConsolidateConditionalMethod();

        public string Name => _method.GetName();

        public List<RefactorParameter> GetParameters()
        {
            return new List<RefactorParameter>();
        }

        public string Execute(string code, Dictionary<string, string> parameters)
        {
            return _method.Apply(code);
        }
    }
}
