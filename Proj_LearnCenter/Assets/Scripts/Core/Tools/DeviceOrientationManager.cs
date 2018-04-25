using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class DeviceOrientationManager : MonoBehaviour
{   
    public static event UnityAction<DeviceOrientation, DeviceOrientation> OnDeviceOrientationChange;

    DeviceOrientation orientation;
    void Start()
    {
        if (Global.UNITY_MOBILE)
        {
            orientation = Input.deviceOrientation;
            if(orientation > DeviceOrientation.LandscapeRight || orientation < DeviceOrientation.Portrait)
            {
                if (Screen.width > Screen.height)
                    orientation = DeviceOrientation.LandscapeLeft;
                else
                    orientation = DeviceOrientation.Portrait;
            }
        }
        else
        {
            if (Screen.width > Screen.height)
                orientation = DeviceOrientation.LandscapeLeft;
            else
                orientation = DeviceOrientation.Portrait;
        }
    }

    void Update()
    {
        if (Global.UNITY_MOBILE)
        {
            if (orientation != Input.deviceOrientation)
            {
                if (Input.deviceOrientation <= DeviceOrientation.LandscapeRight && Input.deviceOrientation >= DeviceOrientation.Portrait)
                {
                    if (null != OnDeviceOrientationChange)
                        OnDeviceOrientationChange(orientation, Input.deviceOrientation);
                    orientation = Input.deviceOrientation;
                }
            }
        }
        else
        {
            if (Screen.width > Screen.height && orientation == DeviceOrientation.Portrait)
            {
                if (null != OnDeviceOrientationChange)
                    OnDeviceOrientationChange(orientation, DeviceOrientation.LandscapeLeft);
                orientation = DeviceOrientation.LandscapeLeft;
            }
            else if (Screen.height > Screen.width && orientation == DeviceOrientation.LandscapeLeft)
            {
                if (null != OnDeviceOrientationChange)
                    OnDeviceOrientationChange(orientation, DeviceOrientation.Portrait);
                orientation = DeviceOrientation.Portrait;
            }
        }        
    }

    public bool IsLandscape()
    {
        return orientation == DeviceOrientation.LandscapeLeft || orientation == DeviceOrientation.LandscapeRight;
    }

    public static void Init()
    {
        SingletonObject.getInstance<DeviceOrientationManager>();
    }
}
