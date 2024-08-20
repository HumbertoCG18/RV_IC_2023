using UnityEngine;

public class ControleBalanca : MonoBehaviour
{
    public Transform pontoEsquerdaPosition;
    public Transform pontoDireitaPosition;
    public Transform meioHaste;
    public Transform meioBase;
    public Transform suporteEsquerda;
    public Transform suporteDireita;
    public Transform pratoEsquerda;
    public Transform pratoDireita;
    public Transform haste;

    private ArticulationBody meioHasteAB;
    private ArticulationBody meioBaseAB;

    void Start()
    {
        // Adicionar componentes necessários
        AdicionarComponentes();

        // Conectar MeioHaste ao MeioBase usando ArticulationBody
        ConectarMeioBase();
    }

    void Update()
    {
        // Sincronizar a posição dos suportes com os pontos de fixação
        AtualizarPosicaoSuportes();

        // Calcular a diferença de peso entre os pratos
        float pesoEsquerda = pratoEsquerda.GetComponent<PlacaBalanca>().GetPesoPrato(PlacaBalanca.PratoSelecionado.Esquerda);
        float pesoDireita = pratoDireita.GetComponent<PlacaBalanca>().GetPesoPrato(PlacaBalanca.PratoSelecionado.Direita);
        float diferencaPeso = pesoEsquerda - pesoDireita;

        // Ajustar o ângulo da haste com base na diferença de peso
        AjustarHaste(diferencaPeso);
    }

    void AdicionarComponentes()
    {
        meioHasteAB = AdicionarArticulationBody(meioHaste);
        meioBaseAB = AdicionarArticulationBody(meioBase, true);
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

    void ConectarMeioBase()
    {
        if (meioHasteAB != null && meioBaseAB != null)
        {
            meioHasteAB.jointType = ArticulationJointType.FixedJoint;

            ArticulationBody hasteAB = haste.gameObject.AddComponent<ArticulationBody>();
            hasteAB.jointType = ArticulationJointType.RevoluteJoint;
            hasteAB.anchorPosition = Vector3.zero;
            hasteAB.parentAnchorPosition = Vector3.zero;

            var drive = hasteAB.xDrive;
            drive.lowerLimit = -30f;
            drive.upperLimit = 30f;
            drive.stiffness = 500f;
            drive.damping = 100f;
            drive.forceLimit = float.MaxValue;
            hasteAB.xDrive = drive;

            hasteAB.twistLock = ArticulationDofLock.LimitedMotion;
            hasteAB.swingYLock = ArticulationDofLock.LockedMotion;
            hasteAB.swingZLock = ArticulationDofLock.LockedMotion;
        }
    }

    void AjustarHaste(float diferencaPeso)
    {
        float angulo = diferencaPeso * 5f;
        angulo = Mathf.Clamp(angulo, -30f, 30f);

        ArticulationBody hasteAB = haste.GetComponent<ArticulationBody>();
        var drive = hasteAB.xDrive;
        drive.target = -angulo;
        hasteAB.xDrive = drive;
    }

    void AtualizarPosicaoSuportes()
    {
        // Posicionar os suportes nas posições dos pontos de fixação
        suporteEsquerda.position = pontoEsquerdaPosition.position;
        suporteDireita.position = pontoDireitaPosition.position;

        // Manter os suportes sempre na orientação horizontal
        suporteEsquerda.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        suporteDireita.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);

        // Manter os pratos também sempre na orientação horizontal
        pratoEsquerda.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        pratoDireita.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
    }

}

