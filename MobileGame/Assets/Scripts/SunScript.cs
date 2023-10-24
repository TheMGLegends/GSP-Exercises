using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunScript : MonoBehaviour
{
    private void OnMouseDown()
    {
        gameObject.SetActive(false);
        GameManager.Instance.AddSuns(GameManager.Instance.SingleSunValue);
    }
}
