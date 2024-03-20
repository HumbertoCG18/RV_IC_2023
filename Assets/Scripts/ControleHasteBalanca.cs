using UnityEngine;

public class ControleHasteBalanca : MonoBehaviour
{
    public PlacaBalanca placaEsquerda;
    public PlacaBalanca placaDireita;
    public Transform pontoEsquerda;
    public Transform pontoDireita;

    private ConfigurableJoint m_ConfigurableJoint;

    void Start()
    {
        // Obt�m ou adiciona um ConfigurableJoint � haste
        m_ConfigurableJoint = GetComponent<ConfigurableJoint>();
        if (m_ConfigurableJoint == null)
        {
            m_ConfigurableJoint = gameObject.AddComponent<ConfigurableJoint>();
        }

        // Configura o eixo de rota��o como o eixo X
        m_ConfigurableJoint.axis = Vector3.right;


        // Configura o corpo conectado como o MeioHaste
        Rigidbody meioHasteRigidbody = GameObject.Find("MeioHaste").GetComponent<Rigidbody>();
        m_ConfigurableJoint.connectedBody = meioHasteRigidbody;
    }

    void FixedUpdate()
    {
        // Calcula a diferen�a de peso entre os pratos
        float diferencaPeso = Mathf.Abs(placaEsquerda.GetPesoPrato(PlacaBalanca.PratoSelecionado.Esquerda) - placaDireita.GetPesoPrato(PlacaBalanca.PratoSelecionado.Direita));

        // Calcula a altura do suporte com base na diferen�a de peso
        float alturaSuporte = Mathf.Clamp(0.1f * diferencaPeso, -0.5f, 0.5f); // Exemplo simples, ajuste conforme necess�rio

        // Move os pontos de suporte dos pratos para cima ou para baixo com base na diferen�a de peso
        pontoEsquerda.localPosition = new Vector3(0f, alturaSuporte, 0f);
        pontoDireita.localPosition = new Vector3(0f, alturaSuporte, 0f);

        // Define a rota��o dos pontos para zero para garantir que eles n�o sejam afetados pela rota��o da haste
        pontoEsquerda.localRotation = Quaternion.identity;
        pontoDireita.localRotation = Quaternion.identity;
    }

}
