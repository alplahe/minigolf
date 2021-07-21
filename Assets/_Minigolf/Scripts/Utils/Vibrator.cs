using UnityEngine;

public class Vibrator
{
  private const long DEFAULT_VIBRATION_LENGTH = 250;

#if UNITY_ANDROID && !UNITY_EDITOR
  public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
  public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
  public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
  public static AndroidJavaClass unityPlayer;
  public static AndroidJavaObject currentActivity;
  public static AndroidJavaObject vibrator;
#endif

  public static void Vibrate(long lengthInMilliseconds = DEFAULT_VIBRATION_LENGTH)
  {
    Debug.Log("#Vibrator# Vibrate. Length: " + lengthInMilliseconds);

    if (IsAndroid())
    {
      vibrator.Call("vibrate", lengthInMilliseconds);
    }
    else
    {
      Handheld.Vibrate();
    }
  }

  public static void Cancel()
  {
    if (IsAndroid())
    {
      vibrator.Call("cancel");
    }
  }

  public static bool IsAndroid()
  {
#if UNITY_ANDROID
    return true;
#else
    return false;
#endif
  }
}
