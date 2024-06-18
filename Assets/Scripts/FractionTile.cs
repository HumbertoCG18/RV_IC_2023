using UnityEngine;

public class FractionTile : MonoBehaviour
{
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    public void SetCorrect()
    {
        rend.material.color = Color.green; // Define a cor como verde para indicar correta
        gameObject.SetActive(false); // Desativa o objeto para impedir seleção adicional
    }

    public void SetIncorrect()
    {
        rend.material.color = Color.red; // Define a cor como vermelha para indicar incorreta
    }

    public void ResetTile()
    {
        rend.material.color = Color.white; // Restaura a cor original
        gameObject.SetActive(true); // Ativa o objeto novamente
    }
}
