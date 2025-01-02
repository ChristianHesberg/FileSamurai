using System.Web;
using Ganss.Xss;

namespace application.transformers;

public class InputSanitizer
{
    public static string Sanitize(string input)
    {
        var sanitizer = new HtmlSanitizer();
        string safeOutput = sanitizer.Sanitize(input);  
        string encodedOutput = HttpUtility.HtmlEncode(safeOutput);

        return encodedOutput;
    }
}