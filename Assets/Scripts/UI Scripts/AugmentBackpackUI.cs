using UnityEngine;
using UnityEngine.InputSystem;

public class AugmentBackpackUI : MonoBehaviour
{
    [SerializeField] private GameObject backpackPanel = null;

    private void Start()
    {
        backpackPanel.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current?.bKey.wasPressedThisFrame != true)
            return;

        bool open = !backpackPanel.activeSelf;

        if (open && Time.timeScale == 0f)
            return;

        backpackPanel.SetActive(open);
        Time.timeScale = open ? 0f : 1f;
    }
}