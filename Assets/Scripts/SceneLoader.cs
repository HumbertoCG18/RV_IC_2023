using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private string targetSceneName;

    public string TargetSceneName
    {
        get => targetSceneName;
        set => targetSceneName = value;
    }

    private void Start()
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            LoadTargetScene();
        }
        else
        {
            Debug.LogError("Target scene name is not specified in the inspector.");
        }
    }

    public void LoadTargetScene()
    {
        SceneManager.LoadScene(targetSceneName);
    }
}
