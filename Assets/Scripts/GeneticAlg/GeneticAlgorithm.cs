using System.Collections.Generic;
using System;

public class Genome
{
    public float[] genome;
    public float fitness = 0;

    public Genome(float[] genes)
    {
        this.genome = genes;
        fitness = 0;
    }

    public Genome(int genesCount)
    {
        genome = new float[genesCount];

        for (int j = 0; j < genesCount; j++)
            genome[j] = (float)(new Random().NextDouble() * 2.0f - 1.0f); //[-1 y 1]

        fitness = 0;
    }

    public Genome()
    {
        fitness = 0;
    }

}

public class GeneticAlgorithm
{
    List<Genome> population = new List<Genome>();
    List<Genome> newPopulation = new List<Genome>();

    float totalFitness;

    int eliteCount = 0;
    float mutationChance = 0.0f;
    float mutationRate = 0.0f;

    Random rand = null;
    public GeneticAlgorithm(int eliteCount, float mutationChance, float mutationRate)
    {
        this.eliteCount = eliteCount;
        this.mutationChance = mutationChance;
        this.mutationRate = mutationRate;

        rand = new Random();
    }

    public Genome[] GetRandomGenomes(int count, int genesCount)
    {
        Genome[] genomes = new Genome[count];

        for (int i = 0; i < count; i++)
        {
            genomes[i] = new Genome(genesCount);
        }

        return genomes;
    }


    public Genome[] Epoch(Genome[] oldGenomes)
    {
        totalFitness = 0;

        population.Clear();
        newPopulation.Clear();

        population.AddRange(oldGenomes);
        population.Sort(HandleComparison);

        foreach (Genome g in population)
        {
            totalFitness += g.fitness;
        }

        SelectElite();

        while (newPopulation.Count < population.Count)
        {
            Crossover();
        }

        return newPopulation.ToArray();
    }

    void SelectElite()
    {
        for (int i = 0; i < eliteCount && newPopulation.Count < population.Count; i++)
        {
            newPopulation.Add(population[i]);
        }
    }

    void Crossover()
    {
        Genome mom = RouletteSelection();
        Genome dad = RouletteSelection();

        Genome child1;
        Genome child2;

        Crossover(mom, dad, out child1, out child2);

        newPopulation.Add(child1);
        newPopulation.Add(child2);
    }

    void Crossover(Genome mom, Genome dad, out Genome child1, out Genome child2)
    {
        child1 = new Genome();
        child2 = new Genome();

        child1.genome = new float[mom.genome.Length];
        child2.genome = new float[mom.genome.Length];

        int pivot = rand.Next(0, mom.genome.Length);

        for (int i = 0; i < pivot; i++)
        {
            child1.genome[i] = mom.genome[i];

            if (ShouldMutate())
                child1.genome[i] += GetRandomFloatBetween(-mutationRate, mutationRate);

            child2.genome[i] = dad.genome[i];

            if (ShouldMutate())
                child2.genome[i] += GetRandomFloatBetween(-mutationRate, mutationRate);
        }

        for (int i = pivot; i < mom.genome.Length; i++)
        {
            child2.genome[i] = mom.genome[i];

            if (ShouldMutate())
                child2.genome[i] += GetRandomFloatBetween(-mutationRate, mutationRate);

            child1.genome[i] = dad.genome[i];

            if (ShouldMutate())
                child1.genome[i] += GetRandomFloatBetween(-mutationRate, mutationRate);
        }
    }

    bool ShouldMutate()
    {
        return (float)rand.NextDouble() < mutationChance;
    }

    int HandleComparison(Genome x, Genome y)
    {
        return x.fitness > y.fitness ? 1 : x.fitness < y.fitness ? -1 : 0;
    }


    public Genome RouletteSelection()
    {
        float rnd = GetRandomFloatBetween(0, Math.Max(totalFitness, 0));

        float fitness = 0;

        for (int i = 0; i < population.Count; i++)
        {
            fitness += Math.Max(population[i].fitness, 0);
            if (fitness >= rnd)
                return population[i];
        }

        return null;
    }

    private float GetRandomFloatBetween(float a, float b)
    {
        float min = Math.Min(a, b);
        float max = Math.Max(a, b);
        return (float)(min + (rand.NextDouble() * (max - min)));
    }
}
