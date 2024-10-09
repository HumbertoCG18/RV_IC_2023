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
    public Transform haste;

    private ArticulationBody meioHasteAB;
    private ArticulationBody meioBaseAB;
    private ArticulationBody hasteAB;

    void Start()
    {
        // Adicionar componentes necessários
        AdicionarComponentes();

        // Conectar MeioHaste ao MeioBase usando ArticulationBody
        ConectarMeioBase();
    }

    void Update()
    {
        // Atualizar posições e rotações dos suportes e pratos
        AtualizarPosicaoSuportesEPratos();

        // Calcular a diferença de peso entre os pratos
        float pesoEsquerda = pontoEsquerda.GetComponent<PlacaBalanca>().GetPesoPrato(PlacaBalanca.PratoSelecionado.Esquerda);
        float pesoDireita = pontoDireita.GetComponent<PlacaBalanca>().GetPesoPrato(PlacaBalanca.PratoSelecionado.Direita);
        float diferencaPeso = pesoDireita - pesoEsquerda;

        // Ajustar o ângulo da haste com base na diferença de peso
        AjustarHaste(diferencaPeso);
    }

    void AdicionarComponentes()
    {
        meioHasteAB = AdicionarArticulationBody(meioHaste);
        meioBaseAB = AdicionarArticulationBody(meioBase, true);
        hasteAB = AdicionarArticulationBody(haste);
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
        // Ajuste a rotação da haste conforme a diferença de peso
        float angulo = diferencaPeso * 5f;
        angulo = Mathf.Clamp(angulo, -30f, 30f);

        var drive = hasteAB.xDrive;
        drive.target = -angulo;  // Inversão do ângulo para corrigir a direção da rotação
        hasteAB.xDrive = drive;
    }

    void AtualizarPosicaoSuportesEPratos()
    {
        // Atualizar posições dos suportes com base na posição e rotação da haste
        Vector3 esquerdaPosicao = haste.TransformPoint(haste.InverseTransformPoint(pontoEsquerda.position));
        Vector3 direitaPosicao = haste.TransformPoint(haste.InverseTransformPoint(pontoDireita.position));

        suporteEsquerda.position = esquerdaPosicao;
        suporteDireita.position = direitaPosicao;

        // Manter a rotação dos suportes em (0, 0, 0)
        suporteEsquerda.rotation = Quaternion.Euler(0f, 0f, 0f);
        suporteDireita.rotation = Quaternion.Euler(0f, 0f, 0f);

        // Atualizar posições dos pratos para coincidir com os suportes
        pratoEsquerda.position = suporteEsquerda.position;
        pratoDireita.position = suporteDireita.position;

        // Definir a rotação dos pratos em (-90, 0, 0)
        pratoEsquerda.rotation = Quaternion.Euler(-90f, 0f, 0f);
        pratoDireita.rotation = Quaternion.Euler(-90f, 0f, 0f);

        // Atualizar BoxColliders para seguir os pratos (se necessário)
        AtualizarBoxColliders(pratoEsquerda);
        AtualizarBoxColliders(pratoDireita);
    }

    void AtualizarBoxColliders(Transform prato)
    {
        // Encontrar todos os BoxColliders no prato e atualizá-los
        BoxCollider[] colliders = prato.GetComponentsInChildren<BoxCollider>();
        foreach (BoxCollider collider in colliders)
        {
            collider.transform.position = prato.position;
            collider.transform.rotation = prato.rotation;
        }
    }
}
