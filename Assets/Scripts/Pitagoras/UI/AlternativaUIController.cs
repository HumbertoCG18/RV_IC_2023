using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AlternativaUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _txtID;
    [SerializeField] private TextMeshProUGUI _txtAlternativa;
    [SerializeField] private XRSimpleInteractable _btnSelecionaAlternativa;
    [SerializeField] private XRSimpleInteractable _btnTocaAudio;

    public void IniciaAlternativa(PerguntaERespostaUIManager manager, int index, AudioDescricao audioDescricao)
    {
        _txtID.text = index.ToString();
        _txtAlternativa.text = audioDescricao.Descricao;

        _btnSelecionaAlternativa.selectEntered.AddListener(args => manager.AlternativaEscolhida(audioDescricao.Descricao));
        _btnTocaAudio.selectEntered.AddListener(args => AudioManager.Instance.PlayDescricao(audioDescricao));
    }
}
