using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject pauseScreen;

    private bool isPaused;

    //on load set pause=false
    private void Awake()
    {
        SetPaused(false);
        CheckEventSystem();
    }

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            SetPaused(!isPaused);
        }
    }

    public void PauseGame() //pause game method for pause button
    {
        SetPaused(true);
    }

    
    public void ResumeGame()//resume game method for resume button
    {
        SetPaused(false);
    }

    public void GoToMainMenu()//go to main menu for start menu button
    {
        LoadScene("StartMenu");
    }

    public void GoToLevelSelect()//go to level select for level selector button
    {
        LoadScene("LevelSelect");
    }

    public void ExitGame()
    {
        Debug.Log("Exit game");
        Application.Quit();
    }

    private void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene(sceneName);
    }

    private void SetPaused(bool paused)
    {
        isPaused = paused;

        pauseScreen.SetActive(paused); //show pause menu screen
        pauseButton.SetActive(!paused); //hide pause button on the top tight

        if (paused)
        {
            Time.timeScale = 0f; //pause time
        }
        else
        {
            Time.timeScale = 1f;//resume time
        }
    }

    private void OnDestroy()
    {
        if (isPaused)
        {
            Time.timeScale = 1f;
        }
    }

    private void CheckEventSystem()
    {
        UnityEngine.EventSystems.EventSystem existingEventSystem = UnityEngine.Object.FindAnyObjectByType < UnityEngine.EventSystems.EventSystem>();

        if (existingEventSystem != null)
        {
            return;
        }
        GameObject eventSystemObject = new GameObject("EventSystem");
        eventSystemObject.AddComponent < UnityEngine.EventSystems.EventSystem>();
        eventSystemObject.AddComponent < UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
    }
}