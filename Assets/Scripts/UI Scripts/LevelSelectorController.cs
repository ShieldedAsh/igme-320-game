using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectorController : MonoBehaviour
{
    [SerializeField] private Button enterButton;

    private LevelInfo selectedLevel;
    private bool isLoading;

    // when no level is choosed, can't hit enter
    private void Awake()
    { 
        enterButton.interactable = false;
    }

    // choose level
    public void SelectLevel(LevelInfo level)
    {
        if (isLoading == true || level == null)
            return;

        selectedLevel = level;
        enterButton.interactable = true;
        Debug.Log(selectedLevel.sceneName);
    }

    // enter level
    public void EnterSelectedLevel()
    {
        if (isLoading == true || selectedLevel == null)
            return;

        string path = selectedLevel.scenePath;

        if (string.IsNullOrWhiteSpace(path) ||
            !Application.CanStreamedLevelBeLoaded(path))
        {
            Debug.LogError("this path is null£º" + path);
            return;
        }

        isLoading = true;
        enterButton.interactable = false;
        SceneManager.LoadSceneAsync(path);
    }
}