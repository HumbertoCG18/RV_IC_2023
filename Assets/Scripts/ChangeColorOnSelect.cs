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

    private bool isSelected = false;

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
        if (!isSelected)
        {
            // Muda a cor do objeto para a cor especificada quando ele é selecionado
            objectRenderer.material.color = selectedColor;
            isSelected = true;
        }
        else
        {
            // Restaura a cor original se o objeto já estiver selecionado
            objectRenderer.material.color = originalColor;
            isSelected = false;
        }
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        // Não faz nada quando a seleção é encerrada
        // Isso permitirá que o objeto mantenha a cor selecionada até ser selecionado novamente
    }
}

