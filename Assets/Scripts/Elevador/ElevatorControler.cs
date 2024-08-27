using UnityEngine;
using UnityEngine.UI;

public class ElevatorController : MonoBehaviour
{
    public Transform lowerPosition;  // The position of the elevator on the lower floor
    public Transform upperPosition;  // The position of the elevator on the upper floor
    public float speed = 2.0f;       // Speed of the elevator

    private bool goingUp = false;    // Is the elevator going up?
    private bool moving = false;     // Is the elevator moving?

    void Update()
    {
        if (moving)
        {
            MoveElevator();
        }
    }

    public void ToggleElevator()
    {
        if (!moving)
        {
            goingUp = !goingUp;
            moving = true;
        }
    }

    void MoveElevator()
    {
        Transform target = goingUp ? upperPosition : lowerPosition;
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (transform.position == target.position)
        {
            moving = false;
        }
    }
}
