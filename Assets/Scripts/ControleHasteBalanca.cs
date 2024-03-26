using UnityEngine;

public class ControleHasteBalanca : MonoBehaviour
{
    public PlacaBalanca placaEsquerda;
    public PlacaBalanca placaDireita;
    public Transform pontoEsquerda;
    public Transform pontoDireita;

    private HingeJoint m_HingeJoint;

    void Start()
    {
        // Obtém ou adiciona um HingeJoint à haste
        m_HingeJoint = GetComponent<HingeJoint>();
        if (m_HingeJoint == null)
        {
            m_HingeJoint = gameObject.AddComponent<HingeJoint>();
        }

        // Configura o eixo de rotação como o eixo X
        m_HingeJoint.axis = Vector3.right;

        // Obtém o Rigidbody do MeioHaste apenas uma vez e armazena na variável meioHasteRigidbody
        Rigidbody meioHasteRigidbody = GameObject.Find("MeioHaste").GetComponent<Rigidbody>();

        // Verifica se o Rigidbody foi encontrado
        if (meioHasteRigidbody == null)
        {
            Debug.LogError("Rigidbody do MeioHaste não encontrado.");
            return;
        }

        // Configura o corpo conectado como o MeioHaste
        m_HingeJoint.connectedBody = meioHasteRigidbody;

        // Configura os limites de rotação para impedir movimentos indesejados
        JointLimits limits = m_HingeJoint.limits;
        limits.min = -90f;
        limits.max = 90f;
        m_HingeJoint.limits = limits;

        // Configura o motor do HingeJoint para permitir a rotação controlada
        JointMotor motor = m_HingeJoint.motor;
        motor.force = 10f;
        motor.targetVelocity = 0f;
        motor.freeSpin = false;
        m_HingeJoint.motor = motor;
    }

    void FixedUpdate()
    {
        // Calcula a diferença de peso entre os pratos
        float diferencaPeso = Mathf.Abs(placaEsquerda.GetPesoPrato(PlacaBalanca.PratoSelecionado.Esquerda) - placaDireita.GetPesoPrato(PlacaBalanca.PratoSelecionado.Direita));

        // Calcula o ângulo de rotação da haste com base na diferença de peso
        float anguloX = Mathf.Clamp(-90f + diferencaPeso, -115f, -65f);

        // Aplica a rotação na haste
        m_HingeJoint.transform.localRotation = Quaternion.Euler(anguloX, 0f, 0f);

        // Calcula a altura do suporte com base na rotação da haste
        float alturaSuporte = Mathf.Clamp(0.1f * diferencaPeso, -0.5f, 0.5f); // Exemplo simples, ajuste conforme necessário

        // Move os pontos de suporte dos pratos para cima ou para baixo com base na diferença de peso e na rotação da haste
        pontoEsquerda.localPosition = new Vector3(0f, alturaSuporte, 0f);
        pontoDireita.localPosition = new Vector3(0f, alturaSuporte, 0f);

        // Define a rotação dos pontos para zero para garantir que eles não sejam afetados pela rotação da haste
        pontoEsquerda.localRotation = Quaternion.identity;
        pontoDireita.localRotation = Quaternion.identity;
    }
}
