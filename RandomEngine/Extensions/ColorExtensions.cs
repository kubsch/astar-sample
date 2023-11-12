namespace RandomEngine.Extensions;

public static class ColorExtensions
{
    public static Color With(this Color instance, int r = -1, int g = -1, int b = -1, int a = -1)
    {
        return Color.FromNonPremultiplied(r == -1 ? instance.R : r, g == -1 ? instance.G : g, b == -1 ? instance.B : b, a == -1 ? instance.A : a);
    }
}