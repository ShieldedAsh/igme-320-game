using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class RoomExit : MonoBehaviour
{
    [SerializeField] private NPCBaseBehavior miniBoss;
    [SerializeField] private GameObject unlockedVisual;
    [SerializeField] private string nextSceneName;

    private bool miniBossWasFound;
    private bool isUnlocked;
    private bool isLoading;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;

        if (miniBoss == null)
        {
            miniBoss = FindMiniBoss();
        }

        miniBossWasFound = miniBoss != null;

        if (unlockedVisual == null)
        {
            unlockedVisual = FindChildByName("DoorVisual");
        }

        bool isLobby = SceneManager.GetActiveScene().name == "Lobby";
        SetUnlocked(isLobby);
    }

    private void Update()
    {
        if (!isUnlocked)
        {
            TryUnlockExit();
        }
    }

    private NPCBaseBehavior FindMiniBoss()
    {
        NPCBaseBehavior[] npcs = FindObjectsByType<NPCBaseBehavior>(FindObjectsSortMode.None);

        foreach (NPCBaseBehavior npc in npcs)
        {
            if (npc.GetType().Name.Contains("MiniBoss",StringComparison.OrdinalIgnoreCase))
            {
                return npc;
            }
        }

        return null;
    }

    private GameObject FindChildByName(string childName)
    {
        Transform[] children = GetComponentsInChildren<Transform>(true);

        foreach (Transform child in children)
        {
            if (child != transform && child.name == childName)
            {
                return child.gameObject;
            }
        }

        return null;
    }

    public void TryUnlockExit()
    {
        bool miniBossDefeated = miniBossWasFound && miniBoss == null;

        if (miniBossDefeated && IsRoomClear())
        {
            SetUnlocked(true);
        }
    }

    public bool IsRoomClear()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length == 0;
    }

    private void SetUnlocked(bool unlocked)
    {
        isUnlocked = unlocked;

        if (unlockedVisual != null)
        {
            unlockedVisual.SetActive(unlocked);
        }
    }

   /* private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isUnlocked ||isLoading ||!other.CompareTag("Player"))
        {
            return;
        }

        EnterNextLevel();
    }
   */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isUnlocked ||isLoading ||!other.CompareTag("Player"))
        {
            return;
        }
        if (other.TryGetComponent(out PlayerStats playerStats))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            PlayerRunStats.Capture(playerStats,playerMovement);
        }

        EnterNextLevel();
    }

    public void EnterNextLevel()
    {
        if (isLoading)
        {
            return;
        }
        isLoading = true;
        SceneManager.LoadSceneAsync(nextSceneName);
    }
}