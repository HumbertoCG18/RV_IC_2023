using UnityEngine;
using UnityEngine.UI;

public class WinLoseController : MonoBehaviour
{
    public Transform secondFloorArea; // The area representing the second floor
    public int requiredObjects = 3;   // Number of objects needed to win
    public Text winText;              // UI Text to display win message
    public Text loseText;             // UI Text to display lose message

    private int objectCount = 0;      // Counter for objects on the second floor

    void Start()
    {
        winText.gameObject.SetActive(false);
        loseText.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ElevatorObject"))
        {
            objectCount++;
            CheckWinCondition();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("ElevatorObject"))
        {
            objectCount--;
        }
    }

    void CheckWinCondition()
    {
        if (objectCount >= requiredObjects)
        {
            Win();
        }
    }

    void Win()
    {
        winText.gameObject.SetActive(true);
        Time.timeScale = 0; // Stop the game
    }

    void Lose()
    {
        loseText.gameObject.SetActive(true);
        Time.timeScale = 0; // Stop the game
    }
}
