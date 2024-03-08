using UnityEngine;

public class BalancaCompleta : MonoBehaviour
{
    public GameObject pratoEsquerda;
    public GameObject pratoDireita;
    public GameObject haste;
    public GameObject meioHaste;
    public GameObject meioSuporte;
    public GameObject suporteConectadoEsquerda;
    public GameObject suporteConectadoDireita;
    public GameObject pontoEsquerda;
    public GameObject pontoDireita;

    private HingeJoint hingeHaste;

    void Start()
    {
        ConectarPratos();
        ConectarHaste();
        AdicionarJointHaste();
    }

    void ConectarPratos()
    {
        pratoEsquerda.transform.SetParent(suporteConectadoEsquerda.transform, false);
        suporteConectadoEsquerda.transform.SetParent(pontoEsquerda.transform, true);

        pratoDireita.transform.SetParent(suporteConectadoDireita.transform, false);
        suporteConectadoDireita.transform.SetParent(pontoDireita.transform, true);

        FixedJoint fixedJointPontoEsquerdaSuporteEsquerda = pontoEsquerda.AddComponent<FixedJoint>();
        fixedJointPontoEsquerdaSuporteEsquerda.connectedBody = suporteConectadoEsquerda.GetComponent<Rigidbody>();

        FixedJoint fixedJointPontoDireitaSuporteDireita = pontoDireita.AddComponent<FixedJoint>();
        fixedJointPontoDireitaSuporteDireita.connectedBody = suporteConectadoDireita.GetComponent<Rigidbody>();
    }

    void ConectarHaste()
    {
        // Remover a FixedJoint da haste do meio
        Destroy(meioHaste.GetComponent<FixedJoint>());

        // Adicionar HingeJoint no meio da haste
        hingeHaste = meioHaste.AddComponent<HingeJoint>();
        hingeHaste.connectedBody = meioSuporte.GetComponent<Rigidbody>();
        hingeHaste.anchor = Vector3.zero;
        hingeHaste.axis = Vector3.up;
        hingeHaste.useSpring = true;
        hingeHaste.spring = new JointSpring { spring = 0f, damper = 0f, targetPosition = 0f };
        hingeHaste.limits = new JointLimits { min = -90f, max = 90f };
    }

    void AdicionarJointHaste()
    {
        // Adicionar hinge joint da haste
        hingeHaste = haste.AddComponent<HingeJoint>();
        hingeHaste.connectedBody = meioHaste.GetComponent<Rigidbody>();
        hingeHaste.anchor = Vector3.zero;
        hingeHaste.axis = Vector3.up;
        hingeHaste.useSpring = true;
        hingeHaste.spring = new JointSpring { spring = 0f, damper = 0f, targetPosition = 0f };
        hingeHaste.limits = new JointLimits { min = -90f, max = 90f };
    }

    void Update()
    {
        // Calcular a diferença de peso entre os pratos
        float pesoPratoEsquerda = pratoEsquerda.GetComponent<PlacaBalanca>().pesoPrato;
        float pesoPratoDireita = pratoDireita.GetComponent<PlacaBalanca>().pesoPrato;
        float diferencaPeso = Mathf.Abs(pesoPratoEsquerda - pesoPratoDireita);

        // Usar a diferença de peso para ajustar o ângulo da haste
        float proporcao = Mathf.Clamp01(diferencaPeso / pratoEsquerda.GetComponent<PlacaBalanca>().pesoAtivarMax);
        float angulo = Mathf.Lerp(-90f, 90f, proporcao);
        JointSpring spring = hingeHaste.spring;
        spring.targetPosition = angulo;
        hingeHaste.spring = spring;
    }
}
