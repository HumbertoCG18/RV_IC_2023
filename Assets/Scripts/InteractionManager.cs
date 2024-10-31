using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractionManager : Singleton<InteractionManager>
{
    public IEnumerator DropObject(XRGrabInteractable controller)
    {
        var mask = controller.interactionLayers;
        controller.interactionLayers = 0;

        yield return null;

        controller.interactionLayers = mask;
    }
}
