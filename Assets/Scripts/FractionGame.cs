using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

/*
public class FractionGame : MonoBehaviour
{
    public Text feedbackText;
    public GameObject correctCheckmark;
    public GameObject wrongCross;
    public XRController controller;

    private bool fractionSelected = false;

    private void Update()
    {
        // Verifica se houve uma interação de poke no controlador XR 
        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool pokePressed) && pokePressed) // CommonUsages.primaryButton é o botão de poke, que é o gatilho no controle Xr
        {
            // Dispara um raio a partir da posição do controlador XR
            Ray ray = new Ray(controller.transform.position, controller.transform.forward); // Ray é um raio que começa na posição do controlador e vai para a frente
            RaycastHit hit; // RaycastHit é uma estrutura que armazena informações sobre o raio atingindo um objeto na cena 

            // Se o raio atingir um objeto na cena
            if (Physics.Raycast(ray, out hit))
            {
                // Verifica se o objeto atingido é uma fração válida (1/2)
                if (hit.collider.CompareTag("Fraction"))
                {
                    fractionSelected = true;
                    CheckFraction(hit.collider.gameObject);
                }
            }
        }
    }

    // Função para verificar se a fração selecionada está correta
    private void CheckFraction(GameObject fraction)
    {
        if (fractionSelected)
        {
            // Verifica se a fração selecionada é 1/2
            if (fraction.name == "HalfFraction")
            {
                // Ativa o feedback de correto
                feedbackText.text = "Correto!";
                correctCheckmark.SetActive(true);
                PlayCorrectSound();
            }
            else
            {
                // Ativa o feedback de errado
                feedbackText.text = "Incorreto!";
                wrongCross.SetActive(true);
                PlayWrongSound();
                ShowFractionHint();
            }
        }
    }

    // Função para exibir uma dica sobre a fração 1/2
    private void ShowFractionHint()
    {
        feedbackText.text += "\n A fração 1/2 corresponde a um inteiro dividido em duas partes.";
    }

    // Função para reproduzir um som de correto
    private void PlayCorrectSound()
    {
        // Implemente a lógica para reproduzir um som de correto
    }

    // Função para reproduzir um som de errado
    private void PlayWrongSound()
    {
        // Implemente a lógica para reproduzir um som de errado
    }
}
*/