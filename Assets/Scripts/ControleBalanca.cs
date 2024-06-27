using UnityEngine;

public class ControleBalanca : MonoBehaviour
{
    public PlacaBalanca placaEsquerda;
    public PlacaBalanca placaDireita;
    public Transform pontoEsquerda;
    public Transform pontoDireita;
    public Transform meioHaste;
    public Transform meioBase;

    private ArticulationBody articulationBodyMeioHaste;
    private ArticulationBody articulationBodyMeioBase;

    void Start()
    {
        // Configura ArticulationBody nos componentes necess�rios
        articulationBodyMeioBase = ConfigurarArticulationBody(meioBase, ArticulationJointType.FixedJoint);
        articulationBodyMeioHaste = ConfigurarArticulationBody(meioHaste, ArticulationJointType.RevoluteJoint);
        ConfigurarArticulationBody(pontoEsquerda, ArticulationJointType.FixedJoint);
        ConfigurarArticulationBody(pontoDireita, ArticulationJointType.FixedJoint);

        if (articulationBodyMeioHaste == null)
        {
            Debug.LogError("ArticulationBody do MeioHaste n�o encontrado.");
            return;
        }

        // Configura o eixo de rota��o do MeioHaste
        articulationBodyMeioHaste.twistLock = ArticulationDofLock.LimitedMotion;
        articulationBodyMeioHaste.swingYLock = ArticulationDofLock.LockedMotion;
        articulationBodyMeioHaste.swingZLock = ArticulationDofLock.LockedMotion;
        articulationBodyMeioHaste.anchorRotation = Quaternion.Euler(0, 0, 0);

        // Configura os limites de rota��o
        ArticulationDrive xDrive = articulationBodyMeioHaste.xDrive;
        xDrive.lowerLimit = -45.0f;
        xDrive.upperLimit = 45.0f;
        xDrive.stiffness = 10000;
        xDrive.damping = 100;
        xDrive.forceLimit = 1000;
        articulationBodyMeioHaste.xDrive = xDrive;
    }

    ArticulationBody ConfigurarArticulationBody(Transform transform, ArticulationJointType jointType)
    {
        if (transform == null)
        {
            Debug.LogError("Transform fornecido � nulo.");
            return null;
        }

        ArticulationBody articulationBody = transform.gameObject.GetComponent<ArticulationBody>() ?? transform.gameObject.AddComponent<ArticulationBody>();
        articulationBody.jointType = jointType;
        return articulationBody;
    }

    void FixedUpdate()
    {
        // Verifica se as refer�ncias s�o v�lidas
        if (placaEsquerda == null)
        {
            Debug.LogError("placaEsquerda est� ausente no ControleBalanca.");
            return;
        }

        if (placaDireita == null)
        {
            Debug.LogError("placaDireita est� ausente no ControleBalanca.");
            return;
        }

        if (articulationBodyMeioHaste == null)
        {
            Debug.LogError("articulationBodyMeioHaste est� ausente no ControleBalanca.");
            return;
        }

        if (articulationBodyMeioBase == null)
        {
            Debug.LogError("articulationBodyMeioBase est� ausente no ControleBalanca.");
            return;
        }

        // Calcula a diferen�a de peso entre os pratos
        float diferencaPeso = placaEsquerda.GetPesoPrato(PlacaBalanca.PratoSelecionado.Esquerda) - placaDireita.GetPesoPrato(PlacaBalanca.PratoSelecionado.Direita);

        // Calcula o �ngulo de rota��o da haste com base na diferen�a de peso
        float anguloX = Mathf.Clamp(diferencaPeso, -45f, 45f);

        // Aplica a rota��o na haste
        ArticulationDrive xDrive = articulationBodyMeioHaste.xDrive;
        xDrive.target = anguloX;
        articulationBodyMeioHaste.xDrive = xDrive;

        // Calcula a altura do suporte com base na rota��o da haste
        float alturaSuporte = Mathf.Clamp(0.1f * diferencaPeso, -0.5f, 0.5f);

        // Move os pontos de suporte dos pratos para cima ou para baixo com base na diferen�a de peso e na rota��o da haste
        if (pontoEsquerda != null)
        {
            pontoEsquerda.localPosition = new Vector3(0f, alturaSuporte, 0f);
        }
        else
        {
            Debug.LogError("PontoEsquerda n�o est� definido.");
        }

        if (pontoDireita != null)
        {
            pontoDireita.localPosition = new Vector3(0f, alturaSuporte, 0f);
        }
        else
        {
            Debug.LogError("PontoDireita n�o est� definido.");
        }
    }
}
