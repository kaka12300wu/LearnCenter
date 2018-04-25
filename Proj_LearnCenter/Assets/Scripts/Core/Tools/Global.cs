#if UNITY_IOS || UNITY_ANDROID
#if !UNITY_EDITOR
#define UNITY_MOBILE

#endif
#endif



public class Global
{
#if UNITY_MOBILE
    public static bool UNITY_MOBILE = true;
#else
    public static bool UNITY_MOBILE = false;
#endif
}

