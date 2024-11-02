using NSGAII.Delegates;
using NSGAII.Extensions;
using NSGAII.Helpers;
using System.Collections.ObjectModel;
using System.Drawing;

namespace NSGAII;
public class Population
{
    public FixedSizeList<Solution> Members {  get; set; }

    public bool IsFull => Members.IsFull;

    /// <summary>
    /// Create empty generation
    /// </summary>
    public Population(int size)
    {
        Members = new FixedSizeList<Solution>(size);
    }


    /// <summary>
    /// Fitness calculated from Non-dominated rank and crowding distance.
    /// </summary>
    /// <returns></returns>
    public Solution BinaryTournamentSelection()
    {
        var i1 = Members.Rand();
        var i2 = Members.Rand();
        
        if(i1.NonDominationRank < i2.NonDominationRank)
        {
            return i1;
        }
        else if(i1.NonDominationRank > i2.NonDominationRank)
        {
            return i2;
        }
        else
        {
            return i1.CrowdingDist > i2.CrowdingDist ? i1 : i2;
        }
    }

    /// <summary>
    /// Calculate the non-dominated fronts
    /// 
    /// For each solution (population member) we calculate 2 entities:
    ///    1. Domination count np, the number of solutions which dominate this solution(p)
    ///    2. Set of solutions(Sp )that this solution (p) dominates
    ///    
    /// This calculation has O(MN2) complexity  [M is number of objective/fitness functions]
    /// (Non-dominated front is a set of solutions that do not dominate each other.)
    /// The first non-dominated front will contain the solutions with np =0
    /// We go to each member (p) of the first non-dominated front and we iterate through each of their dominated solutions(Sp) reducing each of the dominated solutions(q) domination count by one.If the domination count becomes 0 then we put it in a separate list Q. This list forms the second non-dominated front.
    /// The above procedure is repeated until the third front is identified and then until all fronts are identified
    /// </summary>
    public Collection<List<Solution>> NonDominationSort()
    {
        // Create the first non-dominated front
        var front1 = new List<Solution>();
        foreach (var p in Members)
        {
            p.DominatedSolutions = new List<Solution>();
            p.DominationCount = 0;

            foreach (var q in Members)
            {
                if (p.Dominates(q))
                {
                    p.DominatedSolutions.Add(q);
                }
                else if(q.Dominates(p))
                {
                    p.DominationCount++;
                }
            }

            // if p has no solutions that dominate it
            // it belongs to the first non-dominated front
            if (p.DominationCount == 0)
            {
                p.NonDominationRank = 1;
                front1.Add(p);
            }
        }

        // create the other non-dominated fronts
        int frontCounter = 1;
        var fronts = new Collection<List<Solution>>();

        while (fronts[frontCounter].Count != 0)
        {
            var newFront = new List<Solution>();

            foreach (var p in front1)
            {
                foreach (var q in p.DominatedSolutions)
                {
                    q.DominationCount--;
                    if (q.DominationCount == 0)
                    {
                        q.NonDominationRank = frontCounter + 1;
                        newFront.Add(q);
                    }
                }
            }

            frontCounter++;
            fronts.Add(newFront);
        }

        // not needed as properies set on individual solutions
        // but returning it anyway
        return fronts;
    }

    /// <summary>
    /// Calculate crowding distance for each solution in a non-dominated front.
    /// 
    /// The crowding-distance computation requires sorting the population 
    /// according to each objective function value in ascending order of magnitude.
    /// 
    /// Thereafter, for each objective function, the boundary solutions(solutions with smallest and largest function
    /// values) are assigned an infinite distance value.All other intermediate solutions are 
    /// assigned a distance value equal to the absolute normalized difference in the function values
    /// of two adjacent solutions.This calculation is continued with other objective functions.
    /// 
    /// The overall crowding-distance value is calculated as the sum of individual distance values corresponding to each objective.
    /// Each objective function is normalized before calculating the crowding distance.
    /// </summary>
    /// <param name="nonDominatedFront"></param>
    public static void CrowdingDistanceAssignment(List<Solution> nonDominatedFront)
    {
        foreach(var objFunc in Solution.ObjectiveFunctions)
        {
            var sortedFront = nonDominatedFront.OrderBy(x => objFunc(x.Genes)).ToList();

            // boundaries are always selected because they are automatically the most diverse
            // do this my setting there crowding distance to infinity
            sortedFront[0].CrowdingDist = double.PositiveInfinity;
            sortedFront[^1].CrowdingDist = double.PositiveInfinity;

            // Calculate the range of the current objective
            double minObjective = objFunc(sortedFront.First().Genes);
            double maxObjective = objFunc(sortedFront.Last().Genes);
            double range = maxObjective - minObjective;

            // assign crowding distance for all other solutions (excluding first and last)
            for (int i = 1; i<sortedFront.Count-2; i++)
            {
                double distance = (objFunc(sortedFront[i + 1].Genes) - objFunc(sortedFront[i - 1].Genes)) / range; //mean normalisation using the range

                sortedFront[i].CrowdingDist += distance;
            }
        }
    }
}
