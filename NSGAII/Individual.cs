
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
public class Individual
{
    public double[] Genes { get; set; }

    public int GeneCount => Genes.Length;

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
    public List<Individual> DominatedSolutions { get; set; } = new List<Individual>();

    /// <summary>
    /// Space of permiter to nearest 2 neighbours.
    /// Greater crowding distance means more diverse/different solution.
    /// </summary>
    public double CrowdingDist { get; internal set; } = 0;

    public Individual(double[] genes)
    {
        Genes = genes;
    }

    public Individual(int geneCount)
    {
        Genes = new double[geneCount];

        for(int i=0; i<Genes.Length; i++)
        {
            Genes[i] = StaticHelpers.RandDouble(NSGAII.MinValue, NSGAII.MaxValue);
        }
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
                Genes[i] = Math.Max(NSGAII.MinValue, Math.Min(NSGAII.MaxValue, mutation));
            }
        }
    }

    /// <summary>
    /// A solution dominates another if it is "no worse" in all objectives and "better" in at least one objective.
    /// True if this solution dominates the other solution.
    /// </summary>
    public bool Dominates(Individual other)
    {        
        // if worse in any object -> doesn't dominate
        if (NSGAII.ObjectiveFunctions.Any(objFunc => objFunc(Genes) > objFunc(other.Genes)))
        {
            return false;
        }

        // if equal in all objective functions -> doesn't dominate
        if (NSGAII.ObjectiveFunctions.All(objFunc => objFunc(Genes) == objFunc(other.Genes)))
        {
            return false;
        }

        // means this must be better in at least one
        return true;
    }

    /// <summary>
    /// Using BLX-α crossover algorithm
    /// </summary>
    /// <returns></returns>
    public static Individual Crossover(Individual parent1, Individual parent2, double alpha = 0.3)
    {       
        Random random = new Random();
        double[] offspring = new double[parent1.GeneCount];

        for (int i = 0; i < parent1.GeneCount; i++)
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

            if (offspring[i] < NSGAII.MinValue || offspring[i] > NSGAII.MaxValue)
                throw new InvalidOperationException("Error: offspring outside constrained range [-4,4]");
        }

        var result = new Individual(offspring);
        return result;
    }

    /// <summary>
    /// Override because using HashSet collection
    /// </summary>
    public override bool Equals(object obj)
    {
        if (obj is Individual other)
        {
            return Genes == other.Genes;
        }
        return false;
    }

    /// <summary>
    /// Override because using HashSet collection
    /// </summary>
    public override int GetHashCode()
    {
        return HashCode.Combine(Genes);
    }
}