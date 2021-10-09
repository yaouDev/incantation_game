using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    //Use this script to bounce references to attack scripts

    public static PlayerCombatManager instance;

    public Weapon emptyWeapon;
    public float damageTimer;

    public Animator playerAnimator;
    public Animator weaponAnimator;

    public Transform weapon;

    public LayerMask enemyLayers;

    public float baseWeaponOffset = 1.5f;

    public Transform attackPoint;
    public Sprite defaultAttackPointGFX;
    public GameObject player;
    public PlayerStats playerStats;

    public bool clickIsAttack = true;

    [Header("ReadOnly")]
    public float currentWeaponOffset;

    private void Awake()
    {
        #region Singleton
        if (instance != null)
        {
            Debug.LogWarning("More than one Player Combat Managers found");
        }
        instance = this;
        #endregion
    }

    private void Start()
    {
        player = PlayerManager.instance.player;
        playerStats = player.GetComponent<PlayerStats>();
        currentWeaponOffset = baseWeaponOffset;

        playerAnimator = player.GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        //Attack speed timer
        if (damageTimer > 0f)
        {
            damageTimer -= Time.deltaTime;
        }
    }
    public void AttackDelay()
    {
        int attackSpeed = playerStats.attackSpeed.GetValue();
        //max attack time VVV
        float attackTime = 1f;
        float convertedAttackSpeed = attackTime - attackSpeed / 100f;
        damageTimer = convertedAttackSpeed;
    }

    public void PlayerAttackAnimation()
    {
        weaponAnimator.SetFloat("Speed", 0f);
        weaponAnimator.SetTrigger("Attack");

        playerAnimator.SetTrigger("Attack");
    }
}
