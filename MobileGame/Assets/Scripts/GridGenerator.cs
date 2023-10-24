using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject TileGO;

    public List<GameObject> Tiles = new List<GameObject>();

    public int Rows;
    public int Columns;

    public float Origin_X;
    public float Origin_Y;

    public float Offset_X;
    public float Offset_Y;

    public static GridGenerator Grid;

    internal TileScript ActiveTile;
    internal bool bAboutToPlant;

    [System.Serializable]
    public struct STileStruct
    {
        public int X;
        public int Y;

        public bool bPlantOnMe;
    }

    private void Start()
    {
        Grid = this;

        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                GameObject GO = Instantiate(TileGO, new Vector2(Origin_X + (Offset_X * i), Origin_Y + (Offset_Y * j)), Quaternion.identity);

                STileStruct T = new STileStruct();
                T.X = i + 1;
                T.Y = j + 1;

                GO.GetComponent<TileScript>().MyTile = T;

                Tiles.Add(GO);
            }
        }
    }

    internal void SetCurrentTile(TileScript T = null)
    {
        if (T)
        {
            ActiveTile = T;
        }
        else
        {
            ActiveTile = null;
        }
    }
}
