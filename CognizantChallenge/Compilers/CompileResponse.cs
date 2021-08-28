namespace CognizantChallenge.Compilers
{
    public class CompileResponse
    {
        public int StatusCode { get; set; }
        public string Output { get; set; }
        public double? Memory { get; set; }
        public double? CpuTime { get; set; }
    }
}