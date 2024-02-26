using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChangeColorOnSelect : MonoBehaviour
{
    public Color selectedColor = Color.blue; // Cor padrão para quando o objeto for selecionado

    private XRBaseInteractable interactable;
    private Renderer objectRenderer;
    private Color originalColor;

    void Start()
    {
        // Obtém referências para os componentes necessários
        interactable = GetComponent<XRBaseInteractable>();
        objectRenderer = GetComponent<Renderer>();

        // Salva a cor original do objeto
        originalColor = objectRenderer.material.color;

        // Adiciona listeners para os eventos de seleção
        interactable.selectEntered.AddListener(OnSelectEntered);
        interactable.selectExited.AddListener(OnSelectExited);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Muda a cor do objeto quando ele é selecionado para a cor especificada
        objectRenderer.material.color = selectedColor;
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        // Restaura a cor original quando a seleção é encerrada
        objectRenderer.material.color = originalColor;
    }
}

