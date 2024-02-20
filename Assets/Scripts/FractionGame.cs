using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/*
public class FractionGame : MonoBehaviour
{
    public GameObject feedbackText;
    public GameObject correctCheckmark;
    public GameObject wrongCross;
    public XRController controller;
    public bool fractionSelected; 

    private void Update()
    {
    // Verifica se houve uma interação de poke no controlador XR 
        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool pokePressed) && pokePressed) 
    // CommonUsages.primaryButton é o botão de poke, que é o gatilho no controle Xr
        {
         // Dispara um raio a partir da posição do controlador XR
            Ray ray = new Ray(controller.transform.position, controller.transform.forward); // Ray é um raio que começa na posição do controlador e vai para a frente
            RaycastHit hit; // RaycastHit é uma estrutura que armazena informações sobre o raio atingindo um objeto na cena 
        // Se o raio atingir um objeto na cena
            if (Physics.Raycast(ray, out hit))
            {
              // Verifica se o objeto atingido é uma fração válida (1/2)
                if (hit.collider.CompareTag("Fraction")) // como que ele sabe? 
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
            if (fraction.AddComponent.onetwo == "HalfFraction")
            {
                // Ativa o feedback de correto
                feedbackText.text; // = "Correto!";
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
        feedbackText.feedback; // += "\n A fração 1/2 corresponde a um inteiro dividido em duas partes.";
    }

    // Função para passar o próximo nível 
    public void PassarProximoNivel()
    {
        // Verifica se o nome do próximo nível está definido
        if (!string.IsNullOrEmpty(proximoNivel))
        {
            // Carrega o próximo nível
            SceneManager.LoadScene(proximoNivel);
        }
        else
        {
            Debug.LogError("Nome do próximo nível não definido!");
        }
    }
}