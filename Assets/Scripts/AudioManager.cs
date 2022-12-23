using System;
using Random = UnityEngine.Random;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    public string[] snow;
    public string[] mining;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }


    }

    void Start()
    {
        //Check volume values
        Sound bgm = Array.Find(sounds, sound => sound.name == "bgm");
        int currentBGMV = PlayerPrefs.GetInt("BGMusic");
        bgm.source.volume = ((float)currentBGMV / 100) / 1.5f;

        Sound sfx1 = Array.Find(sounds, sound => sound.name == "snow1");
        Sound sfx2 = Array.Find(sounds, sound => sound.name == "snow2");
        Sound sfx3 = Array.Find(sounds, sound => sound.name == "snow3");
        Sound sfx4 = Array.Find(sounds, sound => sound.name == "snow4");
        Sound sfx5 = Array.Find(sounds, sound => sound.name == "snow5");
        Sound sfx6 = Array.Find(sounds, sound => sound.name == "impact1");
        Sound sfx7 = Array.Find(sounds, sound => sound.name == "impact2");
        Sound sfx8 = Array.Find(sounds, sound => sound.name == "impact3");
        Sound sfx9 = Array.Find(sounds, sound => sound.name == "impact4");
        Sound sfx10 = Array.Find(sounds, sound => sound.name == "impact5");
        int currentSFXV = PlayerPrefs.GetInt("SFX");
        sfx1.source.volume = (float)currentSFXV / 100;
        sfx2.source.volume = (float)currentSFXV / 100;
        sfx3.source.volume = (float)currentSFXV / 100;
        sfx4.source.volume = (float)currentSFXV / 100;
        sfx5.source.volume = (float)currentSFXV / 100;
        sfx6.source.volume = (float)currentSFXV / 100;
        sfx7.source.volume = (float)currentSFXV / 100;
        sfx8.source.volume = (float)currentSFXV / 100;
        sfx9.source.volume = (float)currentSFXV / 100;
        sfx10.source.volume = (float)currentSFXV / 100;

        Play("bgm");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void iceBreakSFX()
    {
        int snowVal = Random.Range(1, snow.Length);
        int impactVal = Random.Range(1, mining.Length);

        string snowVar = "snow" + snowVal;
        string impactVar = "impact" + impactVal;

        Play(snowVar);
        Play(impactVar);
    }

    public void bgmVolumeChange()
    {
        Sound bgm = Array.Find(sounds, sound => sound.name == "bgm");
        int currentBGMV = PlayerPrefs.GetInt("BGMusic");
        bgm.source.volume = ((float)currentBGMV / 100)/1.5f;
    }

    public void sfxVolumeChange()
    {
        Sound sfx1 = Array.Find(sounds, sound => sound.name == "snow1");
        Sound sfx2 = Array.Find(sounds, sound => sound.name == "snow2");
        Sound sfx3 = Array.Find(sounds, sound => sound.name == "snow3");
        Sound sfx4 = Array.Find(sounds, sound => sound.name == "snow4");
        Sound sfx5 = Array.Find(sounds, sound => sound.name == "snow5");
        Sound sfx6 = Array.Find(sounds, sound => sound.name == "impact1");
        Sound sfx7 = Array.Find(sounds, sound => sound.name == "impact2");
        Sound sfx8 = Array.Find(sounds, sound => sound.name == "impact3");
        Sound sfx9 = Array.Find(sounds, sound => sound.name == "impact4");
        Sound sfx10 = Array.Find(sounds, sound => sound.name == "impact5");
        int currentSFXV = PlayerPrefs.GetInt("SFX");
        sfx1.source.volume = (float)currentSFXV / 100;
        sfx2.source.volume = (float)currentSFXV / 100;
        sfx3.source.volume = (float)currentSFXV / 100;
        sfx4.source.volume = (float)currentSFXV / 100;
        sfx5.source.volume = (float)currentSFXV / 100;
        sfx6.source.volume = (float)currentSFXV / 100;
        sfx7.source.volume = (float)currentSFXV / 100;
        sfx8.source.volume = (float)currentSFXV / 100;
        sfx9.source.volume = (float)currentSFXV / 100;
        sfx10.source.volume = (float)currentSFXV / 100;
    }
}
