using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;

public class MoonlightGame : GridGenerator
{
    public Sprite Spr_Cloud;
    public Sprite Spr_Star;
    public Sprite Spr_MoonHalf;
    public Sprite Spr_MoonDark;

    public MoonlightGridPiece[,] CustomTileInfos;

    public int MaxMoons;

    private MoonTile[] FinalSolution;
    private MoonTile[] Attempt;

    protected override void GenerateGameGrid()
    {
        FinalSolution = new MoonTile[MaxMoons];
        Attempt = new MoonTile[MaxMoons];

        //1: Create and populate the custom tiles with empty info.

        //2: Attempt at placing one cloud and one star per row and column RANDOMLY

        //3: Attempt at spawning required number of moons from designers in vacant spots

        //4: Foreach moon placed, generate final solution based on their positions
        // and the positions of the randomized clouds and stars

        //5: Validate solution! It should be else bug!

        //6: If solution valid, cleanup! DONT TOUCH THE MOONS!

    }

    protected override void Input_Click()
    {
        switch (CustomTileInfos[X,Y].TileState)
        {
            case MoonlightGridPiece.EMoonlight_TileState.None:
                CustomTileInfos = CreateItemAt(X, Y, Spr_Star, CustomTileInfos) as MoonlightGridPiece[,];
                CustomTileInfos[X, Y].TileState = MoonlightGridPiece.EMoonlight_TileState.Star;
                break;
            case MoonlightGridPiece.EMoonlight_TileState.Star:
                CustomTileInfos = DeleteItemAt(X, Y, CustomTileInfos) as MoonlightGridPiece[,];
                CustomTileInfos = CreateItemAt(X, Y, Spr_Cloud, CustomTileInfos) as MoonlightGridPiece[,];
                CustomTileInfos[X, Y].TileState = MoonlightGridPiece.EMoonlight_TileState.Moon;
                break;
            case MoonlightGridPiece.EMoonlight_TileState.Cloud:
                CustomTileInfos = DeleteItemAt(X, Y, CustomTileInfos) as MoonlightGridPiece[,];
                CustomTileInfos[X, Y].TileState = MoonlightGridPiece.EMoonlight_TileState.None;
                break;
            default:
                break;
        }

        PuzzleValidation(true);
    }

    private void PuzzleValidation(bool IsPlayer)
    {
        //SATISFACTION CRITERIA 1 - Stars and Clouds (Rows & Columns)

        //1: Does the number of STARS and CLOUDS match the number of rows and columns?
        //2: Do we have a unique STAR and CLOUD for each column and row?

        // LOOPING CODE

        //3: IF the loops were correct, we should have as many correct rows and columns as their count.

        // VALIDATE LOOPS

        // SATISFACTION CRITERIA 2 - MOONS
        //1: Generate the attempt - Gather data from the grid that shows how each moon is lit.
        // Gather data...
        //2: Compare attempt with final solution - Is each moon lit properly?

        //3: Collect the number of correct moons! Are all the moons lit properly?

        //4: If so - win
    }
}

public class MoonlightGridPiece : GridPieceInfo
{
    public enum EMoonlight_TileState
    {
        None,
        Star,
        Moon,
        Cloud
    }

    public EMoonlight_TileState TileState;
}

public class MoonTile
{
    public int X;
    public int Y;

    public bool UP;
    public bool DOWN;
    public bool LEFT;
    public bool RIGHT;

    public int LightsOnMe;

    public bool IsFullMoon
    {
        get
        {
            return (LEFT && RIGHT || UP && DOWN);
        }
    }
}