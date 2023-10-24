using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCreator : MonoBehaviour
{
    public GameObject PlantToSpawn;
    private GameObject NewPlant;

    public int Cost;

    private void Update()
    {
        if (NewPlant)
        {
            Vector3 NewPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            NewPlant.transform.position = new Vector3(NewPos.x, NewPos.y, 0);
        }

        bool bShouldDestroy = false;
        if (Input.GetMouseButtonUp(0))
        {
            if (NewPlant)
            {
                if (GridGenerator.Grid.ActiveTile)
                {
                    if (!GridGenerator.Grid.ActiveTile.MyTile.bPlantOnMe)
                    {
                        //Planting mechanic

                        NewPlant.transform.position = GridGenerator.Grid.ActiveTile.transform.position;
                        GridGenerator.Grid.ActiveTile.MyTile.bPlantOnMe = true;

                        NewPlant.GetComponent<GenericPlant>().AssociatedTile = GridGenerator.Grid.ActiveTile.MyTile;

                        NewPlant.GetComponent<GenericPlant>().bIsAwake = true;

                        GameManager.Instance.RemoveSuns(Cost);
                        NewPlant = null;
                    }
                    else
                    {
                        bShouldDestroy = true;
                    }
                }
                else
                {
                    bShouldDestroy = true;
                }
            }
            GridGenerator.Grid.bAboutToPlant = false;
            if (GridGenerator.Grid.ActiveTile)
            {
                GridGenerator.Grid.ActiveTile.ResetColor();
            }
        }

        if (bShouldDestroy) Destroy(NewPlant);
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.SunPriceCheck(Cost))
        {
            NewPlant = Instantiate(PlantToSpawn, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);

            GridGenerator.Grid.bAboutToPlant = true;
        }
    }
}
