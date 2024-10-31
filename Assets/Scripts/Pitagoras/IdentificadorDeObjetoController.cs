using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdentificadorDeObjetoController : MonoBehaviour
{
    [SerializeField] private string _identificadorObjeto;

    public string Identificador => _identificadorObjeto;
}
