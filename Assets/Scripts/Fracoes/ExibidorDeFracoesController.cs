using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExibidorDeFracoesController : MonoBehaviour
{
    [SerializeField] private Transform _containerFracoesUITransform;
    [SerializeField] private GameObject _fracaoUIPrefab;
    [SerializeField] private float _espacoEntreFracoes = 0.1f;

    private List<FracaoUIController> _fracoesControllers;

    private void Awake()
    {
        _fracoesControllers = _containerFracoesUITransform.GetComponentsInChildren<FracaoUIController>().ToList();

        LimpaFracoes();
    }

    public void LimpaFracoes()
    {
        _fracoesControllers.ForEach(f => Destroy(f.gameObject));
        _fracoesControllers.Clear();
    }

    public void AtualizaUI()
    {
        if (_fracoesControllers.Count == 0) return;

        int width = _fracoesControllers[0].Width;
        float scale = _fracoesControllers[0].Scale;

        float larguraTotal = width * _fracoesControllers.Count + _espacoEntreFracoes * (_fracoesControllers.Count - 1);
        Vector3 posicaoInicial = (Vector3.forward * larguraTotal * -0.5f + Vector3.forward * width * 0.5f);
        Vector3 passo = Vector3.forward * (width + _espacoEntreFracoes);

        int i = 0;
        foreach (var fracaoController in _fracoesControllers)
        {
            fracaoController.transform.localPosition = (posicaoInicial + passo * i) * scale;

            i++;
        }
    }

    public void AdicionaFracao(IFracao fracao)
    {
        FracaoUIController instancia = Instantiate(_fracaoUIPrefab, _containerFracoesUITransform).GetComponent<FracaoUIController>();

        instancia.SetFracao(fracao);

        _fracoesControllers.Add(instancia);

        AtualizaUI();
    }

    public void RemoveFracao(IFracao fracao)
    {
        var fracaoParaDeletar = _fracoesControllers.Find(f => f.Fracao == fracao);

        if (fracaoParaDeletar != null)
        {
            _fracoesControllers.Remove(fracaoParaDeletar);
            Destroy(fracaoParaDeletar.gameObject);
        }

        AtualizaUI();
    }
}
