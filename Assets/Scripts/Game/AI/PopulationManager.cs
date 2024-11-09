using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PopulationManager : MonoBehaviour
{
    public Drawer herbivoreDrawer;
    public Drawer carnivoreDrawer;
    public Drawer scavangerDrawer;
    public Drawer plantDrawer;
    public Drawer corpseDrawer;

    public int populationCount = 40; 
    public int plantCount = 50;

    public Vector3 SceneHalfExtents = new Vector3(20.0f, 0.0f, 20.0f); //?

    public float GenerationDuration = 20.0f; 
    public int IterationCount = 1;

    public int EliteCount = 4;
    public float MutationChance = 0.10f;
    public float MutationRate = 0.01f;

    public int InputsCount = 4;
    public int HiddenLayers = 1;
    public int OutputsCount = 2;
    public int NeuronsCountPerHL = 7;
    public float Bias = 1f;
    public float P = 0.5f;

    GeneticAlgorithm genAlg;

    private List<uint> popHerbivore = new List<uint>();
    private List<uint> popCarnivore = new List<uint>();
    private List<uint> popScavenger = new List<uint>();

    List<Genome> genomes = new List<Genome>();
    List<NeuralNetwork> brains = new List<NeuralNetwork>();

    private List<uint> plants = new List<uint>();
    private List<uint> corpse = new List<uint>();

    float accumTime = 0;
    bool isRunning = false;

    public int generation { get; private set; }

    public float bestFitness { get; private set; }

    public float avgFitness { get; private set; }

    public float worstFitness { get; private set; }

    private float getBestFitness()
    {
        float fitness = 0;
        foreach (Genome g in genomes)
        {
            if (fitness < g.fitness)
                fitness = g.fitness;
        }

        return fitness;
    }

    private float getAvgFitness()
    {
        float fitness = 0;
        foreach (Genome g in genomes)
        {
            fitness += g.fitness;
        }

        return fitness / genomes.Count;
    }

    private float getWorstFitness()
    {
        float fitness = float.MaxValue;
        foreach (Genome g in genomes)
        {
            if (fitness > g.fitness)
                fitness = g.fitness;
        }

        return fitness;
    }

    static PopulationManager instance = null;

    public static PopulationManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PopulationManager>();

            return instance;
        }
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
    }

    private void LateUpdate()
    {
        
    }

    public void StartSimulation()
    {
        // Create and confiugre the Genetic Algorithm
        genAlg = new GeneticAlgorithm(EliteCount, MutationChance, MutationRate);

        //GenerateInitialPopulation();
        //CreateMines();

        isRunning = true;
    }

    public void PauseSimulation()
    {
        isRunning = !isRunning;
    }

    /*
    public void StopSimulation()
    {
        isRunning = false;

        generation = 0;

        // Destroy previous tanks (if there are any)
        DestroyTanks();

        // Destroy all mines
        DestroyMines();
    }
    */
    // Generate the random initial population
    void GenerateInitialPopulation()
    {
        generation = 0;

        // Destroy previous tanks (if there are any)
        //DestroyTanks();

        for (int i = 0; i < populationCount; i++)
        {
            NeuralNetwork brain = CreateBrain();

            Genome genome = new Genome(brain.GetTotalWeightsCount());

            brain.SetWeights(genome.genome);
            brains.Add(brain);

            genomes.Add(genome);

            popHerbivore.Add(CreateAnimal(genome, brain, AnimalType.Herbivore));

            populationGOs.Add(CreateTank(genome, brain));
        }

        accumTime = 0.0f;
    }
    
    // Creates a new NeuralNetwork
    NeuralNetwork CreateBrain()
    {
        NeuralNetwork brain = new NeuralNetwork();

        // Add first neuron layer that has as many neurons as inputs
        brain.AddFirstNeuronLayer(InputsCount, Bias, P);

        for (int i = 0; i < HiddenLayers; i++)
        {
            // Add each hidden layer with custom neurons count
            brain.AddNeuronLayer(NeuronsCountPerHL, Bias, P);
        }

        // Add the output layer with as many neurons as outputs
        brain.AddNeuronLayer(OutputsCount, Bias, P);

        return brain;
    }
    */
    /*
    // Evolve!!!
    void Epoch()
    {
        // Increment generation counter
        generation++;

        // Calculate best, average and worst fitness
        bestFitness = getBestFitness();
        avgFitness = getAvgFitness();
        worstFitness = getWorstFitness();

        // Evolve each genome and create a new array of genomes
        Genome[] newGenomes = genAlg.Epoch(genomes.ToArray());

        // Clear current population
        genomes.Clear();

        // Add new population
        genomes.AddRange(newGenomes);

        // Set the new genomes as each NeuralNetwork weights
        for (int i = 0; i < populationCount; i++)
        {
            NeuralNetwork brain = brains[i];

            brain.SetWeights(newGenomes[i].genome);

            populationGOs[i].SetBrain(newGenomes[i], brain);
            populationGOs[i].transform.position = GetRandomPos();
            populationGOs[i].transform.rotation = GetRandomRot();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isRunning)
            return;

        float dt = Time.fixedDeltaTime;

        for (int i = 0; i < Mathf.Clamp((float)(IterationCount / 100.0f) * 50, 1, 50); i++)
        {
            foreach (Tank t in populationGOs)
            {
                // Get the nearest mine
                GameObject mine = GetNearestMine(t.transform.position);

                // Set the nearest mine to current tank
                t.SetNearestMine(mine);

                mine = GetNearestGoodMine(t.transform.position);

                // Set the nearest mine to current tank
                t.SetGoodNearestMine(mine);

                mine = GetNearestBadMine(t.transform.position);

                // Set the nearest mine to current tank
                t.SetBadNearestMine(mine);

                // Think!! 
                t.Think(dt);

                // Just adjust tank position when reaching world extents
                Vector3 pos = t.transform.position;
                if (pos.x > SceneHalfExtents.x)
                    pos.x -= SceneHalfExtents.x * 2;
                else if (pos.x < -SceneHalfExtents.x)
                    pos.x += SceneHalfExtents.x * 2;

                if (pos.z > SceneHalfExtents.z)
                    pos.z -= SceneHalfExtents.z * 2;
                else if (pos.z < -SceneHalfExtents.z)
                    pos.z += SceneHalfExtents.z * 2;

                // Set tank position
                t.transform.position = pos;
            }

            // Check the time to evolve
            accumTime += dt;
            if (accumTime >= GenerationDuration)
            {
                accumTime -= GenerationDuration;
                Epoch();
                break;
            }
        }
    }

    #region Helpers
    */
    uint CreateAnimal(Genome genome, NeuralNetwork brain, AnimalType type)  
    {
        GameObject prefab;

        switch (type)
        {
            case AnimalType.Herbivore:
                break;
            case AnimalType.Carnivore:
                break;
            case AnimalType.Scavanger:
                break;
            default:
                break;
        }

        //GameObject go = Instantiate<GameObject>(prefab, position, GetRandomRot());
        //AnimalBase t = go.GetComponent<AnimalBase>();

        uint newAnimal = ECSManager.CreateEntity();

        //t.SetBrain(genome, brain);
        //return t;
        return 1;
    }
    /*
    void DestroyMines()
    {
        foreach (GameObject go in mines)
            Destroy(go);

        mines.Clear();
        goodMines.Clear();
        badMines.Clear();
    }

    void DestroyTanks()
    {
        foreach (Tank go in populationGOs)
            Destroy(go.gameObject);

        populationGOs.Clear();
        genomes.Clear();
        brains.Clear();
    }

    void CreateMines()
    {
        // Destroy previous created mines
        DestroyMines();

        for (int i = 0; i < MinesCount; i++)
        {
            Vector3 position = GetRandomPos();
            GameObject go = Instantiate<GameObject>(MinePrefab, position, Quaternion.identity);

            bool good = UnityEngine.Random.Range(-1.0f, 1.0f) >= 0;

            SetMineGood(good, go);

            mines.Add(go);
        }
    }

    void SetMineGood(bool good, GameObject go)
    {
        if (good)
        {
            go.GetComponent<Renderer>().material.color = Color.green;
            goodMines.Add(go);
        }
        else
        {
            go.GetComponent<Renderer>().material.color = Color.red;
            badMines.Add(go);
        }

    }

    public void RelocateMine(GameObject mine)
    {
        if (goodMines.Contains(mine))
            goodMines.Remove(mine);
        else
            badMines.Remove(mine);

        bool good = UnityEngine.Random.Range(-1.0f, 1.0f) >= 0;

        SetMineGood(good, mine);

        mine.transform.position = GetRandomPos();
    }

    Vector3 GetRandomPos()
    {
        return new Vector3(UnityEngine.Random.value * SceneHalfExtents.x * 2.0f - SceneHalfExtents.x, 0.0f, UnityEngine.Random.value * SceneHalfExtents.z * 2.0f - SceneHalfExtents.z);
    }

    Quaternion GetRandomRot()
    {
        return Quaternion.AngleAxis(UnityEngine.Random.value * 360.0f, Vector3.up);
    }

    GameObject GetNearestMine(Vector3 pos)
    {
        GameObject nearest = mines[0];
        float distance = (pos - nearest.transform.position).sqrMagnitude;

        foreach (GameObject go in mines)
        {
            float newDist = (go.transform.position - pos).sqrMagnitude;
            if (newDist < distance)
            {
                nearest = go;
                distance = newDist;
            }
        }

        return nearest;
    }

    GameObject GetNearestGoodMine(Vector3 pos)
    {
        GameObject nearest = mines[0];
        float distance = (pos - nearest.transform.position).sqrMagnitude;

        foreach (GameObject go in goodMines)
        {
            float newDist = (go.transform.position - pos).sqrMagnitude;
            if (newDist < distance)
            {
                nearest = go;
                distance = newDist;
            }
        }

        return nearest;
    }

    GameObject GetNearestBadMine(Vector3 pos)
    {
        GameObject nearest = mines[0];
        float distance = (pos - nearest.transform.position).sqrMagnitude;

        foreach (GameObject go in badMines)
        {
            float newDist = (go.transform.position - pos).sqrMagnitude;
            if (newDist < distance)
            {
                nearest = go;
                distance = newDist;
            }
        }

        return nearest;
    }

    #endregion
    */
}
