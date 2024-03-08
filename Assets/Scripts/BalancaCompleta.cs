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

        // Verificar se os rigidbodies est�o presentes
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

        // Conectar o ponto � haste
        FixedJoint jointPontoHaste = ponto.AddComponent<FixedJoint>();
        jointPontoHaste.connectedBody = haste.GetComponent<Rigidbody>();
    }

    void ConectarMeioHasteBase()
    {
        Rigidbody meioHasteRB = meioHaste.GetComponent<Rigidbody>();
        Rigidbody meioBaseRB = meioBase.GetComponent<Rigidbody>();

        // Adicione os rigidbodies se n�o estiverem presentes
        if (meioHasteRB == null)
            meioHasteRB = meioHaste.AddComponent<Rigidbody>();
        if (meioBaseRB == null)
            meioBaseRB = meioBase.AddComponent<Rigidbody>();

        // Configura��o opcional dos rigidbodies
        meioHasteRB.isKinematic = true; // MeioHaste � est�tico
        meioBaseRB.isKinematic = true; // MeioBase � est�tico

        // Conecte o MeioHaste ao MeioBase com um HingeJoint
        HingeJoint jointMeioHasteBase = meioBase.AddComponent<HingeJoint>();
        jointMeioHasteBase.connectedBody = meioHasteRB;
        jointMeioHasteBase.axis = new Vector3(0, 0, 1); // Eixo de rota��o na dire��o z (eixo vertical)
        jointMeioHasteBase.useLimits = true;
        JointLimits limits = new JointLimits();
        limits.min = 0; // �ngulo m�nimo
        limits.max = 180; // �ngulo m�ximo (obtuso)
        jointMeioHasteBase.limits = limits;
    }

    void ConectarMeioHasteHaste()
    {
        Rigidbody hasteRB = haste.GetComponent<Rigidbody>();
        Rigidbody meioHasteRB = meioHaste.GetComponent<Rigidbody>();

        // Verifique se os rigidbodies est�o presentes
        if (hasteRB == null)
        {
            hasteRB = haste.AddComponent<Rigidbody>();
            hasteRB.isKinematic = true; // Configura��o opcional do rigidbody
        }
        if (meioHasteRB == null)
        {
            meioHasteRB = meioHaste.AddComponent<Rigidbody>();
            meioHasteRB.isKinematic = true; // Configura��o opcional do rigidbody
        }

        // Conectar o meio da haste � haste com uma FixedJoint
        FixedJoint jointMeioHasteHaste = haste.AddComponent<FixedJoint>();
        jointMeioHasteHaste.connectedBody = meioHasteRB;
    }

    Rigidbody AdicionarRigidbody(GameObject objeto)
    {
        Rigidbody rb = objeto.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = objeto.AddComponent<Rigidbody>();
            rb.isKinematic = true; // Definir como kinematic para evitar que os objetos caiam devido � gravidade
        }
        return rb;
    }
}
