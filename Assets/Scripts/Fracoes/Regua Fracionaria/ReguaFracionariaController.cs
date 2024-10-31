using UnityEngine;

public class ReguaFracionariaController : MonoBehaviour
{
    [SerializeField] private GameObject _elementoReguaPrefab;
    [SerializeField] private Transform _containerElementos;
    [SerializeField] private float _tamanhoRegua = 0.5f;
    [SerializeField] private Fracao _fracao;

    private Color _corDosElementos = Color.green;

    public void AtualizaVisual()
    {
        CustomUtils.ClearChilds(_containerElementos);

        Vector3 escalaElemento = new Vector3(_tamanhoRegua / _fracao._denominador, _elementoReguaPrefab.transform.localScale.y, _elementoReguaPrefab.transform.localScale.z);
        Vector3 passo = new Vector3(escalaElemento.x, 0, 0);
        Vector3 posicaoInicial = new Vector3(_tamanhoRegua / -2f + escalaElemento.x/2f, 0, 0);

        for (int i = 0; i < _fracao._denominador; i++)
        {
            GameObject instance = Instantiate(_elementoReguaPrefab, _containerElementos);
            
            instance.transform.localPosition = posicaoInicial + passo * i;
            instance.transform.localScale = escalaElemento;

            ElementoReguaController elementController = instance.GetComponent<ElementoReguaController>();

            elementController.SetCores(_corDosElementos, Color.white);   
            elementController.SetActivate(i <  _fracao._numerador);
        }
    }

    public void SetCorDosElementos(Color cor)
    {
        _corDosElementos = cor;
    }

    public void SetFracao(Fracao novaFracao, float tamanhoDaRegua)
    {
        _fracao = novaFracao;
        _tamanhoRegua = tamanhoDaRegua;

        AtualizaVisual();
    }
}
