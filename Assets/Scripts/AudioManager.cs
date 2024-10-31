using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioDescricao;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private TIPO_VOZ _tipoVoz;

    public void PlayAudio(AudioClip clip)
    {
        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();
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
