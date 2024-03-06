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

    private HingeJoint hingePratoEsquerda;
    private HingeJoint hingePratoDireita;
    private FixedJoint fixedSuporteEsquerdo;
    private FixedJoint fixedSuporteDireito;
    private HingeJoint hingeHaste;

    void Start()
    {
        // Conectar os pratos aos seus respectivos suportes e pontos na haste
        ConectarPratos();

        // Conectar a haste ao meio suporte
        ConectarHaste();

        // Adicionar as joints aos pratos e suportes
        AdicionarJoints();
    }

    void ConectarPratos()
    {
        // Conectar prato esquerda ao seu suporte e ponto na haste
        pratoEsquerda.transform.SetParent(suporteConectadoEsquerda.transform, false);
        suporteConectadoEsquerda.transform.SetParent(pontoEsquerda.transform, true);

        // Conectar prato direita ao seu suporte e ponto na haste
        pratoDireita.transform.SetParent(suporteConectadoDireita.transform, false);
        suporteConectadoDireita.transform.SetParent(pontoDireita.transform, true);

        // Conectar o prato esquerdo ao ponto esquerdo
        FixedJoint fixedJointPratoEsquerda = pratoEsquerda.AddComponent<FixedJoint>();
        fixedJointPratoEsquerda.connectedBody = pontoEsquerda.GetComponent<Rigidbody>();

        // Conectar o prato direito ao ponto direito
        FixedJoint fixedJointPratoDireita = pratoDireita.AddComponent<FixedJoint>();
        fixedJointPratoDireita.connectedBody = pontoDireita.GetComponent<Rigidbody>();
    }


    void ConectarHaste()
    {
        // Conectar o meio da haste ao meio do suporte
        FixedJoint fixedJoint = meioHaste.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = meioSuporte.GetComponent<Rigidbody>();
    }

    void AdicionarJoints()
    {
        // Adicionar hinge joint ao prato esquerda
        hingePratoEsquerda = pratoEsquerda.AddComponent<HingeJoint>();
        hingePratoEsquerda.connectedBody = pratoEsquerda.transform.parent.GetComponent<Rigidbody>();
        hingePratoEsquerda.useSpring = true;
        hingePratoEsquerda.spring = new JointSpring { spring = 0f };

        // Adicionar hinge joint ao prato direita
        hingePratoDireita = pratoDireita.AddComponent<HingeJoint>();
        hingePratoDireita.connectedBody = pratoDireita.transform.parent.GetComponent<Rigidbody>();
        hingePratoDireita.useSpring = true;
        hingePratoDireita.spring = new JointSpring { spring = 0f };

        // Adicionar fixed joint ao suporte esquerdo
        fixedSuporteEsquerdo = suporteConectadoEsquerda.AddComponent<FixedJoint>();
        fixedSuporteEsquerdo.connectedBody = suporteConectadoEsquerda.transform.parent.GetComponent<Rigidbody>();

        // Adicionar fixed joint ao suporte direito
        fixedSuporteDireito = suporteConectadoDireita.AddComponent<FixedJoint>();
        fixedSuporteDireito.connectedBody = suporteConectadoDireita.transform.parent.GetComponent<Rigidbody>();

        // Configurar hinge joint da haste
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
