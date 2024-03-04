using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteBalanca : MonoBehaviour
{
    void Start()
    {
        // Adiciona o componente Hinge Joint à aste da balança
        HingeJoint hingeJoint = gameObject.AddComponent<HingeJoint>();

        // Configura o corpo conectado como nulo, pois a aste estará se conectando a si mesma
        hingeJoint.connectedBody = null;

        // Configura o eixo de rotação da junta (nesse caso, para cima)
        hingeJoint.axis = Vector3.up;

        // Configura os limites de rotação para permitir movimento angular
        JointLimits limits = new JointLimits();
        limits.min = -90f; // Ângulo mínimo de rotação (para baixo)
        limits.max = 90f; // Ângulo máximo de rotação (para cima)
        hingeJoint.limits = limits;

        // Habilita os limites para que a rotação seja restrita dentro dos valores configurados
        hingeJoint.useLimits = true;
    }
}
