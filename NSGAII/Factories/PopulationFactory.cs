namespace NSGAII.Factories;
public class PopulationFactory : IPopulationFactory
{
    private readonly ISolutionFactory _solutionFactory;
    private readonly NSGAIIOptions _options;

    public PopulationFactory(ISolutionFactory solutionFactory, NSGAIIOptions options)
    {
        _solutionFactory = solutionFactory;
        _options = options;
    }

    public Population CreateRandom()
    {
        var result = new Population(_options.PopulationSize);
        for (int i = 0; i < _options.PopulationSize; i++)
        {
            result.Members.Add(_solutionFactory.CreateRandom());
        }

        return result;
    }

    public Population CreateCombined(Population g1, Population g2)
    {
        var result = new Population(_options.PopulationSize * 2);
        result.Members.AddRange(g1.Members);
        result.Members.AddRange(g2.Members);
        return result;
    }

    public Population CreateOffspring(Population p)
    {
        var result = new Population(_options.PopulationSize);

        for (int x = 0; x < _options.PopulationSize; x++)
        {
            //selection
            var i1 = p.BinaryTournamentSelection();
            var i2 = p.BinaryTournamentSelection();

            // crossover
            var child = _solutionFactory.CreateChildUsingCrossover(i1, i2);

            // muation
            child.Mutate();

            result.Members.Add(child);
        }

        return result;
    }
}
