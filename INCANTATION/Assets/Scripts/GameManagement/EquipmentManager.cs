using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{

    #region Singleton

    public static EquipmentManager instance;

    private void Awake()
    {

        if (instance != null)
        {
            Debug.LogWarning("More than one equipment manager found!");
        }
        instance = this;
    }

    #endregion

    //look more into this laterVVV
    public Equipment[] defaultItems;
    [SerializeField] private Equipment[] currentEquipment;

    private SpriteRenderer[] equipmentRenderers;
    private Animator weaponAnimator;

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    Inventory inventory;
    public GameObject player;
    private PlayerCombat playerCombat;

    private void Start()
    {
        inventory = Inventory.instance;
        playerCombat = player.GetComponent<PlayerCombat>();

        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
        equipmentRenderers = new SpriteRenderer[numSlots];

        equipmentRenderers[(int)EquipmentSlot.head] = player.transform.Find("Head").GetComponent<SpriteRenderer>();
        equipmentRenderers[(int)EquipmentSlot.chest] = player.transform.Find("Chest").GetComponent<SpriteRenderer>();
        equipmentRenderers[(int)EquipmentSlot.weapon] = player.transform.Find("Weapon").GetComponent<SpriteRenderer>();
        weaponAnimator = player.transform.Find("Weapon").GetComponent<Animator>();
        equipmentRenderers[(int)EquipmentSlot.legs] = player.transform.Find("Legs").GetComponent<SpriteRenderer>();
        equipmentRenderers[(int)EquipmentSlot.essence] = player.transform.Find("Essence").GetComponent<SpriteRenderer>();

        EquipDefaultItems();
    }

    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipSlot;

        Equipment oldItem = Unequip(slotIndex);

        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }

        currentEquipment[slotIndex] = newItem;
        equipmentRenderers[slotIndex].sprite = newItem.sprite;

        if(newItem is Weapon)
        {
            Weapon newWeapon = (Weapon)newItem;

            if(newWeapon.animatorOverride == null)
            {
                weaponAnimator.enabled = false;
            }
            else
            {
                weaponAnimator.enabled = true;
                weaponAnimator.gameObject.GetComponent<SetAnimations>().overrideControllers[0] = newWeapon.animatorOverride;
                weaponAnimator.gameObject.GetComponent<SetAnimations>().Set(0);
            }
        }

    }

    public Equipment Unequip(int slotIndex)
    {
        if (currentEquipment[slotIndex] != null)
        {
            if(currentEquipment[slotIndex].equipSlot == EquipmentSlot.weapon)
            {
                playerCombat.SetAttackType(AttackType.melee);
                //change to default item? VVV
                playerCombat.SetCurrentWeapon(playerCombat.emptyWeapon);
                playerCombat.attackRange = playerCombat.baseAttackRange;
            }
            else if(currentEquipment[slotIndex].equipSlot == EquipmentSlot.essence)
            {
                playerCombat.SetEssenceType(EssenceType.none);
            }

            if (equipmentRenderers[slotIndex].sprite != null)
            {
                equipmentRenderers[slotIndex].sprite = null;
            }

            Equipment oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);

            currentEquipment[slotIndex] = null;

            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldItem);
            }

            return oldItem;
        }
        return null;
    }

    //obsolete?
    public void UnequipAll()
    {
        for (int i = 0; i < currentEquipment.Length; i++)
        {
            Unequip(i);
        }
        EquipDefaultItems();
    }

    private void EquipDefaultItems()
    {
        foreach(Equipment e in defaultItems)
        {
            Equip(e);
        }
    }

    private void Update()
    {
        //Insert input for unequip all
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnequipAll();
        }
    }

    public Weapon GetWeapon()
    {
        Weapon getWeapon = (Weapon)currentEquipment[(int)EquipmentSlot.weapon];
        return getWeapon;
    }

    public Essence GetEssence()
    {
        Essence getEssence = (Essence)currentEquipment[(int)EquipmentSlot.essence];
        return getEssence;
    }
}
