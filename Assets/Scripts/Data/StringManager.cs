
using System.Globalization;

public static class StringManager
{
    public static string ToTitleCase(this string title)
    {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower());
    }
}
