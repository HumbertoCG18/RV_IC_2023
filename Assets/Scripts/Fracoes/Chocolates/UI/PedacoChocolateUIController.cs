using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedacoChocolateUIController : MonoBehaviour
{
    [SerializeField] private RectTransform _pedacoBordaEsquerda;
    [SerializeField] private RectTransform _pedacoBordaDireita;
    [SerializeField] private RectTransform _pedacoDoMeio;

    public void SetTamanhoChocolate(int tamanho)
    {
        float bordas = _pedacoBordaEsquerda.sizeDelta.x + _pedacoBordaDireita.sizeDelta.x;

        _pedacoDoMeio.sizeDelta = new Vector2(tamanho - bordas, _pedacoDoMeio.sizeDelta.y);
    }
}
