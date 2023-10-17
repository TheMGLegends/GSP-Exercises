using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        CustomTileInfos = new MoonlightGridPiece[GridWidth, GridHeight];

        for (int i = 0; i < GridWidth; i++)
        {
            for (int j = 0; j < GridHeight; j++)
            {
                CustomTileInfos[i, j] = new MoonlightGridPiece();
                CustomTileInfos[i, j].X = i;
                CustomTileInfos[i, j].Y = j;

                CustomTileInfos[i, j].AdditionalLayeredGOs = new List<GameObject>();
                CustomTileInfos[i, j].TileState = MoonlightGridPiece.EMoonlight_TileState.None;
            }
        }

        //2: Attempt at placing one cloud and one star per row and column RANDOMLY
        int WhileLoops = 0;
        int WhileLoops_B = 0;

        List<int> AcceptableStars = new List<int>();
        List<int> AcceptableClouds = new List<int>();

        for (int i = 0; i < GridWidth; i++)
        {
            AcceptableClouds.Add(i);
            AcceptableStars.Add(i);
        }

        for (int i = 0; i < GridHeight; i++)
        {
            int R1 = AcceptableStars[UnityEngine.Random.Range(0, AcceptableStars.Count)];
            int R2 = AcceptableClouds[UnityEngine.Random.Range(0, AcceptableClouds.Count)];

            WhileLoops = 0;
            while (R1 == R2)
            {
                R1 = AcceptableStars[UnityEngine.Random.Range(0, AcceptableStars.Count)];
                R2 = AcceptableClouds[UnityEngine.Random.Range(0, AcceptableClouds.Count)];

                ++WhileLoops;

                if (WhileLoops > 9000)
                {
                    RegenarateGrid();
                    return;
                }
            }

            AcceptableStars.Remove(R1);
            AcceptableClouds.Remove(R2);

            CustomTileInfos = CreateItemAt(R1, i, Spr_Star, CustomTileInfos) as MoonlightGridPiece[,];
            CustomTileInfos[R1, i].TileState = MoonlightGridPiece.EMoonlight_TileState.Star;

            CustomTileInfos = CreateItemAt(R2, i, Spr_Cloud, CustomTileInfos) as MoonlightGridPiece[,];
            CustomTileInfos[R2, i].TileState = MoonlightGridPiece.EMoonlight_TileState.Cloud;
        }

        //3: Attempt at spawning required number of moons from designers in vacant spots
        int MoonsToSpawn = MaxMoons;
        List<GridPieceInfo> Moons = new List<GridPieceInfo>();

        WhileLoops_B = 0;
        while(MoonsToSpawn > 0)
        {
            int R1 = UnityEngine.Random.Range(0, GridWidth);
            int R2 = UnityEngine.Random.Range(0, GridHeight);

            WhileLoops = 0;
            while (CustomTileInfos[R1, R2].TileState != MoonlightGridPiece.EMoonlight_TileState.None)
            {
                R1 = UnityEngine.Random.Range(0, GridWidth);
                R2 = UnityEngine.Random.Range(0, GridHeight);

                ++WhileLoops;

                if (WhileLoops > 9000)
                {
                    RegenarateGrid();
                    return;
                }
            }

            CustomTileInfos = CreateItemAt(R1, R2, Spr_MoonDark, CustomTileInfos) as MoonlightGridPiece[,];
            CustomTileInfos[R1, R2].TileState = MoonlightGridPiece.EMoonlight_TileState.Moon;
            --MoonsToSpawn;

            Moons.Add(CustomTileInfos[R1, R2]);

            ++WhileLoops_B;

            if (WhileLoops_B > 9000)
            {
                RegenarateGrid();
                return;
            }
        }


        //4: Foreach moon placed, generate final solution based on their positions
        // and the positions of the randomized clouds and stars
        int MoonLoop = 0;

        foreach (var item in Moons)
        {
            int MoonX = item.X;
            int MoonY = item.Y;

            bool ShouldSpawnTopMoon = false;
            bool ShouldSpawnBottomMoon = false;
            bool ShouldSpawnLeftMoon = false;
            bool ShouldSpawnRightMoon = false;

            MoonlightGridPiece TopNeighbour = GetTopNeighbour(CustomTileInfos[MoonX, MoonY]); 
            if (TopNeighbour != null)
            {
                ShouldSpawnTopMoon = TopNeighbour.TileState == MoonlightGridPiece.EMoonlight_TileState.Star;
            }

            MoonlightGridPiece BottomNeighbour = GetBottomNeighbour(CustomTileInfos[MoonX, MoonY]);
            if (BottomNeighbour != null)
            {
                ShouldSpawnBottomMoon = BottomNeighbour.TileState == MoonlightGridPiece.EMoonlight_TileState.Star;
            }

            MoonlightGridPiece LeftNeighbour = GetLeftNeighbour(CustomTileInfos[MoonX, MoonY]);
            if (LeftNeighbour != null)
            {
                ShouldSpawnLeftMoon = LeftNeighbour.TileState == MoonlightGridPiece.EMoonlight_TileState.Star;
            }

            MoonlightGridPiece RightNeighbour = GetRightNeighbour(CustomTileInfos[MoonX, MoonY]);
            if (RightNeighbour != null)
            {
                ShouldSpawnRightMoon = RightNeighbour.TileState == MoonlightGridPiece.EMoonlight_TileState.Star;
            }

            if (ShouldSpawnTopMoon)
            {
                CustomTileInfos = AddLayerItemAt(MoonX, MoonY, Spr_MoonHalf, CustomTileInfos) as MoonlightGridPiece[,];
                CustomTileInfos[MoonX, MoonY].AdditionalLayeredGOs[CustomTileInfos[MoonX, MoonY].AdditionalLayeredGOs.Count - 1].transform.Rotate(new Vector3(0, 0, 90));
            }

            if (ShouldSpawnBottomMoon)
            {
                CustomTileInfos = AddLayerItemAt(MoonX, MoonY, Spr_MoonHalf, CustomTileInfos) as MoonlightGridPiece[,];
                CustomTileInfos[MoonX, MoonY].AdditionalLayeredGOs[CustomTileInfos[MoonX, MoonY].AdditionalLayeredGOs.Count - 1].transform.Rotate(new Vector3(0, 0, -90));
            }

            if (ShouldSpawnLeftMoon)
            {
                CustomTileInfos = AddLayerItemAt(MoonX, MoonY, Spr_MoonHalf, CustomTileInfos) as MoonlightGridPiece[,];
                CustomTileInfos[MoonX, MoonY].AdditionalLayeredGOs[CustomTileInfos[MoonX, MoonY].AdditionalLayeredGOs.Count - 1].transform.Rotate(new Vector3(0, 180, 0));
            }

            if (ShouldSpawnRightMoon)
            {
                CustomTileInfos = AddLayerItemAt(MoonX, MoonY, Spr_MoonHalf, CustomTileInfos) as MoonlightGridPiece[,];
            }

            FinalSolution[MoonLoop] = new MoonTile();
            FinalSolution[MoonLoop].DOWN = ShouldSpawnBottomMoon;
            FinalSolution[MoonLoop].RIGHT = ShouldSpawnRightMoon;
            FinalSolution[MoonLoop].LEFT = ShouldSpawnLeftMoon;
            FinalSolution[MoonLoop].UP = ShouldSpawnTopMoon;
            FinalSolution[MoonLoop].X = MoonX;
            FinalSolution[MoonLoop].Y = MoonY;

            int LightsOnMeTemp = 0;
            if (ShouldSpawnLeftMoon) ++LightsOnMeTemp;
            if (ShouldSpawnRightMoon) ++LightsOnMeTemp;
            if (ShouldSpawnTopMoon) ++LightsOnMeTemp;
            if (ShouldSpawnBottomMoon) ++LightsOnMeTemp;

            FinalSolution[MoonLoop].LightsOnMe = LightsOnMeTemp;

            Attempt[MoonLoop] = new MoonTile();
            Attempt[MoonLoop].X = MoonX;
            Attempt[MoonLoop].Y = MoonY;

            ++MoonLoop;
        }

        //5: Validate solution! It should be else bug!
        PuzzleValidation(false);

        //6: If solution valid, cleanup! DONT TOUCH THE MOONS!
        for (int i = 0; i < GridWidth; i++)
        {
            for (int j = 0; j < GridHeight; j++)
            {
        
                if (CustomTileInfos[i, j].TileState == MoonlightGridPiece.EMoonlight_TileState.Cloud || 
                    CustomTileInfos[i, j].TileState == MoonlightGridPiece.EMoonlight_TileState.Star)
                {
                    CustomTileInfos = DeleteItemAt(i, j, CustomTileInfos) as MoonlightGridPiece[,];
                    CustomTileInfos[i, j].TileState = MoonlightGridPiece.EMoonlight_TileState.None;
                }
            }
        
        }
    }

    private MoonlightGridPiece GetRightNeighbour(MoonlightGridPiece MGP)
    {
        if (MGP.X + 1 == GridWidth)
        {
            return null;
        }
        else
        {
            if (CustomTileInfos[MGP.X + 1, MGP.Y].GO != null)
            {
                if (CustomTileInfos[MGP.X + 1, MGP.Y].TileState != MoonlightGridPiece.EMoonlight_TileState.None)
                {
                    return CustomTileInfos[MGP.X + 1, MGP.Y];
                }
                else
                {
                    return GetRightNeighbour(CustomTileInfos[MGP.X + 1, MGP.Y]);
                }
            }
            else
            {
                return GetRightNeighbour(CustomTileInfos[MGP.X + 1, MGP.Y]);
            }
        }
    }

    private MoonlightGridPiece GetLeftNeighbour(MoonlightGridPiece MGP)
    {
        if (MGP.X -1 == -1)
        {
            return null;
        }
        else
        {
            if (CustomTileInfos[MGP.X - 1, MGP.Y].GO != null)
            {
                if (CustomTileInfos[MGP.X - 1, MGP.Y].TileState != MoonlightGridPiece.EMoonlight_TileState.None)
                {
                    return CustomTileInfos[MGP.X - 1, MGP.Y];
                }
                else
                {
                    return GetLeftNeighbour(CustomTileInfos[MGP.X - 1, MGP.Y]);
                }
            }
            else
            {
                return GetLeftNeighbour(CustomTileInfos[MGP.X - 1, MGP.Y]);
            }
        }
    }

    private MoonlightGridPiece GetBottomNeighbour(MoonlightGridPiece MGP)
    {
        if (MGP.Y - 1 == -1)
        {
            return null;
        }
        else
        {
            if (CustomTileInfos[MGP.X, MGP.Y - 1].GO != null)
            {
                if (CustomTileInfos[MGP.X, MGP.Y - 1].TileState != MoonlightGridPiece.EMoonlight_TileState.None)
                {
                    return CustomTileInfos[MGP.X, MGP.Y - 1];
                }
                else
                {
                    return GetBottomNeighbour(CustomTileInfos[MGP.X, MGP.Y - 1]);
                }
            }
            else
            {
                return GetBottomNeighbour(CustomTileInfos[MGP.X, MGP.Y - 1]);
            }
        }
    }

    private MoonlightGridPiece GetTopNeighbour(MoonlightGridPiece MGP)
    {
        if (MGP.Y + 1 == GridHeight)
        {
            return null;
        }
        else
        {
            if (CustomTileInfos[MGP.X, MGP.Y + 1].GO != null)
            {
                if (CustomTileInfos[MGP.X, MGP.Y + 1].TileState != MoonlightGridPiece.EMoonlight_TileState.None)
                {
                    return CustomTileInfos[MGP.X, MGP.Y + 1];
                }
                else
                {
                    return GetTopNeighbour(CustomTileInfos[MGP.X, MGP.Y + 1]);
                }
            }
            else
            {
                return GetTopNeighbour(CustomTileInfos[MGP.X, MGP.Y + 1]);
            }
        }
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
                CustomTileInfos[X, Y].TileState = MoonlightGridPiece.EMoonlight_TileState.Cloud;
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
        int SOLUTIONSCORE = 0;

        int RequiredCorrectCols = GridWidth;
        int RequiredCorrectRows = GridHeight;

        int ColsFound = 0;
        int RowsFound = 0;

        //SATISFACTION CRITERIA 1 - Stars and Clouds (Rows & Columns)

        //1: Does the number of STARS and CLOUDS match the number of rows and columns?
        //2: Do we have a unique STAR and CLOUD for each column and row?
        for (int i = 0; i < GridWidth; i++)
        {
            int Stars_R = 0;
            int Clouds_R = 0;

            for (int j = 0; j < GridHeight; j++)
            {
                if (CustomTileInfos[i, j].TileState == MoonlightGridPiece.EMoonlight_TileState.Cloud)
                {
                    ++Clouds_R;
                }
                if (CustomTileInfos[i, j].TileState == MoonlightGridPiece.EMoonlight_TileState.Star)
                {
                    ++Stars_R;
                }
            }

            //If there is only one star and one cloud on each row, then good job!
            if (Stars_R == 1 && Clouds_R == 1)
            {
                ++RowsFound;
            }
        }

        for (int i = 0; i < GridHeight; i++)
        {
            int Stars_C = 0;
            int Clouds_C = 0;

            for (int j = 0; j < GridHeight; j++)
            {
                if (CustomTileInfos[i, j].TileState == MoonlightGridPiece.EMoonlight_TileState.Cloud)
                {
                    ++Clouds_C;
                }
                if (CustomTileInfos[i, j].TileState == MoonlightGridPiece.EMoonlight_TileState.Star)
                {
                    ++Stars_C;
                }
            }

            //If there is only one star and one cloud on each column, then good job!
            if (Stars_C == 1 && Clouds_C == 1)
            {
                ++ColsFound;
            }
        }

        // LOOPING CODE

        //3: If the loops were correct, we should have as many correct rows and columns as their count.
        if (ColsFound == RequiredCorrectCols && RowsFound == RequiredCorrectRows)
        {
            ++SOLUTIONSCORE;
        }

        // SATISFACTION CRITERIA 2 - MOONS
        //1: Generate the attempt - Gather data from the grid that shows how each moon is lit.
        foreach (var Moon in Attempt)
        {
            int MoonX = Moon.X;
            int MoonY = Moon.Y;

            bool ShouldSpawnTopMoon = false;
            bool ShouldSpawnBottomMoon = false;
            bool ShouldSpawnLeftMoon = false;
            bool ShouldSpawnRightMoon = false;

            MoonlightGridPiece LeftNeighour = GetLeftNeighbour(CustomTileInfos[MoonX, MoonY]);
            if (LeftNeighour != null)
            {
                ShouldSpawnLeftMoon = LeftNeighour.TileState == MoonlightGridPiece.EMoonlight_TileState.Star;
            }

            MoonlightGridPiece RightNeighour = GetRightNeighbour(CustomTileInfos[MoonX, MoonY]);
            if (RightNeighour != null)
            {
                ShouldSpawnRightMoon = RightNeighour.TileState == MoonlightGridPiece.EMoonlight_TileState.Star;
            }

            MoonlightGridPiece TopNeighour = GetTopNeighbour(CustomTileInfos[MoonX, MoonY]);
            if (TopNeighour != null)
            {
                ShouldSpawnTopMoon = TopNeighour.TileState == MoonlightGridPiece.EMoonlight_TileState.Star;
            }

            MoonlightGridPiece BottomNeighour = GetBottomNeighbour(CustomTileInfos[MoonX, MoonY]);
            if (BottomNeighour != null)
            {
                ShouldSpawnBottomMoon = BottomNeighour.TileState == MoonlightGridPiece.EMoonlight_TileState.Star;
            }

            Moon.UP = ShouldSpawnTopMoon;
            Moon.DOWN = ShouldSpawnBottomMoon;
            Moon.LEFT = ShouldSpawnLeftMoon;
            Moon.RIGHT = ShouldSpawnRightMoon;

            int LightsOnMeTemp = 0;

            if (ShouldSpawnTopMoon) ++LightsOnMeTemp;
            if (ShouldSpawnBottomMoon) ++LightsOnMeTemp;
            if (ShouldSpawnLeftMoon) ++LightsOnMeTemp;
            if (ShouldSpawnRightMoon) ++LightsOnMeTemp;

            Moon.LightsOnMe = LightsOnMeTemp;
        }


        //2: Compare attempt with final solution - Is each moon lit properly?
        int CorrectMoons = 0;
        for (int i = 0; i < Attempt.Length; i++)
        {
            if (FinalSolution[i].LightsOnMe == Attempt[i].LightsOnMe)
            {
                if (FinalSolution[i].LightsOnMe == 0)
                {
                    ++CorrectMoons;
                }
                else if (FinalSolution[i].LightsOnMe == 1)
                {
                    if (FinalSolution[i].UP == Attempt[i].UP ||
                        FinalSolution[i].DOWN == Attempt[i].DOWN ||
                        FinalSolution[i].LEFT == Attempt[i].LEFT ||
                        FinalSolution[i].RIGHT == Attempt[i].RIGHT)
                    {
                        ++CorrectMoons;
                    }
                }
                else if (FinalSolution[i].LightsOnMe == 2)
                {
                    if (FinalSolution[i].IsFullMoon == Attempt[i].IsFullMoon)
                    {
                        ++CorrectMoons;
                    }
                    else
                    {
                        int CorrectDirs = 0;
                        if (FinalSolution[i].UP == Attempt[i].UP) ++CorrectDirs;
                        if (FinalSolution[i].DOWN == Attempt[i].DOWN) ++CorrectDirs;
                        if (FinalSolution[i].LEFT == Attempt[i].LEFT) ++CorrectDirs;
                        if (FinalSolution[i].RIGHT == Attempt[i].RIGHT) ++CorrectDirs;

                        if (CorrectDirs == 2)
                        {
                            ++CorrectMoons;
                        }
                    }
                }
                else if (FinalSolution[i].LightsOnMe == 3)
                {
                    if (FinalSolution[i].IsFullMoon == Attempt[i].IsFullMoon)
                    {
                        ++CorrectMoons;
                    }
                }
                else if (FinalSolution[i].LightsOnMe == 4)
                {
                    if (FinalSolution[i].UP == Attempt[i].UP ||
                        FinalSolution[i].DOWN == Attempt[i].DOWN ||
                        FinalSolution[i].LEFT == Attempt[i].LEFT ||
                        FinalSolution[i].RIGHT == Attempt[i].RIGHT)
                    {
                        ++CorrectMoons;
                    }
                }
            }
        }

        //3: Collect the number of correct moons! Are all the moons lit properly?
        if (CorrectMoons == MaxMoons)
        {
            ++SOLUTIONSCORE;
        }

        //4: If so - win
        if (SOLUTIONSCORE == 2)
        {
            print(IsPlayer ? "Solved!" : "Validated!");
        }
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