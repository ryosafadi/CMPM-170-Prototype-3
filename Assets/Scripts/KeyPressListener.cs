using UnityEngine;

public class KeyPressListener : MonoBehaviour
{
    // Define events for left and right arrow key presses
    public static event System.Action OnLeftArrowPressed;
    public static event System.Action OnRightArrowPressed;

    void Update()
    {
        // Check for left arrow key press and invoke event
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnLeftArrowPressed?.Invoke();
        }

        // Check for right arrow key press and invoke event
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnRightArrowPressed?.Invoke();
        }
    }
}