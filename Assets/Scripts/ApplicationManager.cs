using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    [SerializeField] private GameObject _telaInicia;
    [SerializeField] private ExerciciosManager _tutorialManager;
    [SerializeField] private float _tempoAteIniciar = 2f;

    private void Start()
    {
        StartCoroutine(IniciaTutorialCoroutine(_tempoAteIniciar));
    }

    private IEnumerator IniciaTutorialCoroutine(float tempoDeEspera)
    {
        yield return new WaitForSeconds(tempoDeEspera);

        _telaInicia.SetActive(false);
        _tutorialManager.IniciaExercicios();
    }
}
