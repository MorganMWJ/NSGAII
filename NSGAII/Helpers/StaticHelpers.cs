using System;

namespace NSGAII.Helpers;
internal static class StaticHelpers
{
    private static readonly Random random = new Random();

    public static double RandDouble(double min, double max)
    {
        return random.NextDouble() * (max - min) + min;
    }

    // Generates a Gaussian (normally distributed) random number
    public static double GaussianRandom(double mean, double stdDev)
    {
        // Box-Muller transform to generate normal distribution from uniform distribution
        double u1 = 1.0 - random.NextDouble();
        double u2 = 1.0 - random.NextDouble();
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        return mean + stdDev * randStdNormal;
    }
}
