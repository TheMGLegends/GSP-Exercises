using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericPlant : MonoBehaviour
{
    public bool bIsAwake;
    public GridGenerator.STileStruct AssociatedTile;
    

    protected virtual void Update()
    {
        if (bIsAwake)
        {
            PlantFunctionality();
        }
    }

    protected virtual void PlantFunctionality()
    {
        throw new NotImplementedException();
    }
}
