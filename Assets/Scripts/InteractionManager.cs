using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractionManager : Singleton<InteractionManager>
{
    [Header("Ray Interaction Parameters")]
    [SerializeField] private XRRayInteractor _xrRayInteractorLeft;
    [SerializeField] private XRRayInteractor _xrRayInteractorRight;

    [Header("Teleport parameters")]
    [SerializeField] private GameObject _teleportParent;
    [SerializeField] private GameObject _teleportScriptParent;
    [SerializeField] private float _teleportTimeout = 0.5f;

    public void DropObject(XRGrabInteractable controller, bool disableTeleport=false)
    {
        StartCoroutine(DropObjectCoroutine(controller, disableTeleport));
    }

    public IEnumerator DropObjectCoroutine(XRGrabInteractable controller, bool disableTeleport=false)
    {
        if (disableTeleport) DisableTeleport();

        //var mask = controller.interactionLayers;
        //controller.interactionLayers = 0;

        var interactor = _xrRayInteractorLeft.interactablesSelected.Contains(controller) ? _xrRayInteractorLeft : _xrRayInteractorRight;

        interactor.allowSelect = false;

        yield return null;

        interactor.allowSelect = true;
        //controller.interactionLayers = mask;

        if (disableTeleport)
        {
            yield return new WaitForSeconds(_teleportTimeout);

            EnableTeleport();
        }
    }

    public void EnableTeleport()
    {
        _teleportParent.SetActive(true);
        _teleportScriptParent.SetActive(true);
    }

    public void DisableTeleport()
    {
        _teleportParent.SetActive(false);
        _teleportScriptParent?.SetActive(false);
    }
}
