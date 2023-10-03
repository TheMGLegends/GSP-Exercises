using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunStats : MonoBehaviour
{
    public List<int> GunAmmo = new List<int>();
    public List<ScriptableObject_Gun> GunTypes= new List<ScriptableObject_Gun>();

    public int GunsOnMe = 0;

    public GameObject GunSocket;

    public int CurEquipGunID = -1;

    internal int ActiveGuns;

    private SimpleTPController LeChar;

    public bool RapidFire;

    private float CurrentFireRateTimer;
    private float WeaponFireRate;

    // Start is called before the first frame update
    void Start()
    {
        GunsOnMe = GunSocket.transform.childCount;

        for (int i = 0; i < GunSocket.transform.childCount; i++)
        {
            GunSocket.transform.GetChild(i).gameObject.SetActive(false);
            GunAmmo.Add(-1);
        }
        LeChar = GetComponent<SimpleTPController>();
    }

    // Update is called once per frame
    void Update()
    {
        Switching();

        Shooting();
    }

    private void Shooting()
    {
        if (CurEquipGunID == -1)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && LeChar.IsAiming && GunAmmo[CurEquipGunID] > 0)
        {
            switch (GunTypes[CurEquipGunID].ShootingType)
            {
                case ScriptableObject_Gun.EShootType.Instant:
                    --GunAmmo[CurEquipGunID];
                    break;
                case ScriptableObject_Gun.EShootType.Hold:
                    WeaponFireRate = GunTypes[CurEquipGunID].FireRate;
                    RapidFire = true;
                    --GunAmmo[CurEquipGunID];
                    break;
                case ScriptableObject_Gun.EShootType.Charge:
                    break;
                case ScriptableObject_Gun.EShootType.Projectile:
                    break;
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            switch (GunTypes[CurEquipGunID].ShootingType)
            {
                case ScriptableObject_Gun.EShootType.Hold:
                    RapidFire = false;
                    break;
                case ScriptableObject_Gun.EShootType.Charge:
                    break;
            }
        }

        if (RapidFire)
        {
            if (GunAmmo[CurEquipGunID] > 0)
            {
                // -1
                CurrentFireRateTimer += Time.deltaTime;

                if (CurrentFireRateTimer > WeaponFireRate)
                {
                    --GunAmmo[CurEquipGunID];
                    CurrentFireRateTimer = 0;
                }
            }
            else
            {
                RapidFire = false;
            }
        }
    }

    private void Switching()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && CurEquipGunID != -1 && ActiveGuns > 1)
        {
            int TempID = CurEquipGunID;

            int Loops = 0;

            while (true)
            {
                ++Loops;
                if (Loops > 1000)
                {
                    print("You screwed up!");
                    break;
                }

                if (CurEquipGunID == GunsOnMe - 1)
                {
                    TempID = 0;
                }
                else
                {
                    ++TempID;
                }

                if (GunAmmo[TempID] >= 0)
                {
                    // I have the gun!
                    CurEquipGunID = TempID;
                    DisableAllGuns();
                    GunSocket.transform.GetChild(TempID).gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    private void DisableAllGuns()
    {
        for (int i = 0; i < GunSocket.transform.childCount; i++)
        {
            GunSocket.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
