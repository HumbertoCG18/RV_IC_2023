using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChangeColorOnSelect : MonoBehaviour
{
    public Color selectedColor = Color.blue; // Cor padr�o para quando o objeto for selecionado

    private XRBaseInteractable interactable;
    private Renderer objectRenderer;
    private Color originalColor;

    void Start()
    {
        // Obt�m refer�ncias para os componentes necess�rios
        interactable = GetComponent<XRBaseInteractable>();
        objectRenderer = GetComponent<Renderer>();

        // Salva a cor original do objeto
        originalColor = objectRenderer.material.color;

        // Adiciona listeners para os eventos de sele��o
        interactable.selectEntered.AddListener(OnSelectEntered);
        interactable.selectExited.AddListener(OnSelectExited);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Muda a cor do objeto quando ele � selecionado para a cor especificada
        objectRenderer.material.color = selectedColor;
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        // Restaura a cor original quando a sele��o � encerrada
        objectRenderer.material.color = originalColor;
    }
}

