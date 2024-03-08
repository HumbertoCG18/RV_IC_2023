using UnityEngine;

public class BalancaCompleta : MonoBehaviour
{
    public GameObject pratoEsquerda;
    public GameObject pratoDireita;
    public GameObject suporteEsquerdo;
    public GameObject suporteDireito;
    public GameObject pontoEsquerda;
    public GameObject pontoDireita;
    public GameObject meioHaste;
    public GameObject meioBase;
    public GameObject haste;

    void Start()
    {
        ConectarPratoSuporte(pratoEsquerda, suporteEsquerdo, pontoEsquerda);
        ConectarPratoSuporte(pratoDireita, suporteDireito, pontoDireita);
        ConectarMeioHasteBase();
        ConectarMeioHasteHaste();
    }

    void ConectarPratoSuporte(GameObject prato, GameObject suporte, GameObject ponto)
    {
        Rigidbody pratoRB = AdicionarRigidbody(prato);
        Rigidbody suporteRB = AdicionarRigidbody(suporte);
        Rigidbody pontoRB = AdicionarRigidbody(ponto);

        // Verificar se os rigidbodies estão presentes
        if (pratoRB == null || suporteRB == null || pontoRB == null)
        {
            Debug.LogError("Rigidbody faltando em um dos objetos.");
            return;
        }

        // Conectar o prato ao suporte
        FixedJoint jointPratoSuporte = prato.AddComponent<FixedJoint>();
        jointPratoSuporte.connectedBody = suporteRB;

        // Conectar o suporte ao ponto
        FixedJoint jointSuportePonto = suporte.AddComponent<FixedJoint>();
        jointSuportePonto.connectedBody = pontoRB;

        // Conectar o ponto à haste
        FixedJoint jointPontoHaste = ponto.AddComponent<FixedJoint>();
        jointPontoHaste.connectedBody = haste.GetComponent<Rigidbody>();
    }

    void ConectarMeioHasteBase()
    {
        Rigidbody meioHasteRB = meioHaste.GetComponent<Rigidbody>();
        Rigidbody meioBaseRB = meioBase.GetComponent<Rigidbody>();

        // Adicione os rigidbodies se não estiverem presentes
        if (meioHasteRB == null)
            meioHasteRB = meioHaste.AddComponent<Rigidbody>();
        if (meioBaseRB == null)
            meioBaseRB = meioBase.AddComponent<Rigidbody>();

        // Configuração opcional dos rigidbodies
        meioHasteRB.isKinematic = true; // MeioHaste é estático
        meioBaseRB.isKinematic = true; // MeioBase é estático

        // Conecte o MeioHaste ao MeioBase com um HingeJoint
        HingeJoint jointMeioHasteBase = meioBase.AddComponent<HingeJoint>();
        jointMeioHasteBase.connectedBody = meioHasteRB;
        jointMeioHasteBase.axis = new Vector3(0, 0, 1); // Eixo de rotação na direção z (eixo vertical)
        jointMeioHasteBase.useLimits = true;
        JointLimits limits = new JointLimits();
        limits.min = 0; // Ângulo mínimo
        limits.max = 180; // Ângulo máximo (obtuso)
        jointMeioHasteBase.limits = limits;
    }

    void ConectarMeioHasteHaste()
    {
        Rigidbody hasteRB = haste.GetComponent<Rigidbody>();
        Rigidbody meioHasteRB = meioHaste.GetComponent<Rigidbody>();

        // Verifique se os rigidbodies estão presentes
        if (hasteRB == null)
        {
            hasteRB = haste.AddComponent<Rigidbody>();
            hasteRB.isKinematic = true; // Configuração opcional do rigidbody
        }
        if (meioHasteRB == null)
        {
            meioHasteRB = meioHaste.AddComponent<Rigidbody>();
            meioHasteRB.isKinematic = true; // Configuração opcional do rigidbody
        }

        // Conectar o meio da haste à haste com uma FixedJoint
        FixedJoint jointMeioHasteHaste = haste.AddComponent<FixedJoint>();
        jointMeioHasteHaste.connectedBody = meioHasteRB;
    }

    Rigidbody AdicionarRigidbody(GameObject objeto)
    {
        Rigidbody rb = objeto.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = objeto.AddComponent<Rigidbody>();
            rb.isKinematic = true; // Definir como kinematic para evitar que os objetos caiam devido à gravidade
        }
        return rb;
    }
}
