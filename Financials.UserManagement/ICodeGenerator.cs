namespace Financials.UserManagement
{
    public interface ICodeGenerator
    {
        string Generate(int byteSize = 16);
    }
}