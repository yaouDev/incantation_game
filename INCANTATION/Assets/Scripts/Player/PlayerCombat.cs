using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private AttackType attackType;
    private EssenceType essenceType = EssenceType.none;
    private GameManager gameManager;

    public Animator animator;
    private Animator weaponAnimator;

    public Transform attackPoint;
    public GameObject freeRangeTarget;
    public Transform weapon;

    private float originalAttackRange;
    public float attackRange = 1f;
    public float baseAttackRange = 1f;
    //space from body of playerVVV
    [SerializeField] private float weaponPosition = 1.5f;

    public LayerMask enemyLayers;
    public PlayerStats playerStats;
    private Rigidbody2D rb;

    private Camera cam;
    Vector2 mousePos;

    private float essenceTimer = 0f;
    private float damageTimer = 0f;

    private Weapon currentWeapon;

    [SerializeField] private Slider chargeMeleeSlider;
    [SerializeField] private Slider chargeProjectileSlider;
    [SerializeField] private Slider chargeFreeRangeSlider;

    //---Ranged weapon related---
    [SerializeField] private GameObject projectile;
    [SerializeField] private SpriteRenderer projectileGFX;
    //---Ranged weapon end---

    //---Charge related---
    [Range(0, 10)]
    [SerializeField] private float chargePower;

    [Range(1, 4)]
    [SerializeField] private float maxCharge;

    private bool movePenaltyActive;
    private int movePenalty;
    public int chargeMoveDivider = 2;

    private float currentCharge;
    private float baseCharge = 1f;
    private bool canFire = true;
    //---charge end---

    public Weapon emptyWeapon;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStats = gameObject.GetComponent<PlayerStats>();
        cam = Camera.main;
        gameManager = GameManager.instance;
        freeRangeTarget.SetActive(false);
        currentWeapon = emptyWeapon;
        originalAttackRange = attackRange;
        weaponAnimator = weapon.GetComponent<Animator>();

        //---ranged weapon---
        chargeProjectileSlider.value = 0f;
        chargeProjectileSlider.maxValue = maxCharge;
        chargeProjectileSlider.gameObject.SetActive(false);
        //---ranged weapon end---

        //---melee weapon---
        chargeMeleeSlider.value = 0f;
        chargeMeleeSlider.maxValue = maxCharge;
        chargeMeleeSlider.gameObject.SetActive(false);
        //---melee weapon end---

        //---free range weapon---
        chargeFreeRangeSlider.value = 0f;
        chargeFreeRangeSlider.maxValue = maxCharge;
        chargeFreeRangeSlider.gameObject.SetActive(false);
        //---free range weapon end---
    }

    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        //EQUIPPING
        switch (attackType)
        {
            case AttackType.melee:
                chargeProjectileSlider.gameObject.SetActive(false);
                chargeFreeRangeSlider.gameObject.SetActive(false);
                if (currentWeapon.isCharged)
                {
                    chargeMeleeSlider.gameObject.SetActive(true);
                }
                else
                {
                    chargeMeleeSlider.gameObject.SetActive(false);
                }
                break;
            case AttackType.range:
                chargeFreeRangeSlider.gameObject.SetActive(false);
                chargeMeleeSlider.gameObject.SetActive(false);
                if (currentWeapon.isCharged)
                {
                    chargeProjectileSlider.gameObject.SetActive(true);
                }
                else
                {
                    chargeProjectileSlider.gameObject.SetActive(false);
                }
                break;
            case AttackType.freeRange:
                chargeProjectileSlider.gameObject.SetActive(false);
                chargeMeleeSlider.gameObject.SetActive(false);
                if (currentWeapon.isCharged)
                {
                    chargeFreeRangeSlider.gameObject.SetActive(true);
                    originalAttackRange = currentWeapon.attackRange;
                }
                else
                {
                    chargeFreeRangeSlider.gameObject.SetActive(false);
                }
                FreeRangeWeapon frw = (FreeRangeWeapon)currentWeapon;

                if (frw.targetOverrides != null)
                {
                    freeRangeTarget.GetComponent<SetAnimations>().overrideControllers[0] = frw.targetOverrides;
                    freeRangeTarget.GetComponent<SetAnimations>().Set(0);
                }

                break;
            default:
                break;
        }

        //ATTACKING
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetButton("Fire1") && canFire && damageTimer <= 0f)
            {
                if (currentWeapon.isCharged)
                {
                    //CHARGE

                    switch (attackType)
                    {
                        case AttackType.melee:
                            //Melee - more damage and knockback
                            ChargeAttack(chargeMeleeSlider);
                            break;
                        case AttackType.range:
                            //Range - more damage and knockback
                            ChargeProjectile();
                            if (!movePenaltyActive)
                            {
                                movePenalty = -(playerStats.movementSpeed.GetValue() / chargeMoveDivider);
                                playerStats.movementSpeed.AddModifier(movePenalty);
                                movePenaltyActive = true;
                            }
                            break;
                        case AttackType.freeRange:
                            //Free Range - more damage and attackrange
                            freeRangeTarget.transform.localScale = new Vector3(attackRange, attackRange);
                            freeRangeTarget.SetActive(true);
                            ChargeAttack(chargeFreeRangeSlider);
                            break;
                        default:
                            break;
                    }

                    Debug.Log("Player charging");
                }
                else
                {
                    //AUTOMATIC

                    switch (attackType)
                    {
                        case AttackType.melee:
                            //Melee - more damage and knockback
                            Attack(playerStats.damage.GetValue());
                            break;
                        case AttackType.range:
                            //Range - more damage and speed
                            //because of projectile code VVV
                            currentCharge = baseCharge;
                            canFire = true;
                            FireProjectile();
                            break;
                        case AttackType.freeRange:
                            //Free Range - more damage and attackrange
                            //Sets target active and scales accordingly to attackRange
                            freeRangeTarget.transform.localScale = new Vector3(attackRange, attackRange);
                            freeRangeTarget.SetActive(true);
                            Attack(playerStats.damage.GetValue());
                            break;
                        default:
                            break;
                    }

                    AttackDelay();
                }
            }
            else if (Input.GetButtonUp("Fire1") && canFire)
            {
                freeRangeTarget.SetActive(false);

                //CHARGE RELEASE
                if (currentWeapon.isCharged && damageTimer <= 0f)
                {
                    switch (attackType)
                    {
                        case AttackType.melee:
                            //Melee - more damage and knockback
                            ReleaseAttack();
                            break;
                        case AttackType.range:
                            //Range - more damage and knockback
                            FireProjectile();
                            //remove slowVVV
                            playerStats.movementSpeed.RemoveModifier(movePenalty);
                            movePenaltyActive = false;
                            break;
                        case AttackType.freeRange:
                            //Free Range - more damage and attackrange
                            ReleaseAttack();
                            break;
                        default:
                            break;
                    }

                    AttackDelay();
                }
            }
            else
            {
                if (currentCharge > 0f)
                {
                    //charge decay VVV
                    currentCharge -= 0.1f;
                }
                else
                {
                    currentCharge = 0f;
                    canFire = true;
                }

                chargeProjectileSlider.value = currentCharge;
                chargeMeleeSlider.value = currentCharge;
                chargeFreeRangeSlider.value = currentCharge;
            }

        }

        //related to essence
        if (Input.GetButtonDown("EssenceAction") && essenceTimer <= 0f)
        {
            EssenceAction();
            Debug.Log(essenceType + " essence!");

            //cooldown VVV currently leads to shared cooldown across all essences
            if (gameManager.GetComponent<EquipmentManager>().GetEssence() != null)
            {
                essenceTimer = gameManager.GetComponent<EquipmentManager>().GetEssence().cooldown;

            }
        }
    }


    private void FixedUpdate()
    {
        /*
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        float attackX = Mathf.Clamp(lookDir.x, -attackRange, attackRange);
        float attackY = Mathf.Clamp(lookDir.y, -attackRange, attackRange);
        attackPoint.localPosition = new Vector3(attackX, attackY);*/

        //Attack speed timer
        if (damageTimer > 0f)
        {
            damageTimer -= Time.deltaTime;
        }

        //Essence timer
        {
            if (essenceTimer > 0f)
            {
                essenceTimer -= Time.deltaTime;
            }
        }


        //Combat
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        weapon.localPosition = new Vector3(lookDir.x, lookDir.y);
        weapon.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));
        weapon.localPosition = Vector3.ClampMagnitude(new Vector3(lookDir.x, lookDir.y), weaponPosition);

        //Combat Modes

        if (attackType == AttackType.freeRange)
        {
            //Ranged
            attackPoint.transform.parent = weapon.transform.parent;
            attackPoint.localPosition = new Vector3(lookDir.x, lookDir.y);
            //Rotates attackPoint
            attackPoint.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));
        }
        else
        {
            //Melee (child of weapon)
            attackPoint.transform.parent = weapon.transform;
            if (attackPoint.transform.position != weapon.transform.position)
            {
                attackPoint.transform.position = weapon.transform.position;
            }
            attackPoint.transform.localPosition = new Vector3(attackPoint.transform.localPosition.x, (attackRange * 0.85f) - baseAttackRange);
        }

        //Rotate point (needed?)
        //attackPoint.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));
    }

    private void Attack(int damageToDeal)
    {
        animator.SetTrigger("Attack");
        weaponAnimator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D hit in hitEnemies)
        {
            if (hit.gameObject.TryGetComponent<CharacterStats>(out CharacterStats enemy))
            {
                enemy.TakeDamage(damageToDeal);
            }
        }

        AttackDelay();
    }

    private void AttackDelay()
    {
        int attackSpeed = playerStats.attackSpeed.GetValue();
        //max attack time VVV
        float attackTime = 1f;
        float convertedAttackSpeed = attackTime - attackSpeed / 100f;
        damageTimer = convertedAttackSpeed;
    }

    private void EssenceAction()
    {
        switch (essenceType)
        {
            case EssenceType.none:
                break;
            case EssenceType.speed:
                StartCoroutine(playerStats.StatBoost(playerStats.movementSpeed, 100, 5f));
                Debug.Log("Speed up!");
                break;
            default:
                break;
        }
    }

    private void ChargeAttack(Slider slider)
    {
        //increase charge VVV
        //currentCharge += Time.deltaTime;
        currentCharge += currentWeapon.chargeRate / 100;

        float newAttackRange;
        if (attackType == AttackType.freeRange)
        {
            if (currentCharge <= maxCharge)
            {
                newAttackRange = attackRange + (currentWeapon.chargeMultiplier) * currentCharge / 100;
                attackRange = newAttackRange;
            }
        }

        slider.value = currentCharge;

        if (currentCharge > maxCharge)
        {
            slider.value = maxCharge;
            currentCharge = maxCharge;
        }

        //cap max attackrange charge for free range
    }

    private void ChargeProjectile()
    {
        projectileGFX.enabled = true;
        ChargeAttack(chargeProjectileSlider);
    }

    private void FireProjectile()
    {
        if (currentCharge > maxCharge)
        {
            currentCharge = maxCharge;
        }

        //float projectileSpeed = currentCharge + chargePower;
        float projectileSpeed = 10f;

        //rb.position -> weapon.position?
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        Quaternion rot = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));

        Projectile projectileInstance = Instantiate(projectile, weapon.position, rot).GetComponent<Projectile>();
        projectileInstance.projectileVelocity = projectileSpeed;

        int projectileDamage = playerStats.damage.GetValue() * Mathf.RoundToInt(currentCharge);
        if (projectileDamage <= 0)
        {
            projectileDamage = playerStats.damage.GetValue();
        }

        RangedWeapon rangedWeapon = (RangedWeapon)currentWeapon;
        projectileInstance.GetSpriteRenderer().sprite = rangedWeapon.projectile;
        projectileInstance.SetDamage(projectileDamage);
        projectileInstance.transform.localScale = new Vector3(projectileInstance.transform.localScale.x + currentCharge, projectileInstance.transform.localScale.y + currentCharge, projectileInstance.transform.localScale.z);

        canFire = false;
    }

    private void ReleaseAttack()
    {
        //Add knockback to melee

        if (currentCharge > maxCharge)
        {
            currentCharge = maxCharge;
        }

        int chargeDamage = playerStats.damage.GetValue() * Mathf.RoundToInt(currentCharge) * Mathf.RoundToInt(currentWeapon.chargeMultiplier);
        if (chargeDamage <= 0)
        {
            chargeDamage = playerStats.damage.GetValue();
        }

        Attack(chargeDamage);

        attackRange = originalAttackRange;

        canFire = false;
    }

    private void PlayMeleeAnimation()
    {
        


    }

    private void PrepareMeleeAnimation(Vector3 originalPos)
    {

    }

    private void ResetMeleeAnimation(Vector3 originalPos)
    {

    }

    public void SetCurrentWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
        if (currentWeapon != null)
        {
            SetAttackType(currentWeapon.attackType);
        }
    }

    public void SetAttackType(AttackType newAttackType)
    {
        attackType = newAttackType;
    }

    public void SetEssenceType(EssenceType newEssence)
    {
        essenceType = newEssence;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
