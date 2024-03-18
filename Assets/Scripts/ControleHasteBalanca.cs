using UnityEngine;

public class ControleHasteBalanca : MonoBehaviour
{
    public PlacaBalanca placaEsquerda;
    public PlacaBalanca placaDireita;
    public Transform pontoEsquerda;
    public Transform pontoDireita;

    private ConfigurableJoint joint;

    void Start()
    {
        // Obter o ConfigurableJoint existente na haste
        joint = GetComponent<ConfigurableJoint>();

        // Certificar-se de que o ConfigurableJoint foi encontrado
        if (joint != null)
        {
            // Configurar o ConfigurableJoint
            joint.connectedAnchor = Vector3.zero; // Ancoragem fixa no MeioHaste
            joint.xMotion = ConfigurableJointMotion.Free;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
            joint.angularXMotion = ConfigurableJointMotion.Free;
            joint.angularYMotion = ConfigurableJointMotion.Locked;
            joint.angularZMotion = ConfigurableJointMotion.Locked;

            // Configura a rota��o inicial da haste
            transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
        }
        else
        {
            Debug.LogError("ConfigurableJoint n�o encontrado na haste!");
        }
    }

    void Update()
    {
        if (joint != null)
        {
            // Calcula a diferen�a de peso entre os pratos
            float diferencaPeso = Mathf.Abs(placaEsquerda.GetPesoPrato(PlacaBalanca.PratoSelecionado.Esquerda) - placaDireita.GetPesoPrato(PlacaBalanca.PratoSelecionado.Direita));

            // Calcula o �ngulo de rota��o da haste com base na diferen�a de peso
            float anguloX = Mathf.Clamp(-90f + diferencaPeso, -115f, -65f);
            transform.localRotation = Quaternion.Euler(anguloX, 0f, 0f);
        }
    }
}
