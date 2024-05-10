namespace UrlShortener.Backend.Util;

public class NumberConverter
{
    public static string ConvertToSpace(
        long number,
        string space = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"
    )
    {
        var spaceSize = space.Length;
        string result = "";

        do
        {
            result = space[(int)(number % 20)] + result;
            number /= spaceSize;
        } while (number > 0);

        return result;
    }
}
