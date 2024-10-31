using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RegistradorDeTentativasManager : MonoBehaviour
{
    [SerializeField] private Transform _campoDeTentativas;
    [SerializeField] private GameObject _elementTentativaPrefab;

    private List<string> _tentativas;

    private void Awake()
    {
        _tentativas = new List<string>();
    }

    public bool AtualizaTentativas(List<BalancaPesoController> pesos)
    {
        string tentativa = GeraStringDaTentativa(pesos);

        if (_tentativas.Contains(tentativa))
        {
            return false;
        }

        _tentativas.Add(tentativa);

        var instancia = Instantiate(_elementTentativaPrefab, _campoDeTentativas);
        instancia.GetComponentInChildren<TextMeshProUGUI>().text = tentativa;

        return true;
    }

    public string GeraStringDaTentativa(List<BalancaPesoController> pesos)
    {
        var variaveis = pesos.FindAll(p => p.IsOculto).ToList();
        var valores = pesos.FindAll(p => !p.IsOculto).OrderBy(p => p.Peso).ToList();

        string resultado = "";

        foreach (var variavel in variaveis) resultado += "X + ";

        for (var i = 0; i < valores.Count - 1; i++) resultado += valores[i].Peso.ToString() + " + ";

        resultado += valores[valores.Count - 1].Peso.ToString();

        return resultado;
    }

    public void LimpaTentativas()
    {
        _tentativas.Clear();

        foreach (Transform child in _campoDeTentativas) Destroy(child.gameObject);
    }
}
