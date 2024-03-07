using UnityEngine;

public class BalancaRoberval : MonoBehaviour
{
    public GameObject pratoEsquerdo;
    public GameObject pratoDireito;
    public GameObject aste;
    public float forcaTorque = 10f;

    private Rigidbody rbPratoEsquerdo;
    private Rigidbody rbPratoDireito;
    private Rigidbody rbAste;

    void Start()
    {
        // Obtém os Rigidbody dos pratos e da aste
        rbPratoEsquerdo = pratoEsquerdo.GetComponent<Rigidbody>();
        rbPratoDireito = pratoDireito.GetComponent<Rigidbody>();
        rbAste = aste.GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Verifica se um objeto com massa está em contato com os pratos
        bool objetoNoPratoEsquerdo = rbPratoEsquerdo != null && rbPratoEsquerdo.mass > 0;
        bool objetoNoPratoDireito = rbPratoDireito != null && rbPratoDireito.mass > 0;

        // Calcula a diferença de peso entre os pratos
        float diferencaPeso = Mathf.Abs(rbPratoEsquerdo.mass - rbPratoDireito.mass);

        // Aplica torque na aste para simular o movimento da balança
        if (objetoNoPratoEsquerdo || objetoNoPratoDireito)
        {
            Vector3 torque = diferencaPeso * forcaTorque * Vector3.up;
            rbAste.AddTorque(torque);
        }
    }
}
