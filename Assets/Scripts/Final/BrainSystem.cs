using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class BrainSystem : ECSSystem
{
    private ParallelOptions parallelOptions;

    private IDictionary<uint, NeuronLayerComponent> neuronLayerComponents;
    private IEnumerable<uint> queryedEntities;

    public override void Initialize()
    {
        parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 32 };
    }

    protected override void PreExecute(float deltaTime)
    {
        neuronLayerComponents ??= ECSManager.GetComponents<NeuronLayerComponent>();
        queryedEntities ??= ECSManager.GetEntitiesWhitComponentTypes(typeof(NeuronLayerComponent));
    }

    protected override void Execute(float deltaTime)
    {
        
        //La ejecución de cada neurona en el sistema ECS, debe realizar la sinapsis:
        float[] outputs = null;

        for (int i = 0; i < neuronLayerComponents.Count; i++)
        {
            //outputs = neuronLayerComponents[i].Synapsis(inputs);
            //inputs = outputs;
        }

        //return outputs;
        
    }

    protected override void PostExecute(float deltaTime)
    {
        throw new System.NotImplementedException();
    }

}