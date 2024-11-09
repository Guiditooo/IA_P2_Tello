public class NeuronLayerComponent : ECSComponent
{
    
    public Neuron[] neurons;
    public float[] outputs;
    public int totalWeights = 0;
    public int inputsCount = 0;
    public float bias = 1;
    public float p = 0.5f;
    
    public NeuronLayerComponent()
    {

    }


    
}
