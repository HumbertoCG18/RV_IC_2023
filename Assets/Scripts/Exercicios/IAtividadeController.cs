using System;
using UnityEngine;

public interface  IAtividadeController
{
    public void SetOnAtividadeConcluida(Action callback);

    public void CarregaAtividade(ScriptableObject atividade);
}