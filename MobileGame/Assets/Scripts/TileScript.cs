using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    [SerializeField]
    public GridGenerator.STileStruct MyTile;

    internal void ResetColor()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

    private void OnMouseEnter()
    {
        GridGenerator.Grid.SetCurrentTile(this);

        if (GridGenerator.Grid.bAboutToPlant)
        {
            if (MyTile.bPlantOnMe)
            {
                GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                GetComponent<SpriteRenderer>().color = Color.green;
            }
        }
    }

    private void OnMouseExit()
    {
        GridGenerator.Grid.SetCurrentTile(null);
        ResetColor();
    }
}
