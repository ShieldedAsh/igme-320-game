using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelSelectorController : MonoBehaviour
{
    [SerializeField] private Button enterButton;
    [SerializeField] private Color normalButtonColor = Color.white;
    [SerializeField] private Color selectedButtonColor = Color.gray;
    private UnityEngine.UI.Button selectedLevelButton;
    private LevelInfo selectedLevel;
    private bool isLoading;

    // when no level is choosed, can't hit enter
    private void Awake()
    { 
        enterButton.interactable = false;
        UpdateLevelButtonColors(null);
    }

    // choose level
    public void SelectLevel(LevelInfo level)
    {
        if (isLoading == true || level == null)        
        return;

        selectedLevel = level;
        enterButton.interactable = true;
        Button clickedButton = null;
        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
        {
            clickedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        }

        UpdateLevelButtonColors(clickedButton);
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
            Debug.LogError("this path is null " + path);
            return;
        }

        isLoading = true;
        enterButton.interactable = false;
        SceneManager.LoadSceneAsync(path);
    }

    private void UpdateLevelButtonColors(UnityEngine.UI.Button newSelectedButton)
    {
        if (selectedLevelButton != null)
        {
            SetButtonColor(selectedLevelButton,normalButtonColor);
        }

        selectedLevelButton = newSelectedButton;

        if (selectedLevelButton != null)
        {
            SetButtonColor(selectedLevelButton,selectedButtonColor);
        }
    }

    private void SetButtonColor(UnityEngine.UI.Button button,Color color)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = color;
        colors.highlightedColor = color;
        colors.pressedColor = color;
        colors.selectedColor = color;
        button.colors = colors;
    }
}