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
        // Obt�m ou adiciona um HingeJoint � haste
        m_HingeJoint = GetComponent<HingeJoint>();
        if (m_HingeJoint == null)
        {
            m_HingeJoint = gameObject.AddComponent<HingeJoint>();
        }

        // Configura o eixo de rota��o como o eixo X
        m_HingeJoint.axis = Vector3.right;

        // Obt�m o Rigidbody do MeioHaste apenas uma vez e armazena na vari�vel meioHasteRigidbody
        Rigidbody meioHasteRigidbody = GameObject.Find("MeioHaste").GetComponent<Rigidbody>();

        // Verifica se o Rigidbody foi encontrado
        if (meioHasteRigidbody == null)
        {
            Debug.LogError("Rigidbody do MeioHaste n�o encontrado.");
            return;
        }

        // Configura o corpo conectado como o MeioHaste
        m_HingeJoint.connectedBody = meioHasteRigidbody;

        // Configura os limites de rota��o para impedir movimentos indesejados
        JointLimits limits = m_HingeJoint.limits;
        limits.min = -90f;
        limits.max = 90f;
        m_HingeJoint.limits = limits;

        // Configura o motor do HingeJoint para permitir a rota��o controlada
        JointMotor motor = m_HingeJoint.motor;
        motor.force = 10f;
        motor.targetVelocity = 0f;
        motor.freeSpin = false;
        m_HingeJoint.motor = motor;
    }

    void FixedUpdate()
    {
        // Calcula a diferen�a de peso entre os pratos
        float diferencaPeso = Mathf.Abs(placaEsquerda.GetPesoPrato(PlacaBalanca.PratoSelecionado.Esquerda) - placaDireita.GetPesoPrato(PlacaBalanca.PratoSelecionado.Direita));

        // Calcula o �ngulo de rota��o da haste com base na diferen�a de peso
        float anguloX = Mathf.Clamp(-90f + diferencaPeso, -115f, -65f);

        // Aplica a rota��o na haste
        m_HingeJoint.transform.localRotation = Quaternion.Euler(anguloX, 0f, 0f);

        // Calcula a altura do suporte com base na rota��o da haste
        float alturaSuporte = Mathf.Clamp(0.1f * diferencaPeso, -0.5f, 0.5f); // Exemplo simples, ajuste conforme necess�rio

        // Move os pontos de suporte dos pratos para cima ou para baixo com base na diferen�a de peso e na rota��o da haste
        pontoEsquerda.localPosition = new Vector3(0f, alturaSuporte, 0f);
        pontoDireita.localPosition = new Vector3(0f, alturaSuporte, 0f);

        // Define a rota��o dos pontos para zero para garantir que eles n�o sejam afetados pela rota��o da haste
        pontoEsquerda.localRotation = Quaternion.identity;
        pontoDireita.localRotation = Quaternion.identity;
    }
}
