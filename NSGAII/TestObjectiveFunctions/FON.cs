using NSGAII.Delegates;
using System.Numerics;

namespace NSGAII.TestObjectiveFunctions;

/// Using well known test search problem
/// "Fonseca and Fleming’s study" (FON)
/// 
/// Minimise Object Functions
///   f1(X) = 1 - exp ( - sum3,i=1( (x - 1/rt(3))^2) ) 
///   f2(X) = 1 - exp ( - sum3,i=1( (x + 1/rt(3))^2) ) 
/// 
/// Expanded as:
/// 
///   f1(x1, x2, x3) = 1 - exp( (x1 - 1/rt(3))^2  + (x2 - 1/rt(3))^2  +  (x3 - 1/rt(3))^2)
///   f2(x1, x2, x3) = 1 - exp( (x1 + 1/rt(3))^2  + (x2 + 1/rt(3))^2  +  (x3 + 1/rt(3))^2)
/// 
/// Optimal soltuions:
///   
///   x1 == x2 == x3
///   E [-1/rt(3), 1/rt(3)]
public class FON : ITestProblem
{
    public const double MinValue = -4.0;
    public const double MaxValue = 4.0;
    public const int ParameterCount = 3;

    public double LowerBound => MinValue;

    public double UpperBound => MaxValue;

    public int ParamCount => ParameterCount;

    public List<ObjectiveFunction> ObjectiveFunctions => new List<ObjectiveFunction> { F1, F2 };

    /// <summary>
    /// Minimize objective function.
    /// Lowest value returned is best.
    /// </summary>
    public double F1(double x1, double x2, double x3)
    {
        var result = 1 - Math.Exp(-1 *
            (Math.Pow(x1 - 1 / Math.Sqrt(3), 2)
            + Math.Pow(x2 - 1 / Math.Sqrt(3), 2)
            + Math.Pow(x3 - 1 / Math.Sqrt(3), 2)));

        return result;
    }

    /// <summary>
    /// Adapter call
    /// </summary>
    public double F1(double[] X)
    {
        return F1(X[0], X[1], X[2]);
    }

    /// <summary>
    /// Minimize objective function.
    /// Lowest value returned is best.
    /// </summary>
    public double F2(double x1, double x2, double x3)
    {
        var result = 1 - Math.Exp(-1 *
            (Math.Pow(x1 + 1 / Math.Sqrt(3), 2)
            + Math.Pow(x2 + 1 / Math.Sqrt(3), 2)
            + Math.Pow(x3 + 1 / Math.Sqrt(3), 2)));

        return result;
    }

    /// <summary>
    /// Adapter call
    /// </summary>
    public double F2(double[] X)
    {
        return F1(X[0], X[1], X[2]);
    }
}
