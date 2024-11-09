using UnityEngine;

public class Tank : TankBase
{
    float fitness = 0;
    int mineCount = 0;
    int goodMineCount = 0;

    protected override void OnReset()
    {
        fitness = 1;
    }

    protected override void OnThink(float dt)
    {
        Vector3 dirToMine = GetDirToMine(nearMine);

        inputs[0] = dirToMine.x;
        inputs[1] = dirToMine.z;
        inputs[2] = transform.forward.x;
        inputs[3] = transform.forward.z;
        inputs[4] = IsGoodMine(nearMine) ? 1 : 0;
        //inputs[5] = PopulationManager.Instance.GetNearestTank(nearMine.transform.position) ? 1 : 0;
        inputs[5] = 1;

        float[] output = brain.Synapsis(inputs);

        SetForces(output[0], output[1], dt);
    }

    protected override void OnTakeMine(GameObject mine)
    {
        mineCount++;
        if (IsGoodMine(mine))
            goodMineCount++;
        fitness = IsGoodMine(mine) ? fitness + 100 : fitness * 0.9f;
        genome.fitness = fitness;
    }

    void CalculateFitness()
    {
        if (goodMineCount > mineCount / 2)
        {
            fitness *= 2;
            Debug.Log("COMI MAS VERDES");
        }
        else
        {
            fitness *= 0.2f;
            Debug.Log("COMI MAS ROJAS");
        }
        genome.fitness = fitness;
    }
}
