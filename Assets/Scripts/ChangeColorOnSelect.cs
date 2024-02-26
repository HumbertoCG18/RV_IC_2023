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

    private bool isSelected = false;

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
        if (!isSelected)
        {
            // Muda a cor do objeto para a cor especificada quando ele � selecionado
            objectRenderer.material.color = selectedColor;
            isSelected = true;
        }
        else
        {
            // Restaura a cor original se o objeto j� estiver selecionado
            objectRenderer.material.color = originalColor;
            isSelected = false;
        }
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        // N�o faz nada quando a sele��o � encerrada
        // Isso permitir� que o objeto mantenha a cor selecionada at� ser selecionado novamente
    }
}

