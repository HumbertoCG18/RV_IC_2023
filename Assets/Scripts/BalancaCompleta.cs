using UnityEngine;

public class BalancaCompleta : MonoBehaviour
{
    public GameObject pratoEsquerda;
    public GameObject pratoDireita;
    public GameObject haste;
    public GameObject meioHaste;
    public GameObject meioSuporte;

    void Start()
    {
        // Conectar os pratos aos seus respectivos suportes e pontos na haste
        ConectarPratos();

        // Adicionar as joints aos pratos e suportes
        AdicionarJoints();
    }

    void ConectarPratos()
    {
        // Conectar o Prato Esquerda ao seu respectivo suporte e ponto na haste
        Transform suporteEsquerda = transform.Find("X(Prato Esquerda)/suporteConectado_E");
        Transform pontoEsquerda = haste.transform.Find("PontoEsquerda");
        pratoEsquerda.transform.SetParent(suporteEsquerda, true);
        suporteEsquerda.parent = pontoEsquerda;

        // Conectar o Prato Direita ao seu respectivo suporte e ponto na haste
        Transform suporteDireita = transform.Find("Y(Prato Direita)/suporteConectado_D");
        Transform pontoDireita = haste.transform.Find("PontoDireita");
        pratoDireita.transform.SetParent(suporteDireita, true);
        suporteDireita.parent = pontoDireita;

        // Conectar a haste ao suporte
        FixedJoint fixedHaste = haste.AddComponent<FixedJoint>();
        fixedHaste.connectedBody = meioSuporte.GetComponent<Rigidbody>();

        // Conectar os suportes dos pratos às extremidades da haste
        ConnectSupportToHaste(suporteEsquerda.gameObject, haste.transform.Find("PontoEsquerda").gameObject);
        ConnectSupportToHaste(suporteDireita.gameObject, haste.transform.Find("PontoDireita").gameObject);
    }

    void ConnectSupportToHaste(GameObject support, GameObject point)
    {
        FixedJoint fixedJoint = support.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = point.GetComponent<Rigidbody>();
    }



    void AdicionarJoints()
    {
        // Adicionar hinge joint ao Prato Esquerda
        HingeJoint hingePratoEsquerda = pratoEsquerda.AddComponent<HingeJoint>();
        hingePratoEsquerda.connectedBody = pratoEsquerda.transform.parent.GetComponent<Rigidbody>();
        hingePratoEsquerda.useSpring = true; // Adicionando mola para tornar a junta mais rígida
        hingePratoEsquerda.spring = new JointSpring { spring = 5000f }; // Ajuste a rigidez da mola conforme necessário

        // Adicionar hinge joint ao Prato Direita
        HingeJoint hingePratoDireita = pratoDireita.AddComponent<HingeJoint>();
        hingePratoDireita.connectedBody = pratoDireita.transform.parent.GetComponent<Rigidbody>();
        hingePratoDireita.useSpring = true; // Adicionando mola para tornar a junta mais rígida
        hingePratoDireita.spring = new JointSpring { spring = 5000f }; // Ajuste a rigidez da mola conforme necessário

        // Adicionar fixed joint ao suporte esquerdo
        FixedJoint fixedSuporteEsquerdo = pratoEsquerda.transform.parent.gameObject.AddComponent<FixedJoint>();
        fixedSuporteEsquerdo.connectedBody = pratoEsquerda.transform.parent.parent.GetComponent<Rigidbody>();

        // Adicionar fixed joint ao suporte direito
        FixedJoint fixedSuporteDireito = pratoDireita.transform.parent.gameObject.AddComponent<FixedJoint>();
        fixedSuporteDireito.connectedBody = pratoDireita.transform.parent.parent.GetComponent<Rigidbody>();
    }
}
