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
    public Action<int> temp;


    private void Start()
    {
        int linhas = 0, colunas = 0;

        for (int i = 1; i < 20; i++)
        {
            /*
            if (i % 2 == 1)
            {
                linhas = 1;
                colunas = i;
            }
            else
            {
                float expoente = Mathf.Log(i, 2);
                Debug.Log($"Exporente  {expoente} {(expoente * expoente - i)}");
                if ((expoente * expoente - i) <= 0.0001f)
                {
                    linhas = colunas = (int)expoente;
                }
                else
                {
                    colunas = Mathf.FloorToInt(expoente);
                    linhas = i / colunas;
                    
                    if (colunas < linhas)
                    {
                        // Swap
                        linhas = linhas + colunas;
                        colunas = linhas - colunas;
                        linhas = linhas - colunas;
                    }
                }
            }
            */

            linhas = 1;
            colunas = i;

            // Procurar o par de divisores mais próximo da raiz quadrada para definir o grid
            for (int j = 1; j <= Math.Sqrt(i); j++)
            {
                if (i % j == 0)
                {
                    linhas = j;
                    colunas = i / j;
                }
            }
            Debug.Log($"{i} -> {linhas}x{colunas}");
        }


    }
}
