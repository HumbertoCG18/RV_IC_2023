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
        // Configura ArticulationBody nos componentes necessários
        articulationBodyMeioBase = ConfigurarArticulationBody(meioBase, ArticulationJointType.FixedJoint);
        articulationBodyMeioHaste = ConfigurarArticulationBody(meioHaste, ArticulationJointType.RevoluteJoint);
        ConfigurarArticulationBody(pontoEsquerda, ArticulationJointType.FixedJoint);
        ConfigurarArticulationBody(pontoDireita, ArticulationJointType.FixedJoint);

        if (articulationBodyMeioHaste == null)
        {
            Debug.LogError("ArticulationBody do MeioHaste não encontrado.");
            return;
        }

        // Configura o eixo de rotação do MeioHaste
        articulationBodyMeioHaste.twistLock = ArticulationDofLock.LimitedMotion;
        articulationBodyMeioHaste.swingYLock = ArticulationDofLock.LockedMotion;
        articulationBodyMeioHaste.swingZLock = ArticulationDofLock.LockedMotion;
        articulationBodyMeioHaste.anchorRotation = Quaternion.Euler(0, 0, 0);

        // Configura os limites de rotação
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
            Debug.LogError("Transform fornecido é nulo.");
            return null;
        }

        ArticulationBody articulationBody = transform.gameObject.GetComponent<ArticulationBody>() ?? transform.gameObject.AddComponent<ArticulationBody>();
        articulationBody.jointType = jointType;
        return articulationBody;
    }

    void FixedUpdate()
    {
        // Verifica se as referências são válidas
        if (placaEsquerda == null)
        {
            Debug.LogError("placaEsquerda está ausente no ControleBalanca.");
            return;
        }

        if (placaDireita == null)
        {
            Debug.LogError("placaDireita está ausente no ControleBalanca.");
            return;
        }

        if (articulationBodyMeioHaste == null)
        {
            Debug.LogError("articulationBodyMeioHaste está ausente no ControleBalanca.");
            return;
        }

        if (articulationBodyMeioBase == null)
        {
            Debug.LogError("articulationBodyMeioBase está ausente no ControleBalanca.");
            return;
        }

        // Calcula a diferença de peso entre os pratos
        float diferencaPeso = placaEsquerda.GetPesoPrato(PlacaBalanca.PratoSelecionado.Esquerda) - placaDireita.GetPesoPrato(PlacaBalanca.PratoSelecionado.Direita);

        // Calcula o ângulo de rotação da haste com base na diferença de peso
        float anguloX = Mathf.Clamp(diferencaPeso, -45f, 45f);

        // Aplica a rotação na haste
        ArticulationDrive xDrive = articulationBodyMeioHaste.xDrive;
        xDrive.target = anguloX;
        articulationBodyMeioHaste.xDrive = xDrive;

        // Calcula a altura do suporte com base na rotação da haste
        float alturaSuporte = Mathf.Clamp(0.1f * diferencaPeso, -0.5f, 0.5f);

        // Move os pontos de suporte dos pratos para cima ou para baixo com base na diferença de peso e na rotação da haste
        if (pontoEsquerda != null)
        {
            pontoEsquerda.localPosition = new Vector3(0f, alturaSuporte, 0f);
        }
        else
        {
            Debug.LogError("PontoEsquerda não está definido.");
        }

        if (pontoDireita != null)
        {
            pontoDireita.localPosition = new Vector3(0f, alturaSuporte, 0f);
        }
        else
        {
            Debug.LogError("PontoDireita não está definido.");
        }
    }
}
