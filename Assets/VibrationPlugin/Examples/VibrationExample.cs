using UnityEngine;
using System.Collections;

public class VibrationExample : MonoBehaviour
{
    // Test text
    public GUIText guiText;

    void Start()
    {
        // Check the vibrator on existence
        if (Vibration.HasVibrator())
            guiText.text = "Vibration.HasVibrator() = true";
        else
            guiText.text = "Vibration.HasVibrator() = false";
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 250, 50), "Vibrate();"))
            Vibration.Vibrate();

        if (GUI.Button(new Rect(10, 90, 250, 50), "Vibrate(2000);"))
            Vibration.Vibrate(2000);

        if (GUI.Button(new Rect(10, 170, 250, 50), "Vibrate(4000);"))
            Vibration.Vibrate(4000);

        if (GUI.Button(new Rect(10, 250, 250, 50), "Vibrate(8000);"))
            Vibration.Vibrate(8000);

        if (GUI.Button(new Rect(10, 330, 250, 50), "Cancel();"))
            Vibration.Cancel();
    }
}