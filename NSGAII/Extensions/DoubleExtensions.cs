namespace NSGAII.Extensions;

public static class DoubleExtensions
{
    public static double Clamp(this double x, double minVal, double maxVal)
    {
        return Math.Max(minVal, Math.Min(maxVal, x));
    }
}