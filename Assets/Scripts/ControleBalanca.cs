using UnityEngine;

public class ControleBalanca : MonoBehaviour
{
    public Transform pontoEsquerda;
    public Transform pontoDireita;
    public Transform meioHaste;
    public Transform meioBase;
    public Transform suporteEsquerda;
    public Transform suporteDireita;
    public Transform pratoEsquerda;
    public Transform pratoDireita;

    private ArticulationBody meioHasteAB;
    private ArticulationBody meioBaseAB;
    private ArticulationBody pontoEsquerdaAB;
    private ArticulationBody pontoDireitaAB;
    private Rigidbody suporteEsquerdaRB;
    private Rigidbody suporteDireitaRB;
    private Rigidbody pratoEsquerdaRB;
    private Rigidbody pratoDireitaRB;

    void Start()
    {
        // Alinhar os pivots de MeioHaste e MeioBase
        AlinharPivots();

        // Adicionar componentes necessários
        AdicionarComponentes();

        // Conectar os suportes aos pontos da haste usando FixedJoint
        ConectarSuportesAosPontosVirtuais();

        // Conectar os pratos aos suportes
        ConectarPratosAosSuportes();

        // Conectar MeioHaste ao MeioBase usando ArticulationBody
        ConectarMeioBase();

        // Configurar propriedades dos Rigidbody dos pratos
        ConfigurarPratos();
    }

    void Update()
    {
        // Calcular a diferença de peso entre os pratos
        float pesoEsquerda = pratoEsquerda.GetComponentInChildren<PlacaBalanca>().GetPesoPrato(PlacaBalanca.PratoSelecionado.Esquerda);
        float pesoDireita = pratoDireita.GetComponentInChildren<PlacaBalanca>().GetPesoPrato(PlacaBalanca.PratoSelecionado.Direita);
        float diferencaPeso = pesoDireita - pesoEsquerda;

        // Ajustar o ângulo da haste com base na diferença de peso
        AjustarHaste(diferencaPeso);
    }

    void AlinharPivots()
    {
        meioHaste.position = meioBase.position;
    }

    void AdicionarComponentes()
    {
        meioHasteAB = AdicionarArticulationBody(meioHaste);
        meioBaseAB = AdicionarArticulationBody(meioBase, true);
        pontoEsquerdaAB = AdicionarArticulationBody(pontoEsquerda);
        pontoDireitaAB = AdicionarArticulationBody(pontoDireita);

        suporteEsquerdaRB = AdicionarRigidbody(suporteEsquerda);
        suporteDireitaRB = AdicionarRigidbody(suporteDireita);
        pratoEsquerdaRB = AdicionarRigidbody(pratoEsquerda);
        pratoDireitaRB = AdicionarRigidbody(pratoDireita);
    }

    ArticulationBody AdicionarArticulationBody(Transform obj, bool isImmovable = false)
    {
        ArticulationBody ab = obj.GetComponent<ArticulationBody>();
        if (ab == null)
        {
            ab = obj.gameObject.AddComponent<ArticulationBody>();
        }

        if (isImmovable)
        {
            ab.immovable = true;
            ab.anchorPosition = Vector3.zero;
            ab.parentAnchorPosition = Vector3.zero;
        }

        return ab;
    }

    Rigidbody AdicionarRigidbody(Transform obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = obj.gameObject.AddComponent<Rigidbody>();
        }

        // Garantir que o Collider exista
        if (obj.GetComponent<Collider>() == null)
        {
            obj.gameObject.AddComponent<BoxCollider>();
        }

        return rb;
    }

    void ConectarSuportesAosPontosVirtuais()
    {
        if (suporteDireitaRB != null && pontoDireitaAB != null)
        {
            FixedJoint jointDireita = suporteDireitaRB.gameObject.AddComponent<FixedJoint>();
            jointDireita.connectedArticulationBody = pontoDireitaAB;
        }

        if (suporteEsquerdaRB != null && pontoEsquerdaAB != null)
        {
            FixedJoint jointEsquerda = suporteEsquerdaRB.gameObject.AddComponent<FixedJoint>();
            jointEsquerda.connectedArticulationBody = pontoEsquerdaAB;
        }
    }

    void ConectarPratosAosSuportes()
    {
        if (pratoDireitaRB != null && suporteDireitaRB != null)
        {
            FixedJoint jointPratoDireita = pratoDireitaRB.gameObject.AddComponent<FixedJoint>();
            jointPratoDireita.connectedBody = suporteDireitaRB;  // Conectar com o Rigidbody do suporte direito
        }

        if (pratoEsquerdaRB != null && suporteEsquerdaRB != null)
        {
            FixedJoint jointPratoEsquerda = pratoEsquerdaRB.gameObject.AddComponent<FixedJoint>();
            jointPratoEsquerda.connectedBody = suporteEsquerdaRB;  // Conectar com o Rigidbody do suporte esquerdo
        }
    }

    void ConectarMeioBase()
    {
        if (meioHasteAB != null && meioBaseAB != null)
        {
            meioHasteAB.jointType = ArticulationJointType.RevoluteJoint;
            meioHasteAB.anchorPosition = Vector3.zero;
            meioHasteAB.parentAnchorPosition = Vector3.zero;

            var drive = meioHasteAB.zDrive;
            drive.lowerLimit = -30f;
            drive.upperLimit = 30f;
            drive.stiffness = 200f;
            drive.damping = 100f;
            drive.forceLimit = float.MaxValue;
            meioHasteAB.zDrive = drive;

            meioHasteAB.twistLock = ArticulationDofLock.LockedMotion;
            meioHasteAB.swingYLock = ArticulationDofLock.LockedMotion;
            meioHasteAB.swingZLock = ArticulationDofLock.LimitedMotion;
        }
    }

    void ConfigurarPratos()
    {
        pratoEsquerdaRB.useGravity = true;
        pratoDireitaRB.useGravity = true;
    }

    void AjustarHaste(float diferencaPeso)
    {
        float angulo = diferencaPeso * 5f;
        angulo = Mathf.Clamp(angulo, -30f, 30f);

        var drive = meioHasteAB.zDrive;
        drive.target = angulo;
        meioHasteAB.zDrive = drive;
    }
}
