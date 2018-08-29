using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Vibration
{
#if UNITY_ANDROID && !UNITY_EDITOR
    private static AndroidJavaObject vibrationObj = VibrationActivity.activityObj.Get<AndroidJavaObject>("vibration");
#else
    private static AndroidJavaObject vibrationObj;
#endif

    public static void Vibrate()
    {
        if (Application.platform == RuntimePlatform.Android)
            vibrationObj.Call("vibrate");
    }

    public static void Vibrate(long milliseconds)
    {
        if (Application.platform == RuntimePlatform.Android)
            vibrationObj.Call("vibrate", milliseconds);
    }

    public static void Vibrate(long[] pattern, int repeat)
    {
        if (Application.platform == RuntimePlatform.Android)
            vibrationObj.Call("vibrate", pattern, repeat);
    }

    public static bool HasVibrator()
    {
        if (Application.platform == RuntimePlatform.Android)
            return vibrationObj.Call<bool>("hasVibrator");
        else
            return false;
    }

    public static void Cancel()
    {
        if (Application.platform == RuntimePlatform.Android)
            vibrationObj.Call("cancel");
    }
}