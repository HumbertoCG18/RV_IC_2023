using UnityEngine;

public class FixarPratosEBalanca : MonoBehaviour
{
    public GameObject pratoEsquerdo;
    public GameObject suportePratoEsquerdo;
    public GameObject pratoDireito;
    public GameObject suportePratoDireito;
    public GameObject pontoDireita; // Ponto virtual na extremidade direita da haste
    public GameObject pontoEsquerda; // Ponto virtual na extremidade esquerda da haste

    void Start()
    {
        // Conectar prato esquerdo ao suporte esquerdo
        ConectarComponentes(pratoEsquerdo, suportePratoEsquerdo);

        // Conectar prato direito ao suporte direito
        ConectarComponentes(pratoDireito, suportePratoDireito);

        // Conectar suporte da balança aos pontos virtuais nas extremidades da haste
        ConectarComponentes(suportePratoEsquerdo, pontoEsquerda);
        ConectarComponentes(suportePratoDireito, pontoDireita);
    }

    void ConectarComponentes(GameObject obj1, GameObject obj2)
    {
        FixedJoint joint = obj1.AddComponent<FixedJoint>();
        joint.connectedBody = obj2.GetComponent<Rigidbody>();
        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = Vector3.zero;
        joint.connectedAnchor = Vector3.zero;
    }
}

