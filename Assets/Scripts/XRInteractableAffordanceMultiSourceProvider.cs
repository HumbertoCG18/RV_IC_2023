/*
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.State;

public class XRInteractableAffordanceMultiSourceProvider : MonoBehaviour
{
    [Tooltip("Lista de fontes de Interactable")]
    public List<XRBaseInteractable> interactableSources = new List<XRBaseInteractable>();

    private void OnEnable()
    {
        foreach (var source in interactableSources)
        {
            if (source != null)
            {
                source.selectEntered.AddListener(OnSelectEntered);
                source.selectExited.AddListener(OnSelectExited);
            }
        }
    }

    private void OnDisable()
    {
        foreach (var source in interactableSources)
        {
            if (source != null)
            {
                source.selectEntered.RemoveListener(OnSelectEntered);
                source.selectExited.RemoveListener(OnSelectExited);
            }
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        XRInteractableAffordanceStateProvider affordance = args.interactableObject.GetComponentInChildren<XRInteractableAffordanceStateProvider>();
        if (affordance != null)
        {
            affordance.grabbed = true; // Exemplo de modifica��o do estado do objeto quando � selecionado
        }
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        XRInteractableAffordanceStateProvider affordance = args.interactableObject.GetComponentInChildren<XRInteractableAffordanceStateProvider>();
        if (affordance != null)
        {
            affordance.grabbed = false; // Exemplo de restaura��o do estado do objeto quando a sele��o � encerrada
        }
    }
}
*/