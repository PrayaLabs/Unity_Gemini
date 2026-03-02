using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Tooltip("Enter the exact scene name from Build Settings")]
    public string sceneName;

    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Scene name is empty!");
        }
    }
    public void ExitScene()
    {
        Application.Quit();
    }
}