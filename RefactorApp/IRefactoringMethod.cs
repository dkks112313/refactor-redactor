using System;

namespace RefactoringApp
{
    public interface IRefactoringMethod
    {
        string GetName();
        string Apply(string sourceCode);
    }
}
