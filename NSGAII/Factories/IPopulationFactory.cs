using NSGAII.TestObjectiveFunctions;

namespace NSGAII.Factories;

public interface IPopulationFactory
{
    /// <summary>
    /// Used to set up the first generation with random values.
    /// </summary>
    public Population CreateRandom();

    /// <summary>
    /// Create combined population of 2 * Size.
    /// As part of temporary step in main loop.
    /// </summary>
    public Population CreateCombined(Population g1, Population g2);

    /// <summary>
    /// Create the next generation of the same size by
    /// selection, crossover, and mutation.
    /// </summary>
    /// <returns>New Population Q</returns>
    public Population CreateOffspring(Population p);
}