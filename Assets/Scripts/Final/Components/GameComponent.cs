using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameComponent : ECSComponent
{
    public int herbivoreCount;
    public int carnivoreCount;
    public int scavengerCount;

    public GameComponent(int herbivoreCount, int carnivoreCount, int scavengerCount)
    {
        this.herbivoreCount = herbivoreCount;
        this.carnivoreCount = carnivoreCount;
        this.scavengerCount = scavengerCount;
    }

}
