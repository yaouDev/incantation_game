using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    public static PlayerCombatManager instance;

    public Weapon emptyWeapon;
    private float damageTimer;

    private Animator playerAnimator;
    public Animator weaponAnimator;

    [SerializeField] private LayerMask enemyLayers;
    public Transform attackPoint;
    public Sprite defaultAttackPointGFX;
    private GameObject player;
    private PlayerStats playerStats;

    private float originalAttackRange;
    public float attackRange = 1f;
    [HideInInspector] public float baseAttackRange = 1f;

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

        originalAttackRange = attackRange;
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
