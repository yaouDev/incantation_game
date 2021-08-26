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

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private AttackType attackType;
    private GameManager gameManager;

    public Animator animator;
    private Animator weaponAnimator;
    private bool isInputEnabled = true;

    public Transform attackPoint;
    public Sprite defaultAttackPointGFX;
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
        attackPoint.gameObject.GetComponent<SpriteRenderer>().sprite = defaultAttackPointGFX;
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

        EquipmentManager.instance.onEquipmentChanged += UpdateAttackType;
    }

    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        //Flip weapon sprite when looking left/right
        Vector2 relativePosition = mousePos - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        if (relativePosition.x < 0)
        {
            weapon.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            weapon.GetComponent<SpriteRenderer>().flipX = false;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                isInputEnabled = false;
            }
        }
        else if (Input.GetButton("Fire1"))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                isInputEnabled = true;
            }
        }

        //ATTACKING
        if (isInputEnabled)
        {
            if (Input.GetButton("Fire1") && damageTimer <= 0f)
            {
                if (!currentWeapon.isCharged)
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
            else if (Input.GetButtonUp("Fire1"))
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
                            if (movePenaltyActive)
                            {
                                playerStats.movementSpeed.RemoveModifier(movePenalty);
                                movePenaltyActive = false;
                            }
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
                }

                chargeProjectileSlider.value = currentCharge;
                chargeMeleeSlider.value = currentCharge;
                chargeFreeRangeSlider.value = currentCharge;
            }

        }

        //Combat
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        weapon.localPosition = new Vector3(lookDir.x, lookDir.y);
        weapon.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));
        weapon.localPosition = Vector3.ClampMagnitude(new Vector3(lookDir.x, lookDir.y), weaponPosition);

        //Combat Modes

        if (attackType == AttackType.freeRange || attackType == AttackType.range)
        {
            attackPoint.localPosition = new Vector3(lookDir.x, lookDir.y);
        }
        else
        {
            //Melee (child of weapon)
            if (attackPoint.transform.position != weapon.transform.position)
            {
                attackPoint.transform.position = weapon.transform.position;
            }
            attackPoint.transform.localPosition = new Vector3(attackPoint.transform.localPosition.x, (attackRange * 0.85f) - baseAttackRange);
        }

        //Rotate point (needed?)
        //attackPoint.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));
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

        if (isInputEnabled)
        {
            if (Input.GetButton("Fire1"))
            {
                if (damageTimer <= 0f)
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
                    }
                }
            }
        }
    }

    private void Attack(int damageToDeal)
    {
        weaponAnimator.SetFloat("Speed", 0f);
        weaponAnimator.SetTrigger("Attack");

        animator.SetTrigger("Attack");

        //if(currentWeapon.customAttack != null)
        //{
        //currentWeapon.customAttack.Attack(damageToDeal);
        //}
        //else
        //{
        DefaultAttack(damageToDeal);
        //}

        AttackDelay();
    }

    private void DefaultAttack(int damageToDeal)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D hit in hitEnemies)
        {
            if (hit.gameObject.TryGetComponent<CharacterStats>(out CharacterStats enemy))
            {
                enemy.TakeDamage(damageToDeal, currentWeapon.knockbackPower, transform);
            }
        }
    }

    private void AttackDelay()
    {
        int attackSpeed = playerStats.attackSpeed.GetValue();
        //max attack time VVV
        float attackTime = 1f;
        float convertedAttackSpeed = attackTime - attackSpeed / 100f;
        damageTimer = convertedAttackSpeed;
    }

    private void ChargeAttack(Slider slider)
    {
        //weaponAnimator.SetTrigger("Attack");

        //increase charge VVV
        //currentCharge += Time.deltaTime;
        currentCharge += currentWeapon.chargeRate / 30;
        weaponAnimator.SetFloat("Speed", currentCharge / maxCharge);

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

        float projectileSpeed = 10f;
        //float projectileSpeed = currentCharge + chargePower;
        RangedWeapon rangedWeapon = (RangedWeapon)currentWeapon;
        projectileSpeed = rangedWeapon.projectileSpeed;

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

        //object is knocked back in the short space that it is dynamic bc of impact physics

        projectileInstance.GetSpriteRenderer().sprite = rangedWeapon.projectile;
        projectileInstance.knockbackPower = rangedWeapon.knockbackPower;
        projectileInstance.SetDamage(projectileDamage);
        projectileInstance.transform.localScale = new Vector3(projectileInstance.transform.localScale.x + currentCharge, projectileInstance.transform.localScale.y + currentCharge, projectileInstance.transform.localScale.z);
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
    }

    void UpdateAttackType(Item newItem, Item oldItem)
    {
        if (movePenaltyActive)
        {
            playerStats.movementSpeed.RemoveModifier(movePenalty);
            movePenaltyActive = false;
        }

        if (newItem is Weapon)
        {
            Weapon newWeapon = (Weapon)newItem;
            SetCurrentWeapon(newWeapon);
            attackRange = newWeapon.attackRange;
            if (newWeapon.attackPointGFX != null)
            {
                attackPoint.gameObject.GetComponent<SpriteRenderer>().sprite = newWeapon.attackPointGFX;
            }
            else
            {
                attackPoint.gameObject.GetComponent<SpriteRenderer>().sprite = defaultAttackPointGFX;
            }

            

            switch (newWeapon.attackType)
            {
                case AttackType.melee:
                    attackPoint.transform.parent = weapon.transform;

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
                    attackPoint.transform.parent = weapon.transform.parent;

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
                    attackPoint.transform.parent = weapon.transform.parent;
                    attackPoint.transform.localScale = attackPoint.GetComponent<PopUp>().startSize;

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

            //Set attackPoint spin
            if (newWeapon.attackPointSpin)
            {
                attackPoint.GetComponent<Rotate>().isSpinning = true;
                attackPoint.GetComponent<Rotate>().isUsedForCombat = true;
            }
            else
            {
                attackPoint.GetComponent<Rotate>().isSpinning = false;
                attackPoint.GetComponent<Rotate>().isUsedForCombat = false;
                attackPoint.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            }
        }
        else
        {
            return;
        }
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
