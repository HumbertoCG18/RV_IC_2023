using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NivelElevadorController : NivelTutorialController
{
    [SerializeField] private List<AreaDosObjetosController> _areasDosObjetos;

    public override void ConfiguraNivel()
    {
        ElevadorController.Instance.SetAreasDosObjetosDinamicas(_areasDosObjetos);
    }
}
