using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static AudioDescricao;

public class GerenciadorDeDescricaoEmAudio : MonoBehaviour
{
    [SerializeField] private Toggle _toggleVozMasculina;
    [SerializeField] private Toggle _toggleVozFeminina;

    public UnityEvent OnTocarUltimoAudio;

    private void Awake()
    {
        AudioManager.Instance.OnTipoDeVozMudou.AddListener(OnTipoDeVozMudou);

        OnTipoDeVozMudou(AudioManager.Instance.VozAtual);
    }

    private void OnTipoDeVozMudou(TIPO_VOZ tipoVoz)
    {
        switch(tipoVoz)
        {
            case TIPO_VOZ.Masculina: _toggleVozMasculina.SetIsOnWithoutNotify(true); break;
            case TIPO_VOZ.Feminina: _toggleVozFeminina.SetIsOnWithoutNotify(true); break;
        }
    }

    public void TriggerTocarUltimoAudio()
    {
        OnTocarUltimoAudio?.Invoke();
    }
}
