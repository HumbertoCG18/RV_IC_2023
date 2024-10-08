//Fazer de alguma maneira, que o prato e o suporte sempre fiquem a 90 graus, mesmo quando � aplicada uma angula��o a haste, pois, caso isso n�o ocorra, o peso cair� do prato
 // Pode se pensar na possibilidade do uso de attatchs points? (N�o seria o ideal, mas funcionaria)
 // Fazer com que o Box Collider siga o objeto na cena; 

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
    private ArticulationBody hasteAB;

    void Start()
    {
        // Adicionar componentes necess�rios
        AdicionarComponentes();

        // Conectar MeioHaste ao MeioBase usando ArticulationBody
        ConectarMeioBase();
    }

    void Update()
    {

        AtualizarPosicaoSuportesEPratos();

        // Calcular a diferen�a de peso entre os pratos
        float pesoEsquerda = pratoEsquerda.GetComponent<PlacaBalanca>().GetPesoPrato(PlacaBalanca.PratoSelecionado.Esquerda);
        float pesoDireita = pratoDireita.GetComponent<PlacaBalanca>().GetPesoPrato(PlacaBalanca.PratoSelecionado.Direita);
        float diferencaPeso = pesoDireita - pesoEsquerda;

        // Ajustar o �ngulo da haste com base na diferen�a de peso
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
        // Ajuste a rota��o da haste conforme a diferen�a de peso
        float angulo = diferencaPeso * 5f;
        angulo = Mathf.Clamp(angulo, -30f, 30f);

        var drive = hasteAB.xDrive;
        drive.target = -angulo;  // Invers�o do �ngulo para corrigir a dire��o da rota��o
        hasteAB.xDrive = drive;
    }


    void AtualizarPosicaoSuportesEPratos()
    {
        // Atualizar posi��o dos suportes para coincidir com os pontos virtuais
        suporteEsquerda.SetPositionAndRotation(pontoEsquerdaPosition.position, Quaternion.identity);
        suporteDireita.SetPositionAndRotation(pontoDireitaPosition.position, Quaternion.identity);

        // Agora garantir que os pratos fiquem sempre alinhados horizontalmente
        pratoEsquerda.SetPositionAndRotation(suporteEsquerda.position, Quaternion.LookRotation(Vector3.up, Vector3.forward));
        pratoDireita.SetPositionAndRotation(suporteDireita.position, Quaternion.LookRotation(Vector3.up, Vector3.forward));
    }

}
