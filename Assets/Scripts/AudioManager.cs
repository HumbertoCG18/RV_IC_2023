using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioDescricao;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource _descricaoAudioSource;
    [SerializeField] private AudioSource _sfxAudioSource;

    [SerializeField] private TIPO_VOZ _tipoVoz;

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        PlayAudio(_sfxAudioSource, clip, volume);
    }

    public void PlayAudio(AudioClip clip, float volume=1f)
    {
        PlayAudio(_descricaoAudioSource, clip, volume);
    }

    private void PlayAudio(AudioSource audioSource, AudioClip clip, float volume)
    {
        audioSource.volume = volume;
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PlayDescricao(AudioDescricao audioDescricao)
    {
        switch (_tipoVoz)
        {
            case TIPO_VOZ.Masculina: if (audioDescricao.AudioMasculino != null) PlayAudio(audioDescricao.AudioMasculino); break;
            case TIPO_VOZ.Feminina: if (audioDescricao.AudioFeminino != null) PlayAudio(audioDescricao.AudioFeminino); break;
        }
    }

    public void SetTipoVoz(TIPO_VOZ novoTipoVoz)
    {
        _tipoVoz = novoTipoVoz;
    }

    public void SetVozMasculina(bool value)
    {
        if (value) SetTipoVoz(TIPO_VOZ.Masculina);
    }

    public void SetVozFeminina(bool value)
    {
        if (value) SetTipoVoz(TIPO_VOZ.Feminina);
    }
}
