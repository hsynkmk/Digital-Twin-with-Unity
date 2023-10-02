using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseResume : MonoBehaviour
{
    private bool isPaused = false;

    // Reference to the button's Image component
    public Image buttonImage;

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;

        // Change the button's panel background color based on the pause state
        if (buttonImage != null)
        {
            buttonImage.color = isPaused ? Color.red : Color.white; // You can set any color you prefer
        }
    }

    public void OnPauseButtonClicked()
    {
        TogglePause();
    }
}
