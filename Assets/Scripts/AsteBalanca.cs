using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteBalanca : MonoBehaviour
{
    void Start()
    {
        // Adiciona o componente Hinge Joint � aste da balan�a
        HingeJoint hingeJoint = gameObject.AddComponent<HingeJoint>();

        // Configura o corpo conectado como nulo, pois a aste estar� se conectando a si mesma
        hingeJoint.connectedBody = null;

        // Configura o eixo de rota��o da junta (nesse caso, para cima)
        hingeJoint.axis = Vector3.up;

        // Configura os limites de rota��o para permitir movimento angular
        JointLimits limits = new JointLimits();
        limits.min = -90f; // �ngulo m�nimo de rota��o (para baixo)
        limits.max = 90f; // �ngulo m�ximo de rota��o (para cima)
        hingeJoint.limits = limits;

        // Habilita os limites para que a rota��o seja restrita dentro dos valores configurados
        hingeJoint.useLimits = true;
    }
}
