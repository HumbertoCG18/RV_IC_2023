using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DivisorDeObjetosController : MonoBehaviour
{
    [SerializeField] private GameObject _uiView;
    [SerializeField] private Transform _modelo3D;
 
    private ObjetoDivisivelController _objetoDivisivelAtual;

    private void Awake()
    {
        _uiView.SetActive(false);
    }

    public void OnCustomTriggerEnter(Collider other)
    {

        var controller = other.GetComponentInChildren<ColisorModelo3DController>();

        if (controller == null) return;

        if (controller.Controller.layer == LayerMask.NameToLayer("ObjetoBloqueado")) return;

        var objetoDivisivel = controller.Controller.GetComponentInChildren<ObjetoDivisivelController>();

        if (objetoDivisivel == null) return;

        StartCoroutine(IniciaInterfaceCoroutine(objetoDivisivel));
    }

    private IEnumerator IniciaInterfaceCoroutine(ObjetoDivisivelController objetoDivisivel)
    {
        var grabController = _modelo3D.GetComponentInChildren<XRGrabInteractable>();

        var mask = grabController.interactionLayers;
        grabController.interactionLayers = 0;

        yield return null;

        grabController.interactionLayers = mask;

        _uiView.SetActive(true);
        _uiView.transform.position = objetoDivisivel.transform.position + objetoDivisivel.transform.up * 0.1f;

        _objetoDivisivelAtual = objetoDivisivel;
        _modelo3D.localPosition = Vector3.zero;
        _modelo3D.localRotation = Quaternion.identity;

        _modelo3D.gameObject.SetActive(false);
    }

    public void DivideObjeto(int qtdPartes)
    {
        if (_objetoDivisivelAtual == null) return;

        _objetoDivisivelAtual.DivideObjeto(qtdPartes);

        _objetoDivisivelAtual = null;
        _uiView.SetActive(false);

        _modelo3D.gameObject.SetActive(true);
    }

    public void CancelaOperacao()
    {
        _uiView.SetActive(false);
        _modelo3D.localPosition = Vector3.zero;
        _modelo3D.localRotation = Quaternion.identity;
        _objetoDivisivelAtual = null;
        _modelo3D.gameObject.SetActive(true);
    }
}
