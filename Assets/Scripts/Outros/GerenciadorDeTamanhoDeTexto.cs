using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GerenciadorDeTamanhoDeTexto : MonoBehaviour
{
    [SerializeField] private Transform _containerDeTextos;

    private List<ComponenteDeTexto> _componentesDeTexto;

    private void Awake()
    {
        _componentesDeTexto = new List<ComponenteDeTexto>();

        foreach (var txt in _containerDeTextos.GetComponentsInChildren<TextMeshProUGUI>())
        {
            _componentesDeTexto.Add(new ComponenteDeTexto(txt, txt.fontSize, txt.fontSizeMin, txt.fontSizeMax));

            txt.enableAutoSizing = false;
        }
    }

    public void AlteraTamanhoDosTextos(float porcentagem)
    {
        _componentesDeTexto.ForEach(c => c._txtTexto.fontSize = (c._tamanhoMaximo - c._tamanhoMinimo) * porcentagem + c._tamanhoMinimo);
    }


    private class ComponenteDeTexto
    {
        public TextMeshProUGUI _txtTexto;
        public float _tamanhoFonteOriginal;
        public float _tamanhoMinimo;
        public float _tamanhoMaximo;

        public ComponenteDeTexto(TextMeshProUGUI txtTexto, float tamanhoFonteOriginal, float tamanhoMinimo, float tamanhoMaximo)
        {
            _txtTexto = txtTexto;
            _tamanhoFonteOriginal = tamanhoFonteOriginal;
            _tamanhoMinimo = tamanhoMinimo;
            _tamanhoMaximo = tamanhoMaximo;
        }
    }
}
