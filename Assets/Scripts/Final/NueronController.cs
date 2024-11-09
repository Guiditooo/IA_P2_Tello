using System;

public class NueronController
{
    private uint[] neurons;

    private uint[] AddValueToArray(uint[] entities, uint newValue)
    {
        uint[] originalArray = entities;
        uint[] newArray = new uint[originalArray.Length + 1];

        Array.Copy(originalArray, newArray, originalArray.Length);

        newArray[newArray.Length - 1] = newValue;

        return newArray;
    }
}
