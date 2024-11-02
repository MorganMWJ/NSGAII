
using NSGAII.Delegates;
using NSGAII.Extensions;
using NSGAII.Helpers;
using System;
using System.Collections;
using System.Numerics;
using System.Xml.Linq;

namespace NSGAII;

/// <summary>
/// Geneotype is a collection of real values (doubles)
/// </summary>
public class Solution
{
    /// <summary>
    /// Each solution will have a different geneome (real values)
    /// </summary>
    public double[] Genes { get; set; }

    /// <summary>
    /// Every solution should be encoded in the same number of real values - ? maybe not for all test problems?
    /// </summary>
    public static int GeneCount { get; set; }

    /// <summary>
    /// Every solution value will have an upper variable bound
    /// </summary>
    public static double Max {  get; set; }

    /// <summary>
    /// Every solution value will have an lower variable bound
    /// </summary>
    public static double Min {  get; set; }

    /// <summary>
    /// Every solution will be tested against the same set of minimisation objective functions
    /// </summary>
    public static List<ObjectiveFunction> ObjectiveFunctions { get; set; } = new List<ObjectiveFunction>();

    /// <summary>
    /// The non-dominated front this solution belongs too.
    /// Example:
    ///   Solutions not dominated by any other will be in front 0
    ///   and therefore have NonDominationRank = 0
    /// </summary>
    public int NonDominationRank { get; set; }

    /// <summary>
    /// Number of solutions/individuals that dominate this solution/individual
    /// </summary>
    public int DominationCount { get; set; }

    /// <summary>
    /// Set of solutions that this solution dominates
    /// </summary>
    public List<Solution> DominatedSolutions { get; set; } = new List<Solution>();

    /// <summary>
    /// Space of permiter to nearest 2 neighbours.
    /// Greater crowding distance means more diverse/different solution.
    /// </summary>
    public double CrowdingDist { get; set; } = 0;

    public object GeneStr => string.Join(", ", Genes);

    public Solution(double[] genes)
    {
        Genes = genes;
    }

    /// <summary>
    /// Gaussian (Normal) Mutation - revise this!
    /// </summary>
    public void Mutate(double mutationRate = 0.3, double stdDev = 0.2)
    {
        Random random = new Random();
        for (int i = 0; i < GeneCount; i++)
        {
            // Apply mutation with a probability equal to the mutation rate
            if (random.NextDouble() < mutationRate)
            {
                // Apply Gaussian mutation
                double mutation = Genes[i] + StaticHelpers.GaussianRandom(0, stdDev);

                // Clamp mutation to within min and max values
                Genes[i] = mutation.Clamp(Min, Max);
            }
        }
    }

    /// <summary>
    /// A solution dominates another if it is "no worse" in all objectives and "better" in at least one objective.
    /// True if this solution dominates the other solution.
    /// </summary>
    public bool Dominates(Solution other)
    {        
        // if worse in any object -> doesn't dominate
        if (ObjectiveFunctions.Any(objFunc => objFunc(Genes) > objFunc(other.Genes)))
        {
            return false;
        }

        // if equal in all objective functions -> doesn't dominate
        if (ObjectiveFunctions.All(objFunc => objFunc(Genes) == objFunc(other.Genes)))
        {
            return false;
        }

        // means this must be better in at least one
        return true;
    }
}