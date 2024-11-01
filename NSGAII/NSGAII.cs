using NSGAII.Delegates;
using NSGAII.TestObjectiveFunctions;

namespace NSGAII;
public class NSGAII
{
    public List<Generation> PopulationHistory {  get; set; } = new List<Generation>();
    public Generation? CurrentPopulation {  get; private set; }
    public Generation? NextPopulation {  get; private set; }

    private readonly ITestProblem _testProblem;
    private readonly NSGAIIOptions _options;

    public NSGAII(ITestProblem testProblem, NSGAIIOptions options)
    {
        _testProblem = testProblem;
        _options = options;
    }

    public void Run()
    {
        CurrentPopulation = Generation.CreateRandom(_options.PopulationSize, _testProblem.N);
        NextPopulation = CurrentPopulation.CreateNextGeneration();

        for (int i = 0; i < _options.Iterations; i++)
        {
            IterateGeneration();
        }
    }

    private void IterateGeneration()
    {
        throw new NotImplementedException();
    }
}
