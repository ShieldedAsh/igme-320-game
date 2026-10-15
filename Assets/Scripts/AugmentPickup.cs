using UnityEngine;

public class AugmentPickup : MonoBehaviour
{
    [SerializeField] private AugmentBase augment;
    private bool collected;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (collected)
        {
            return;
        }

        if (other.TryGetComponent(out PlayerAugments player))
        {
            collected = true;
            player.Add(augment);
            Destroy(gameObject);
        }
    }
}
