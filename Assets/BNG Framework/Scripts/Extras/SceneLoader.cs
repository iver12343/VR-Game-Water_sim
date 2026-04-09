using UnityEngine;
using Esper.Freeloader; // Add this namespace at the top

public class SceneLoader : MonoBehaviour
{
    public void LoadScene()
    {
       

        // Option B: Load by build index
        LoadingScreen.Instance.Load(1);
    }
}