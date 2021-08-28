namespace CognizantChallenge.Compilers
{
    public interface ICompiler
    {
        public CompileResponse Run(string script, string language);
    }
}