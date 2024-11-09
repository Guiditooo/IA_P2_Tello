using System.Collections.Generic;
using System.Threading.Tasks;

public class GridSystem : ECSSystem
{
    private ParallelOptions parallelOptions;

    private IDictionary<uint, GridComponent> gridComponents;
    private IEnumerable<uint> queryedEntities;

    private GridBoundsComponent gridBounds;
    private uint gridBoundsID;

    public override void Initialize()
    {
        parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 32 };

        gridBoundsID = ECSManager.GetEntitiesWhitComponentTypes(typeof(GridBoundsComponent)).GetEnumerator().Current;
        gridBounds = ECSManager.GetComponent<GridBoundsComponent>(gridBoundsID);
    }

    protected override void PreExecute(float deltaTime)
    {
        gridComponents ??= ECSManager.GetComponents<GridComponent>();
        queryedEntities ??= ECSManager.GetEntitiesWhitComponentTypes(typeof(GridComponent));
    }

    protected override void Execute(float deltaTime)
    {
        Parallel.ForEach(queryedEntities, parallelOptions, i =>
        {
            if (gridComponents[i].X > gridBounds.gridXMax)
                gridComponents[i].X -= gridBounds.gridXMax;
            
            if (gridComponents[i].Y > gridBounds.gridYMax)
                gridComponents[i].Y = gridBounds.gridYMax;
            else if (gridComponents[i].Y < 0)
                gridComponents[i].Y = 0;
        });
    }

    protected override void PostExecute(float deltaTime)
    {

    }
}
