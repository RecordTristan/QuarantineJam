using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController instance;

    public AudioSource[] musicSource;
    public AudioSource sfxSource;

    public AudioClip musicHUD;
    public AudioClip musicGame;

    public float maxVolume = 0.5f;
    private int _currentAudio = 0;

    void Awake()
    {
        if (instance) Destroy(gameObject);
        else instance = this;

        DontDestroyOnLoad(gameObject);

        PlayMusic(musicGame);
    }
    // Update is called once per frame
    void Update()
    {
        if (musicSource[_currentAudio].volume < maxVolume)
        {
            if (_currentAudio == 0)
            {
                musicSource[1].volume -= Time.deltaTime;
            }
            else
            {
                musicSource[0].volume -= Time.deltaTime;
            }
            musicSource[_currentAudio].volume += Time.deltaTime;
        }
        else
        {
            musicSource[_currentAudio].volume = maxVolume;
        }
    }
    public void PlayMusic(AudioClip Music, bool Loop = true, ulong Time = 0)
    {
        if (musicSource[_currentAudio].clip == Music)
        {
            return;
        }
        if (_currentAudio == 0)
        {
            _currentAudio = 1;
            musicSource[1].clip = Music;
            musicSource[1].loop = Loop;
            musicSource[1].Play(Time);
        }
        else
        {
            _currentAudio = 0;
            musicSource[0].clip = Music;
            musicSource[0].loop = Loop;
            musicSource[0].Play(Time);
        }
    }

    public void PlaySFX(AudioClip sfx)
    {
        sfxSource.PlayOneShot(sfx);
    }
    public void PlaySFX(AudioClip[] sfx)
    {
        int rand = UnityEngine.Random.Range(0, sfx.Length);
        sfxSource.PlayOneShot(sfx[rand]);
    }
}
