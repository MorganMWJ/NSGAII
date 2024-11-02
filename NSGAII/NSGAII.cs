using ConsoleTables;
using NSGAII.Factories;

namespace NSGAII;
public class NSGAII
{
    public List<Population> PopulationHistory { get; private set; }

    private Population? _currentPopulation;
    public Population? CurrentPopulation 
    {
        get => _currentPopulation;
        set
        {
            if(value != null)
            {
                _currentPopulation = value;
                PopulationHistory.Add(value);
            }            
        }
    }
    public Population? OffspringPopulation {  get; private set; }   

    private readonly NSGAIIOptions _options;
    private readonly IPopulationFactory _populationFactory;

    public NSGAII(NSGAIIOptions options, IPopulationFactory populationFactory)
    {
        _options = options;
        _populationFactory = populationFactory;
        PopulationHistory = new List<Population>();
    }

    public void Run()
    {
        // init P and Q
        CurrentPopulation = _populationFactory.CreateRandom();
        OffspringPopulation = _populationFactory.CreateOffspring(CurrentPopulation);

        CurrentPopulation.Print();

        for (int i = 0; i < _options.Iterations; i++)
        {
            // Rt = Pr U Qt - combine parent and offspring population
            var CombinedPopulation = _populationFactory.CreateCombined(CurrentPopulation, OffspringPopulation);

            // Sort by non-domination
            var nonDominatedFronts = CombinedPopulation.NonDominationSort();

            // Pt+1 = {}
            var nextPopulation = new Population(_options.PopulationSize);

            // front index/counter
            int fi = 0;

            // until the parent population is filled            
            while (nextPopulation.Members.Count + nonDominatedFronts[fi].Count < _options.PopulationSize)
            {
                // calculate crowding-distance in each front
                Population.CrowdingDistanceAssignment(nonDominatedFronts[fi]);

                // add entire front into population
                nextPopulation.Members.AddRange(nonDominatedFronts[fi]);

                fi++;
            }

            // order the front by most diverse using crowding distance
            var orderedFront = nonDominatedFronts[fi].OrderByDescending(s=>s.CrowdingDist);

            // fill the remaing spaces
            var spaces = _options.PopulationSize - nextPopulation.Members.Count;
            var solutionsToAdd = orderedFront.Take(spaces);
            nextPopulation.Members.AddRange(solutionsToAdd);

            // make new current & offspring population
            CurrentPopulation = nextPopulation;
            OffspringPopulation = _populationFactory.CreateOffspring(CurrentPopulation);

            CurrentPopulation.Print();
        }
    }
}
