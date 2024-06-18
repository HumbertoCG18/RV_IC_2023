using UnityEngine;



public class FractionUnit : MonoBehaviour

{

    public FractionTile[] tiles; // Array de FractionTiles que comp�em a unidade

    public int baseValue; // Base da fra��o

    private bool selectionCompleted = false;



    // Verifica se a sele��o atual � correta

    public bool IsSelectionCorrect()

    {

        float selectedValue = 0f;

        foreach (FractionTile tile in tiles)

        {

            if (tile.gameObject.activeSelf)

            {

                selectedValue += 1f / baseValue;

            }

        }

        return Mathf.Approximately(selectedValue, 1f / baseValue);

    }



    // Define todos os tiles como corretos

    public void SetTilesCorrect()

    {

        foreach (FractionTile tile in tiles)

        {

            tile.SetCorrect();

        }

    }



    // Define todos os tiles como incorretos

    public void SetTilesIncorrect()

    {

        foreach (FractionTile tile in tiles)

        {

            tile.SetIncorrect();

        }

    }



    // Reseta todos os tiles

    public void ResetTiles()

    {

        foreach (FractionTile tile in tiles)

        {

            tile.ResetTile();

        }

        selectionCompleted = false;

    }



    // Define a sele��o como completada

    public void SetSelectionCompleted()

    {

        selectionCompleted = true;

    }



    // Verifica se a sele��o est� completada

    public bool IsSelectionCompleted()

    {

        return selectionCompleted;

    }

}