using System.Collections;
using UnityEngine;

public class Speed : MonoBehaviour
{
    private float speedValue = 1.0f;  // Initial speed value
    private float timeSinceStart = 0f;  // Track the time since the game started

    void Update()
    {
        timeSinceStart += Time.deltaTime;  // Update the time since the game started

        // Increase speed every 5 seconds
        if (timeSinceStart >= 5f)
        {
            speedValue += 0.5f;
            timeSinceStart = 0f;  // Reset the timer
        }
    }

    public float GetSpeed()
    {
        return speedValue;
    }
}
