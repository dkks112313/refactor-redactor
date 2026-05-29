using System;

namespace RefactoringEditor.Core
{
    public interface IRefactoringMethod
    {
        string GetName();
        string Apply(string sourceCode);
    }
}
