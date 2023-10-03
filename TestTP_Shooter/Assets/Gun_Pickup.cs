using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Pickup : MonoBehaviour
{
    public ScriptableObject_Gun Gun;

    void Start()
    {
        GameObject go = null;
        go = Instantiate(Gun.GunPrefab, transform.position, Quaternion.identity);
        go.transform.SetParent(gameObject.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);

            PlayerGunStats PlayerGun = other.GetComponent<PlayerGunStats>();

            if (PlayerGun.CurEquipGunID == -1)
            {
                PlayerGun.CurEquipGunID = Gun.GunID;
                PlayerGun.GunSocket.transform.GetChild(Gun.GunID).gameObject.SetActive(true);

                PlayerGun.GunAmmo[Gun.GunID] = 0;
            }

            if (PlayerGun.GunAmmo[Gun.GunID] == -1)
            {
                PlayerGun.GunAmmo[Gun.GunID] = 0;
                ++PlayerGun.ActiveGuns;
            }

            PlayerGun.GunAmmo[Gun.GunID] += (int)UnityEngine.Random.Range(Gun.ClipSize_OnPickup.x, Gun.ClipSize_OnPickup.y);
        }
    }
}
