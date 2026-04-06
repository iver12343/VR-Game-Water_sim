using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Exit : MonoBehaviour
{
    public void ConfirmExit()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #elif UNITY_ANDROID
        Application.Quit();
        using (AndroidJavaClass javaClass = new AndroidJavaClass("java.lang.System"))
        {
            javaClass.CallStatic("exit", 0);
        }
        #endif
    }

    public void CancelExit()
    {
        gameObject.SetActive(false);
    }
}