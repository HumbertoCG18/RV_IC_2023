using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Teste : Singleton<Teste>
{
    public Action<int> temp;


    private void Start()
    {
        AddListener(Callback1);    
        AddListener(Callback2);

        temp?.Invoke(10);
    }

    public void AddListener(Action<int> callback)
    {
        temp += callback;
    }

    public void Callback1(int i)
    {
        Debug.Log($">>>> {i}");
    }


    public void Callback2(int i)
    {
        Debug.Log($"===> {i}");
    }
}
