using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource bgmAudio;
    [SerializeField] private AudioSource sfxAudio;

    public static AudioPlayer instance;

    private void Awake()
    {
        instance = this;
    }

    public void TocarBGM(AudioClip _musica)
    {
        bgmAudio.clip = _musica;
        bgmAudio.Play();
    }

    public void PararBGM()
    {
        bgmAudio.Stop();
    }

    public void TocarSFX(AudioClip _efeitoSonoro)
    {
        sfxAudio.PlayOneShot(_efeitoSonoro);
    }

    public void PararSFX()
    {
        sfxAudio.Stop();
    }
}

/*
 *  // Método para tocar música de fundo
    public void TocarBGM(AudioClip _musica)
    {
        bgmAudio.clip = _musica;
        bgmAudio.Play();
    }

    // Método para parar a música de fundo
    public void PararBGM()
    {
        bgmAudio.Stop();
    }

    // Método para tocar efeito sonoro
    public void TocarSFX(AudioClip _efeitoSonoro)
    {
        sfxAudio.PlayOneShot(_efeitoSonoro);
    }

    // Método para parar os efeitos sonoros
    public void PararSFX()
    {
        sfxAudio.Stop();
    }

    // Método para ajustar o volume da música de fundo
    public void AjustarVolumeBGM(float volume)
    {
        bgmAudio.volume = volume;
    }

    // Método para ajustar o volume dos efeitos sonoros
    public void AjustarVolumeSFX(float volume)
    {
        sfxAudio.volume = volume;
    }
}
// Ajustar o volume da música de fundo para 0.5 (50%)
soundManager.AjustarVolumeBGM(0.5f);

// Ajustar o volume dos efeitos sonoros para 0.7 (70%)
soundManager.AjustarVolumeSFX(0.7f);
 * 