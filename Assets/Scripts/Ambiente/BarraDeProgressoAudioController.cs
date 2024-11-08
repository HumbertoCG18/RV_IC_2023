using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BarraDeProgressoAudioController : MonoBehaviour
{
    [SerializeField] private Slider _sliderTempo;
    [SerializeField] private AudioDescricao _audioDescricao;

    public UnityEvent OnAudioTerminou;

    private AudioManager _audioManagerInstance;

    private void Start()
    {
        _audioManagerInstance = AudioManager.Instance;
        _sliderTempo.gameObject.SetActive(false);
    }

    public void PlayAudio()
    {
        _audioManagerInstance.PararAudioDescricao();

        _audioManagerInstance.OnAudioTerminou.AddListener(AudioTerminou);
        _audioManagerInstance.OnTempoAtualDoAudio.AddListener(AudioEmAndamento);

        _audioManagerInstance.PlayDescricao(_audioDescricao);

        _sliderTempo.gameObject.SetActive(true);
    }

    public void PararAudio()
    {
        _audioManagerInstance.PararAudioDescricao();
    }

    private void AudioTerminou()
    {
        _audioManagerInstance.OnAudioTerminou.RemoveListener(AudioTerminou);
        _audioManagerInstance.OnTempoAtualDoAudio.RemoveListener(AudioEmAndamento);

        _sliderTempo.gameObject.SetActive(false);

        OnAudioTerminou?.Invoke();
    }

    private void AudioEmAndamento(float tempo)
    {
        _sliderTempo.value = tempo;
    }
}
