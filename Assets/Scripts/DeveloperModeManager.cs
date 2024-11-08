using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Referencia https://discussions.unity.com/t/accessing-menubutton-on-oculus-quest-2/853970
public class DeveloperModeManager : MonoBehaviour
{
    [SerializeField] private List<ExerciciosManager> _exercicionsManagers;
    [SerializeField] private InputActionReference _botaoMenuController;

    private bool _modoDesenvolvedorLigado = false;

    private void OnEnable()
    {
        _botaoMenuController.action.performed += AtivarModoDesenvolvedor;
    }
    private void OnDisable()
    {
        _botaoMenuController.action.performed -= AtivarModoDesenvolvedor;
    }

    public void AtivarModoDesenvolvedor(InputAction.CallbackContext context)
    {
        _modoDesenvolvedorLigado = !_modoDesenvolvedorLigado;
        _exercicionsManagers.ForEach(e => e.SetExerciciosFinalizados(_modoDesenvolvedorLigado));
    }
}
