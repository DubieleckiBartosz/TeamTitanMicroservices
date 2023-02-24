namespace Identity.Application.Utils;

public static class TemplateUtils
{
    public static string GetTemplateReplaceData(this string template, Dictionary<string, string> dictData)
    {
        if (template == null)
        {
            throw new ArgumentNullException(nameof(template));
        }

        foreach (var item in dictData.Keys)
        {
            template = template.Replace("{" + item + "}", dictData[item]);
        }

        return template;
    }
}