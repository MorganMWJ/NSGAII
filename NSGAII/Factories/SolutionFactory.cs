using NSGAII.Helpers;
using NSGAII.TestObjectiveFunctions;

namespace NSGAII.Factories;
public class SolutionFactory : ISolutionFactory
{
    private readonly ITestProblem _testProblem;

    public SolutionFactory(ITestProblem testProblem)
    {
        _testProblem = testProblem;

        Solution.Min = _testProblem.LowerBound;
        Solution.Max = _testProblem.UpperBound;
        Solution.GeneCount = _testProblem.ParamCount;
        Solution.ObjectiveFunctions = _testProblem.ObjectiveFunctions;
    }

    public Solution CreateRandom()
    {
        var genes = new double[_testProblem.ParamCount];

        for (int i = 0; i < genes.Length; i++)
        {
            genes[i] = StaticHelpers.RandDouble(_testProblem.LowerBound, _testProblem.UpperBound);
        }

        return new Solution(genes);
    }

    /// <summary>
    /// Using BLX-α crossover algorithm
    /// create child from two parents.
    /// </summary>
    /// <returns></returns>
    public Solution CreateChildUsingCrossover(Solution parent1, Solution parent2, double alpha = 0.3)
    {
        Random random = new Random();
        double[] offspring = new double[_testProblem.ParamCount];

        for (int i = 0; i < _testProblem.ParamCount; i++)
        {
            // Get min and max of the i-th gene
            double min = Math.Min(parent1.Genes[i], parent2.Genes[i]);
            double max = Math.Max(parent1.Genes[i], parent2.Genes[i]);

            // Calculate the extended range for the BLX-alpha crossover
            double range = max - min;
            double lowerBound = min - range * alpha;
            double upperBound = max + range * alpha;

            // Generate a random value within the extended range for the offspring gene
            offspring[i] = random.NextDouble() * (upperBound - lowerBound) + lowerBound;

            if (offspring[i] < _testProblem.LowerBound || offspring[i] > _testProblem.UpperBound)
                throw new InvalidOperationException("Error: offspring outside constrained range.");
        }

        var result = new Solution(offspring);
        return result;
    }
}
