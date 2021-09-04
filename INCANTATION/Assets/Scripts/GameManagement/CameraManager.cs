using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    private Camera mainCam;


    private void Awake()
    {
        #region Singleton
        if (instance != null)
        {
            Debug.LogWarning("More than one Camera Managers in scene!");
            return;
        }

        instance = this;
        #endregion

        mainCam = Camera.main;
    }
}
