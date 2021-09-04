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

// this script:
// handele basic things such as whether or not attacking is possible

public class PlayerCombat : MonoBehaviour
{
    public static PlayerCombat instance;

    public Animator animator;
    public Animator weaponAnimator;
    public bool isInputEnabled = true;

    public Transform attackPoint;
    public Sprite defaultAttackPointGFX;
    public Transform weapon;

    public float originalAttackRange;
    public float attackRange = 1f;
    public float baseAttackRange = 1f;
    //space from body of playerVVV
    [SerializeField] public float weaponPosition = 1.5f;

    public LayerMask enemyLayers;
    public PlayerStats playerStats;
    public Rigidbody2D rb;

    public Camera cam;
    public Vector2 mousePos;
    public Vector2 lookDir;

    public float damageTimer = 0f;

    public Weapon currentWeapon;

    public Weapon emptyWeapon;

    private void Awake()
    {
        #region Singleton
        if(instance != null)
        {
            Debug.LogWarning("More than one Player Combat");
        }
        instance = this;
        #endregion
    }

    private void Start()
    {
        rb = PlayerManager.instance.player.GetComponent<Rigidbody2D>();
        playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        cam = Camera.main;
        currentWeapon = emptyWeapon;
        originalAttackRange = attackRange;
        attackPoint.gameObject.GetComponent<SpriteRenderer>().sprite = defaultAttackPointGFX;
        weaponAnimator = weapon.GetComponent<Animator>();

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

        lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        weapon.localPosition = new Vector3(lookDir.x, lookDir.y);
        weapon.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));
        weapon.localPosition = Vector3.ClampMagnitude(new Vector3(lookDir.x, lookDir.y), weaponPosition);
    }


    private void FixedUpdate()
    {
        //Attack speed timer
        if (damageTimer > 0f)
        {
            damageTimer -= Time.deltaTime;
        }
    }

    public void Attack(int damageToDeal)
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
        //VVV hard coded knockback duration FIX!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //StartCoroutine(playerStats.Knockback(0.1f, currentWeapon.knockbackPlayerPower, weapon.transform));
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

    public void AttackDelay()
    {
        int attackSpeed = playerStats.attackSpeed.GetValue();
        //max attack time VVV
        float attackTime = 1f;
        float convertedAttackSpeed = attackTime - attackSpeed / 100f;
        damageTimer = convertedAttackSpeed;
    }

    void UpdateAttackType(Item newItem, Item oldItem)
    {
        //Handle attack range and attackpoint rotation VVV
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
