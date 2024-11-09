using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBoundsComponent : ECSComponent
{
    public int gridXMax;
    public int gridYMax;

    public GridBoundsComponent(int gridXMax, int gridYMax)
    {
        this.gridXMax = gridXMax;
        this.gridYMax = gridYMax;
    }
}
