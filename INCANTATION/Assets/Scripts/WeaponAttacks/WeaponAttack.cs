using UnityEngine;

public abstract class WeaponAttack : MonoBehaviour
{
    public static WeaponAttack instance;

    protected PlayerCombatManager pcm;
    protected Transform attackPoint;
    protected Transform weapon;
    protected LayerMask enemyLayers;
    protected PlayerStats playerStats;
    protected float damageTimer;
    protected float knockbackPower;

    public bool manualAttackFreeze;

    private void Awake()
    {
        Singleton();
    }

    private void Start()
    {
        InitializeWeapon();
    }

    protected void Singleton()
    {
        #region Singleton
        if (instance != null)
        {
            Debug.Log("Found more than one special weapon, might be temporary");
        }
        instance = this;
        #endregion
    }

    protected void InitializeWeapon()
    {
        pcm = PlayerCombatManager.instance;
        attackPoint = pcm.attackPoint;
        weapon = pcm.weapon;
        enemyLayers = pcm.enemyLayers;
        playerStats = pcm.playerStats;

        Weapon currentWeapon = (Weapon)EquipmentManager.instance.currentEquipment[(int)EquipmentSlot.weapon];
        knockbackPower = currentWeapon.knockbackPower;
    }

    protected bool canAttack()
    {
        //VV insert more conditions
        if (GameManager.instance.isInputEnabled && damageTimer <= 0f || manualAttackFreeze)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Update()
    {
        canAttack();
    }

    private void FixedUpdate()
    {
        attackSpeedTimer();
    }

    protected void attackSpeedTimer()
    {
        //put in fixed update
        if (damageTimer > 0f)
        {
            damageTimer -= Time.deltaTime;
        }
    }

    protected void AttackDelay()
    {
        int attackSpeed = playerStats.attackSpeed.GetValue();
        //max attack time VVV
        float attackTime = 1f;
        float convertedAttackSpeed = attackTime - attackSpeed / 100f;
        damageTimer = convertedAttackSpeed;
    }
}
