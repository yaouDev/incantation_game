using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : CharacterStats
{
    public Transform spawnPoint;
    [SerializeField] Slider healthbar;

    private float essenceTimer = 0f;
    private EssenceType essenceType = EssenceType.none;

    //constants
    [SerializeField] private int baseDamage = 2;
    [SerializeField] private int baseArmor = 0;
    [SerializeField] private int baseAttackSpeed = 10;
    [SerializeField] private int baseMovementSpeed = 75;
    [SerializeField] private int baseMaxHealth = 200;

    void Start()
    {
        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
        this.onDamageTakenCallback += UpdateHealth;

        SetBaseStats();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //related to essence
        if (Input.GetButtonDown("EssenceAction") && essenceTimer <= 0f && !GameManager.instance.chatBox.isFocused)
        {
            EssenceAction();
            Debug.Log(essenceType + " essence!");

            //cooldown VVV currently leads to shared cooldown across all essences
            if (EquipmentManager.instance.GetEssence() != null)
            {
                essenceTimer = EquipmentManager.instance.GetEssence().cooldown;
            }
        }
    }

    private void FixedUpdate()
    {
        //Essence timer
        {
            if (essenceTimer > 0f)
            {
                essenceTimer -= Time.deltaTime;
            }
        }
    }

    private void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if (newItem != null)
        {
            if (newItem is Essence)
            {
                Essence newEssence = (Essence)newItem;
                essenceType = newEssence.essenceType;
            }

            armor.AddModifier(newItem.armorModifer);
            damage.AddModifier(newItem.damageModifier);
            movementSpeed.AddModifier(newItem.moveSpeedModifier);
            attackSpeed.AddModifier(newItem.attackSpeedModifier);
        }

        if (oldItem != null)
        {
            armor.RemoveModifier(oldItem.armorModifer);
            damage.RemoveModifier(oldItem.damageModifier);
            movementSpeed.RemoveModifier(oldItem.moveSpeedModifier);
            attackSpeed.RemoveModifier(oldItem.attackSpeedModifier);
        }
    }

    private void EssenceAction()
    {
        switch (essenceType)
        {
            case EssenceType.none:
                break;
            case EssenceType.speed:
                StartCoroutine(StatBoost(movementSpeed, 100, 5f));
                Debug.Log("Speed up!");
                break;
            default:
                break;
        }
    }

    public void SetBaseStats()
    {
        damage.SetBaseValue(baseDamage);
        armor.SetBaseValue(baseArmor);
        attackSpeed.SetBaseValue(baseAttackSpeed);
        movementSpeed.SetBaseValue(baseMovementSpeed);
        //...and health
        maxHealth.SetBaseValue(baseMaxHealth);

        currentHealth = maxHealth.GetValue();
    }

    public void SetEssenceType(EssenceType newEssence)
    {
        essenceType = newEssence;
    }

    void UpdateHealth()
    {
        healthbar.maxValue = maxHealth.GetValue();
        healthbar.value = currentHealth;
    }

    public override void Die()
    {
        transform.position = spawnPoint.position;
        Heal(maxHealth.GetValue());
    }
}
