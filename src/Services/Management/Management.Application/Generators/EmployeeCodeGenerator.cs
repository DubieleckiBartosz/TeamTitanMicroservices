namespace Management.Application.Generators;

public class EmployeeCodeGenerator
{ 
    public static string Generate(int codeCounter)
    {
        var codeNumber = Interlocked.Increment(ref codeCounter);
        var code = "EMP" + codeNumber.ToString("D6");
        return code;
    }
}