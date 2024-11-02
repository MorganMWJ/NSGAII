using NSGAII.Delegates;

namespace NSGAII.TestObjectiveFunctions;
public interface ITestProblem
{
    /// <summary>
    /// Lower variable bound 
    /// (smallest possible param)
    /// </summary>
    public double LowerBound { get; }

    /// <summary>
    /// Upper variable bound
    /// (largest possible param)
    /// </summary>
    public double UpperBound { get; }

    /// <summary>
    /// Parameter count. 
    /// Length of double array passed to F1, F2.
    /// </summary>
    public int ParamCount { get; }

    /// <summary>
    /// List of function pointers (delegates)
    /// Should contain objective functions F1 & F2
    /// </summary>
    public List<ObjectiveFunction> ObjectiveFunctions { get; }

    double F1(double[] X);

    double F2(double[] X);
}
