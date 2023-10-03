using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "SOs/Guns", order = 1)]
public class ScriptableObject_Gun : ScriptableObject
{
    public string GunName;

    public enum EShootType
    {
        Instant,
        Hold,
        Charge,
        Projectile
    }

    public EShootType ShootingType;

    [Tooltip("If zero, then you don't reload, else you do")]
    public float ClipSize;

    public int GunID;

    [Range(1f, 100f)]
    public float Damage;

    public float FireRate;
    public float Recoil;

    //public int ClipSize_Min;
    //public int ClipSize_Max;

    public Vector2 ClipSize_OnPickup; //X = Min, Y = Max

    public GameObject GunPrefab;
}
