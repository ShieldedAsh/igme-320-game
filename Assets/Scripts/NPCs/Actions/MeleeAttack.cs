using UnityEngine;

/*
 * To Do:
 * - Multiattack (multiple positions)
 * - Multi-stage attack (multiple hitboxes)
 */


public class MeleeAttack : NPCBaseAction
{
    GameObject hitboxPrefab;

    public GameObject HitboxPrefab { get => hitboxPrefab; }

    public MeleeAttack(GameObject _hitboxPrefab, Transform _parent, float _actionDuration = 1) : base(_parent, _actionDuration) { hitboxPrefab = _hitboxPrefab; }

    public void SetHitbox(GameObject hitbox)
    {
        hitboxPrefab = hitbox;
    }

    #region Attack Location
    /// <summary>
    /// Attacks the location
    /// </summary>
    /// <param name="location">The location to target</param>
    /// <param name="lifetime">How long the hitbox lasts</param>
    /// <param name="hitboxXScale">The scale of the hitbox in the x-axis</param>
    /// <param name="hitboxYScale">The scale of the hitbox in the y-axis</param>
    /// <returns>The created hitbox</returns>
    public GameObject AttackLocation(Vector2 location, float lifetime, float hitboxXScale = 1.0f, float hitboxYScale = 1.0f)
    {
        return CreateHitbox(location, new Vector2(hitboxXScale, hitboxYScale), lifetime);
    }

    /// <summary>
    /// Attacks the location
    /// </summary>
    /// <param name="location">The location to target</param>
    /// <param name="lifetime">How long the hitbox lasts</param>
    /// <param name="hitboxScale">The scale of the hitbox</param>
    /// <returns>The created hitbox</returns>
    public GameObject AttackLocation(Vector2 location, float lifetime, Vector2 hitboxScale)
    {
        return CreateHitbox(location, hitboxScale, lifetime);
    }

    /// <summary>
    /// Attacks the location
    /// </summary>
    /// <param name="location">The location to target</param>
    /// <param name="settings">The attack settings</param>
    /// <returns>The created hitbox</returns>
    public GameObject AttackLocation(Vector2 location, MeleeSettings settings)
    {
        return CreateHitbox(location, settings.Scale, settings.Lifetime);
    }
    #endregion

    #region Create Hitbox
    /// <summary>
    /// Creates a hitbox at the location
    /// </summary>
    /// <param name="location">The location to create at</param>
    /// <param name="scale">The scale of the hitbox</param>
    /// <param name="lifetime">How long the lifetime lasts</param>
    /// <returns>The created hitbox</returns>
    private GameObject CreateHitbox(Vector2 location, Vector2 scale, float lifetime)
    {
        GameObject hitbox = Object.Instantiate(hitboxPrefab, parent);
        hitbox.gameObject.transform.position = location;
        hitbox.gameObject.transform.localScale = scale;
        MeleeBase meleeScript = hitbox.GetComponent<MeleeBase>();
        if (meleeScript != null)
        {
            meleeScript.SetLifetime(lifetime);
        }
        return hitbox;
    }
    #endregion
}
