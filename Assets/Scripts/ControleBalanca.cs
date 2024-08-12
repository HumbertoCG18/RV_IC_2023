using UnityEngine;

public class ControleBalanca : MonoBehaviour
{
    public Transform pontoEsquerda;
    public Transform pontoDireita;
    public Transform meioHaste;
    public Transform meioBase;
    public Transform suporteEsquerda;
    public Transform suporteDireita;

    private ArticulationBody meioHasteAB;
    private ArticulationBody meioBaseAB;
    private ArticulationBody pontoEsquerdaAB;
    private ArticulationBody pontoDireitaAB;
    private Rigidbody suporteEsquerdaRB;
    private Rigidbody suporteDireitaRB;

    void Start()
    {
        // Adicionar ArticulationBody aos componentes necessários
        AdicionarComponentes();

        // Conectar os suportes aos pontos da haste usando FixedJoint
        ConectarSuportesAosPontosVirtuais();

        // Conectar MeioHaste ao MeioBase usando ArticulationBody
        ConectarMeioBase();
    }

    void Update()
    {
        // Calcular a diferença de peso entre os pratos
        float pesoEsquerda = suporteEsquerda.GetComponentInChildren<PlacaBalanca>().GetPesoPrato(PlacaBalanca.PratoSelecionado.Esquerda);
        float pesoDireita = suporteDireita.GetComponentInChildren<PlacaBalanca>().GetPesoPrato(PlacaBalanca.PratoSelecionado.Direita);
        float diferencaPeso = pesoDireita - pesoEsquerda;

        // Ajustar o ângulo da haste com base na diferença de peso
        AjustarHaste(diferencaPeso);
    }

    void AdicionarComponentes()
    {
        // Adicionar ArticulationBody aos componentes MeioHaste, MeioBase, PontoEsquerda e PontoDireita
        meioHasteAB = AdicionarArticulationBody(meioHaste);
        meioBaseAB = AdicionarArticulationBody(meioBase, true);
        pontoEsquerdaAB = AdicionarArticulationBody(pontoEsquerda);
        pontoDireitaAB = AdicionarArticulationBody(pontoDireita);

        // Adicionar Rigidbody aos suportes
        suporteEsquerdaRB = AdicionarRigidbody(suporteEsquerda);
        suporteDireitaRB = AdicionarRigidbody(suporteDireita);
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
        return rb;
    }

    void ConectarSuportesAosPontosVirtuais()
    {
        if (suporteDireitaRB != null && pontoDireitaAB != null)
        {
            FixedJoint jointDireita = suporteDireitaRB.gameObject.AddComponent<FixedJoint>();
            jointDireita.connectedBody = pontoDireitaAB.GetComponent<Rigidbody>();
        }

        if (suporteEsquerdaRB != null && pontoEsquerdaAB != null)
        {
            FixedJoint jointEsquerda = suporteEsquerdaRB.gameObject.AddComponent<FixedJoint>();
            jointEsquerda.connectedBody = pontoEsquerdaAB.GetComponent<Rigidbody>();
        }
    }

    void ConectarMeioBase()
    {
        if (meioHasteAB != null && meioBaseAB != null)
        {
            meioHasteAB.jointType = ArticulationJointType.RevoluteJoint;
            meioHasteAB.anchorPosition = Vector3.zero;
            meioHasteAB.parentAnchorPosition = Vector3.zero;

            var drive = meioHasteAB.zDrive;  // Usar zDrive para rotação no eixo Z
            drive.lowerLimit = -30f;  // Ajuste para um valor mais suave
            drive.upperLimit = 30f;
            drive.stiffness = 500f;  // Reduza a rigidez para um movimento mais fluido
            drive.damping = 100f;
            drive.forceLimit = float.MaxValue;
            meioHasteAB.zDrive = drive;

            meioHasteAB.twistLock = ArticulationDofLock.LockedMotion;
            meioHasteAB.swingYLock = ArticulationDofLock.LockedMotion;
            meioHasteAB.swingZLock = ArticulationDofLock.LimitedMotion;
        }
    }

    void AjustarHaste(float diferencaPeso)
    {
        float angulo = diferencaPeso * 5f;  // Ajuste o fator de multiplicação para controlar a sensibilidade
        angulo = Mathf.Clamp(angulo, -30f, 30f);  // Limitar o movimento dentro de um intervalo seguro

        var drive = meioHasteAB.zDrive;  // Aplicar o ângulo no eixo Z
        drive.target = angulo;
        meioHasteAB.zDrive = drive;
    }
}
