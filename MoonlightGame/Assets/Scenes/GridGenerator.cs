using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Jobs;

public class GridGenerator : MonoBehaviour
{
    public GameObject GridPiece;
    public int GridWidth;
    public int GridHeight;
    public GameObject RefObj;
    public float Offset;
    public float Margin;
    public float TileSize;
    public float ScaleDownValue;

    public GameObject SelectionBoxPrefab;
    protected GameObject SelectionBox;
    public Sprite SelectionBoxSpr;

    protected int X = 0;
    protected int Y = 0;

    public GameObject[,] Tiles;

    protected void Start()
    {
        GenerateGrid();
        GenerateGameGrid();
        ScaleAndCenterGrid(GridHeight);
    }

    protected void ScaleAndCenterGrid(int gridHeight)
    {
        //1: Scale only if height is greater than 6
        int Amt = gridHeight - 6;

        //2: Get all child tiles
        List<GameObject> TilesToScale = new List<GameObject>();
        for (int i = 0; i < RefObj.transform.childCount; i++)
        {
            TilesToScale.Add(RefObj.transform.GetChild(i).gameObject);
        }

        //3: De-parenting
        foreach (var Item in TilesToScale) 
        { 
            Item.transform.parent = null;
        }

        //4: Find center of mass
        Vector3 VEC = Vector3.zero;
        foreach (var item in TilesToScale)
        {
            VEC += item.transform.position;
        }

        VEC /= TilesToScale.Count;

        //5: Move the now empty RefObj to that centroid
        RefObj.transform.position = VEC;

        //6: Re-parent them all!
        foreach (var item in TilesToScale)
        {
            item.transform.parent = RefObj.transform;
        }

        //7: Scale parent, meaning you scale all tiles!
        Vector3 ScaleVec = RefObj.transform.localScale;

        if (Amt > 0)
        {
            for (int i = 0; i < Amt; i++)
            {
                ScaleVec.x /= ScaleDownValue;
                ScaleVec.y /= ScaleDownValue;
                ScaleVec.z /= ScaleDownValue;
            }
        }
        RefObj.transform.localScale = ScaleVec;

        //8: Put the parent at center of camera.
        RefObj.transform.position = new Vector3(Camera.main.transform.position.x, 
            Camera.main.transform.position.y, RefObj.transform.position.z);
    }

    internal void RegenarateGrid()
    {
        print("Regen!");
        DestroyGrid();
        GenerateGrid();
        GenerateGameGrid();
        ScaleAndCenterGrid(GridHeight);
    }

    protected void DestroyGrid()
    {
        for (int i = 0; i < GridWidth; i++)
        {
            for (int j = 0; j < GridHeight; j++)
            {
                Destroy(Tiles[i, j]);
            }
        }
        Destroy(SelectionBox);
    }

    protected virtual void GenerateGameGrid()
    {
        //Empty
    }

    protected void GenerateGrid()
    {
        //Creation of the actual grid
        Tiles = new GameObject[GridWidth, GridHeight];

        Vector3 SpawnPoint = RefObj.transform.position;

        for (int i = 0; i < GridWidth; i++)
        {
            for (int j = 0; j < GridHeight; j++)
            {
                GameObject GO = Instantiate(GridPiece, SpawnPoint, Quaternion.identity);
                GO.transform.SetParent(RefObj.transform);

                GO.name = "TILE [" + i + ", " + j + "]";

                GO.transform.Translate(new Vector3(
                    (i * Offset + i * Margin), 
                    (j * Offset + j * Margin), 
                    0), Space.Self);

                GO.transform.localScale = new Vector3(TileSize, TileSize, TileSize);

                Tiles[i, j] = GO;
            }
        }

        //Creation of the actual selection box
        X = 0;
        Y = 0;
        SelectionBox = Instantiate(SelectionBoxPrefab, 
            Tiles[X, Y].transform.position, 
            RefObj.transform.rotation);
        SelectionBox.transform.SetParent(RefObj.transform);
        SelectionBox.name = "Selection Box";
        SelectionBox.transform.localScale = new Vector3(TileSize, TileSize, TileSize);

        SpriteRenderer SelectionBoxSprComp = SelectionBox.GetComponent<SpriteRenderer>();
        SelectionBoxSprComp.sprite = SelectionBoxSpr;
        SelectionBoxSprComp.color = Color.green;
        SelectionBoxSprComp.sortingOrder += 10;
    }

    void Update()
    {
        InputHandling();
    }

    private void InputHandling()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) Input_Up();
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) Input_Down();
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) Input_Left();
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) Input_Right();
        if (Input.GetKeyDown(KeyCode.Space)) Input_Click();
    }

    protected virtual void Input_Click()
    {
    }

    protected virtual void Input_Right()
    {
        if (X < GridWidth - 1)
        {
            ++X;
            SnapToTile();
        }
    }

    protected virtual void Input_Left()
    {
        if (X > 0)
        {
            --X;
            SnapToTile();
        }
    }

    protected virtual void Input_Down()
    {
        if (Y > 0)
        {
            --Y;
            SnapToTile();
        }
    }

    protected virtual void Input_Up()
    {
        if (Y < GridHeight - 1)
        {
            ++Y;
            SnapToTile();
        }
    }

    private void SnapToTile()
    {
        SelectionBox.transform.position = Tiles[X, Y].transform.position;
    }

    public virtual GridPieceInfo[,] CreateItemAt(int i, int j, Sprite S, GridPieceInfo[,] TileInfos)
    {
        GameObject GO = Instantiate(GridPiece, Tiles[0, 0].transform.position, RefObj.transform.rotation);
        GO.transform.parent = Tiles[i, j].transform;
        GO.transform.localPosition = Vector3.zero;

        GO.name = S.name + " [" + i + ", " + j + "]";
        GO.transform.localScale = new Vector3(1, 1, 1);

        GO.GetComponent < SpriteRenderer>().sprite = S;
        ++GO.GetComponent<SpriteRenderer>().sortingOrder;

        TileInfos[i, j].GO = GO;
        return TileInfos;
    }

    public virtual GridPieceInfo[,] DeleteItemAt(int i, int j, GridPieceInfo[,] TileInfos)
    {
        Destroy(TileInfos[i, j].GO);
        TileInfos[i, j].GO = null;
        return TileInfos;
    }

    public virtual GridPieceInfo[,] AddLayerItemAt(int i, int j, Sprite S, GridPieceInfo[,] TileInfos)
    {
        GameObject GO = Instantiate(GridPiece, Tiles[0, 0].transform.position, RefObj.transform.rotation);
        GO.transform.parent = Tiles[i, j].transform;
        GO.transform.localPosition = Vector3.zero;

        GO.name = S.name + " [" + i + ", " + j + "]";
        GO.transform.localScale = new Vector3(1, 1, 1);

        GO.GetComponent<SpriteRenderer>().sprite = S;

        TileInfos[i, j].AdditionalLayeredGOs.Add(GO);
        GO.GetComponent<SpriteRenderer>().sortingOrder += 2 + TileInfos[i, j].AdditionalLayeredGOs.Count;

        return TileInfos;
    }
}

public class GridPieceInfo
{
    public int X;
    public int Y;

    public GameObject GO;
    public GameObject Tile;

    public List<GameObject> AdditionalLayeredGOs;

    public GridPieceInfo UpNeighbour;
    public GridPieceInfo DownNeighbour;
    public GridPieceInfo LeftNeighbour;
    public GridPieceInfo RightNeighbour;


}