using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public enum SoundEffects
    {
        Bounce,
        Goal,
        Respawn
    }

    public List<SoundEffects> soundEffectsList = new List<SoundEffects>();
    public List<AudioClip> clipList = new List<AudioClip>();

    public Dictionary<SoundEffects, AudioClip> SFXLib = new Dictionary<SoundEffects, AudioClip>();

    public static SFXManager sfxManager;

    public GameObject SFXPrefab;

    private void Awake()
    {
        sfxManager = this;
    }

    private void Start()
    {
        for (int i = 0; i < soundEffectsList.Count; i++)
        {
            SFXLib.Add(soundEffectsList[i], clipList[i]);
        }
    }

    public void PlaySFX(SoundEffects soundEffectToPlay)
    {
        if (SFXLib.ContainsKey(soundEffectToPlay))
        {
            GameObject GO = Instantiate(SFXPrefab);
            GO.GetComponent<AudioSource>().PlayOneShot(SFXLib[soundEffectToPlay]);
            Destroy(GO, SFXLib[soundEffectToPlay].length);
        }
    }
}
