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
}
