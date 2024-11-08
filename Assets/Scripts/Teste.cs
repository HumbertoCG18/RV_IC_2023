using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Teste : Singleton<Teste>
{
    public AtividadePitagorasController atividade;


    private void Update()
    {
        if (Keyboard.current[Key.P].wasPressedThisFrame)
        {
            atividade.OnNivelConcluido();
        }
    }
}
