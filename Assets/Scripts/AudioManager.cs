using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static AudioDescricao;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource _descricaoAudioSource;
    [SerializeField] private List<AudioSource> _audioSources;

    [SerializeField] private TIPO_VOZ _tipoVoz;

    private int _indexAudioSourceSFX = 0;

    public UnityEvent<TIPO_VOZ> OnTipoDeVozMudou;

    public UnityEvent<float> OnAudioComecou;
    public UnityEvent<float> OnTempoAtualDoAudio;
    public UnityEvent OnAudioTerminou;


    private Coroutine _audioDescricaoCoroutine = null;

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        PlayAudio(_audioSources[_indexAudioSourceSFX], clip, volume);

        _indexAudioSourceSFX = (_indexAudioSourceSFX + 1) % _audioSources.Count;
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

        PararAudioDescricao();

        _audioDescricaoCoroutine = StartCoroutine(AcompanhaProgressoAudioCoroutine(_descricaoAudioSource));
    }

    public void SetTipoVoz(TIPO_VOZ novoTipoVoz)
    {
        _tipoVoz = novoTipoVoz;
        OnTipoDeVozMudou?.Invoke(_tipoVoz);
    }

    public void SetVozMasculina(bool value)
    {
        if (value) SetTipoVoz(TIPO_VOZ.Masculina);
    }

    public void SetVozFeminina(bool value)
    {
        if (value) SetTipoVoz(TIPO_VOZ.Feminina);
    }

    private IEnumerator AcompanhaProgressoAudioCoroutine(AudioSource audioSource)
    {
        OnAudioComecou?.Invoke(audioSource.clip.length);

        while (audioSource.isPlaying)
        {
            float tempoAtual = audioSource.time / audioSource.clip.length;

            OnTempoAtualDoAudio?.Invoke(tempoAtual);

            yield return null;
        }

        OnAudioTerminou?.Invoke();

        _audioDescricaoCoroutine = null;
    }

    public void PararAudioDescricao()
    {
        if (_audioDescricaoCoroutine != null)
        {
            _descricaoAudioSource.Stop();
            StopCoroutine(_audioDescricaoCoroutine);
            _audioDescricaoCoroutine = null;

            OnAudioTerminou?.Invoke();
        }
    }

    public TIPO_VOZ VozAtual => _tipoVoz;
}
