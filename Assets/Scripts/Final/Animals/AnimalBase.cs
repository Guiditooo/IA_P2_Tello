public enum AnimalType
{
    Herbivore,
    Carnivore,
    Scavanger
}

public abstract class AnimalBase
{

    protected Genome genome;
    protected NeuralNetwork brain;

    protected float fitness;

    protected float[] inputs;

    public void SetBrain(Genome genome, NeuralNetwork brain)
    {
        this.genome = genome;
        this.brain = brain;
        inputs = new float[brain.InputsCount];
        OnReset();
    }


    protected void Move(float dt)
    {

    }

    public void Think(float dt)
    {
        OnThink(dt);

    }

    protected virtual void OnThink(float dt)
    {

    }

    protected virtual void OnTakeFood(uint food)
    {

    }

    protected virtual void OnReset()
    {

    }

}
