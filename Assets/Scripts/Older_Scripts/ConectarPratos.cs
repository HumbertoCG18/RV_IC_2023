using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConectarPratos : MonoBehaviour
{
    public Transform pratoEsquerda;
    public Transform pratoDireita;
    public Transform suportePratoEsquerda;
    public Transform suportePratoDireita;
    public Transform pontoEsquerda;
    public Transform pontoDireita;

    void Start()
    {
        // Conectar prato esquerdo ao seu suporte e ponto na haste
        Conectar(pratoEsquerda, suportePratoEsquerda);
        Conectar(suportePratoEsquerda, pontoEsquerda);

        // Conectar prato direito ao seu suporte e ponto na haste
        Conectar(pratoDireita, suportePratoDireita);
        Conectar(suportePratoDireita, pontoDireita);
    }

    void Conectar(Transform filho, Transform pai)
    {
        filho.parent = pai;
        filho.localPosition = Vector3.zero;
        filho.localRotation = Quaternion.identity;
    }
}

