using UnityEngine;

public class ControleHasteBalanca : MonoBehaviour
{
    public Rigidbody pontoEsquerda;
    public Rigidbody pontoDireita;
    public HingeJoint hingeJoint;
    public float maxAngle = 180f;

    void Start()
    {
        if (hingeJoint == null || pontoEsquerda == null || pontoDireita == null)
        {
            Debug.LogError("Certifique-se de atribuir os Rigidbody e o HingeJoint no inspector.");
            return;
        }

        // Configura o ponto de ancoragem esquerdo e direito do HingeJoint
        JointSpring hingeSpring = hingeJoint.spring;
        hingeSpring.spring = 1000f; // Ajuste conforme necessário
        hingeJoint.spring = hingeSpring;

        // Configura a rotação inicial da haste com base nos pesos iniciais dos pratos
        AtualizarRotação();
    }

    void Update()
    {
        // Atualiza a rotação da haste com base na diferença de peso entre os pratos
        AtualizarRotação();
    }

    void AtualizarRotação()
    {
        // Busca automaticamente os pratos nas crianças deste objeto
        PlacaBalanca placaEsquerda = transform.Find("X(Prato Esquerdo)").GetComponent<PlacaBalanca>();
        PlacaBalanca placaDireita = transform.Find("Y(Prato Direita)").GetComponent<PlacaBalanca>();

        // Obtém os pesos dos pratos esquerdo e direito
        float pesoPratoEsquerda = placaEsquerda.GetPesoPrato(PlacaBalanca.PratoSelecionado.Esquerda);
        float pesoPratoDireita = placaDireita.GetPesoPrato(PlacaBalanca.PratoSelecionado.Direita);

        // Calcula a diferença de peso entre os pratos
        float diferencaPeso = pesoPratoEsquerda - pesoPratoDireita;

        // Calcula o ângulo com base na diferença de peso
        float novoAngulo = Mathf.Clamp(diferencaPeso, -1f, 1f) * maxAngle;

        // Define o ângulo no HingeJoint para controlar a rotação da haste
        JointLimits limits = hingeJoint.limits;
        limits.min = novoAngulo;
        limits.max = novoAngulo;
        hingeJoint.limits = limits;

        // Ativa o motor do HingeJoint para fazer a haste se mover para o novo ângulo
        JointMotor motor = hingeJoint.motor;
        motor.force = 1000f; // Ajuste conforme necessário
        motor.targetVelocity = 0f; // Mantém a velocidade constante
        hingeJoint.useMotor = true;
        hingeJoint.motor = motor;

        // Exibe os pesos dos pratos no console
        Debug.Log("Peso do prato esquerdo: " + pesoPratoEsquerda);
        Debug.Log("Peso do prato direito: " + pesoPratoDireita);
    }
}
