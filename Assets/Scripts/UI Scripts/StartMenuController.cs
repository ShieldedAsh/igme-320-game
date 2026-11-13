using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("MiniBossRoom_1");
    }

    public void StartTutorial()
    {
        SceneManager.LoadScene("Lobby");
    }
}