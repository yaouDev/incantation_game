using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <bugs>
/// - [FIXED] if a charged weapon hovers over UI it doesn't release but loses charge
/// - mouse follows weapon instead of attackpoint with melee
/// - melee combat isnt satisfying as mouse is invisible - make melee pointer object?
/// - [FIXED] - move anything time related to FixedUpdate as there is a frame difference on Max screen/computer
/// - [FIXED] If a player begins charge/attack on incantation text it doesn't register -> put canvas group on UI object and uncheck interactable and blocks raycast
/// </bugs>

//As it is right now, equipment modifiers will mainly impact charge weapons

public class PlayerCombat : MonoBehaviour
{

    /*
    public Transform attackPoint;
    [SerializeField] protected LayerMask enemyLayers;

    protected Weapon currentWeapon;
    protected PlayerStats playerStats;
    protected PlayerCombatManager playerCombatManager;

    private void Start()
    {
        playerCombatManager = PlayerCombatManager.instance;
        playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        currentWeapon = EquipmentManager.instance.GetWeapon();

        MouseManager.instance.lockedCombat = currentWeapon.lockedCombat;
    }

    public virtual void Attack(int damageToDeal)
    {
        playerCombatManager.PlayerAttackAnimation();

        //Deal damage

        playerCombatManager.AttackDelay();
    }*/
}
