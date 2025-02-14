﻿using UnityEngine;

public class BirdAI : BirdBase
{
    protected override void OnThink(float dt, BirdBehaviour birdBehaviour, Obstacle obstacle)
    {
        float[] inputs = new float[2];
        inputs[0] = (obstacle.transform.position - birdBehaviour.transform.position).x / 10.0f;
        inputs[1] = (obstacle.transform.position - birdBehaviour.transform.position).y / 10.0f;

        float[] outputs;
        outputs = brain.Synapsis(inputs);
        if (outputs[0] < 0.5f)
        {
            birdBehaviour.Flap();
        }

        if (Vector3.Distance(obstacle.transform.position, birdBehaviour.transform.position) <= 1.0f)
        {
            genome.fitness *= 2;
        }

        genome.fitness += (100.0f - Vector3.Distance(obstacle.transform.position, birdBehaviour.transform.position));
    }

    protected override void OnDead()
    {
        base.OnDead();
    }

    protected override void OnReset()
    {
        genome.fitness = 0.0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Tube")
        {
            OnDead();
            state = State.Dead;
        }
    }
}
