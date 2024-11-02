namespace NSGAII.Factories;

public interface ISolutionFactory
{
    public Solution CreateRandom();

    public Solution CreateChildUsingCrossover(Solution parent1, Solution parent2, double alpha = 0.3);
}