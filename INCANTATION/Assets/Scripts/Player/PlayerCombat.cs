using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private AttackType attackType;
    private GameManager gameManager;

    public Animator animator;

    public Transform attackPoint;
    public Transform weapon;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public PlayerStats playerStats;
    private Rigidbody2D rb;

    private Camera cam;
    Vector2 mousePos;

    //---Ranged weapon related---
    [SerializeField] private GameObject projectile;
    [SerializeField] private SpriteRenderer projectileGFX;
    [SerializeField] private Slider chargeProjectileSlider;
    //reference to weapon

    [Range(0, 10)]
    [SerializeField] private float chargePower;

    [Range(1, 4)]
    [SerializeField] private float maxCharge;

    private float currentCharge;
    private bool canFire = true;
    //---Ranged weapon end---

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStats = gameObject.GetComponent<PlayerStats>();
        cam = Camera.main;
        gameManager = GameManager.instance;

        //---ranged weapon---
        chargeProjectileSlider.value = 0f;
        chargeProjectileSlider.maxValue = maxCharge;
        //---ranged weapon end---
    }

    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (attackType == AttackType.range)
            {
                //charged attack
                if (Input.GetButton("Fire1") && canFire)
                {
                    ChargeProjectile();
                }
                else if (Input.GetButtonUp("Fire1") && canFire)
                {
                    FireProjectile();
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
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Attack();
                }
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

        //Combat
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        weapon.localPosition = new Vector3(lookDir.x, lookDir.y);
        weapon.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));
        weapon.localPosition = Vector3.ClampMagnitude(new Vector3(lookDir.x, lookDir.y), attackRange);

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
        }

        //Rotate point (needed?)
        //attackPoint.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D hit in hitEnemies)
        {
            if (hit.gameObject.TryGetComponent<CharacterStats>(out CharacterStats enemy))
            {
                enemy.TakeDamage(playerStats.damage.GetValue());
            }
        }
    }

    private void ChargeProjectile()
    {
        projectileGFX.enabled = true;
        //increase charge VVV
        currentCharge += Time.deltaTime;

        chargeProjectileSlider.value = currentCharge;

        if (currentCharge > maxCharge)
        {
            chargeProjectileSlider.value = maxCharge;
        }
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
        //Change to reflect charge
        int projectileDamage = playerStats.damage.GetValue() * (int)currentCharge;
        if (projectileDamage <= 0)
        {
            projectileDamage = playerStats.damage.GetValue();
        }

        projectileInstance.GetSpriteRenderer().sprite = gameManager.GetComponent<EquipmentManager>().GetWeapon().projectile;
        projectileInstance.SetDamage(projectileDamage);
        projectileInstance.transform.localScale = new Vector3(projectileInstance.transform.localScale.x + currentCharge, projectileInstance.transform.localScale.y + currentCharge, projectileInstance.transform.localScale.z);

        canFire = false;
        projectileGFX.enabled = false;
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
