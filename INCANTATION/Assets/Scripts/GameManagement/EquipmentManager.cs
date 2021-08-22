using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{

    public static EquipmentManager instance;

    //look more into this laterVVV
    public Equipment[] defaultItems;
    [ReadOnly]
    public Equipment[] currentEquipment;

    [SerializeField] private EquipmentUI equipmentUI;

    private SpriteRenderer[] equipmentRenderers;
    [HideInInspector]
    public Animator[] equipmentAnimators;

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    Inventory inventory;
    public GameObject player;
    private PlayerCombat playerCombat;

    [HideInInspector]
    public bool replace;

    private void Awake()
    {
        #region Singleton
        if (instance != null)
        {
            Debug.LogWarning("More than one equipment manager found!");
        }
        instance = this;
        #endregion

        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;

        currentEquipment = new Equipment[numSlots];
        equipmentRenderers = new SpriteRenderer[numSlots];
        equipmentAnimators = new Animator[numSlots];

        equipmentRenderers[(int)EquipmentSlot.head] = player.transform.Find("Head").GetComponent<SpriteRenderer>();
        equipmentRenderers[(int)EquipmentSlot.chest] = player.transform.Find("Chest").GetComponent<SpriteRenderer>();
        equipmentRenderers[(int)EquipmentSlot.weapon] = player.transform.Find("Weapon").GetComponent<SpriteRenderer>();
        equipmentRenderers[(int)EquipmentSlot.legs] = player.transform.Find("Legs").GetComponent<SpriteRenderer>();
        equipmentRenderers[(int)EquipmentSlot.essence] = player.transform.Find("Essence").GetComponent<SpriteRenderer>();

        equipmentAnimators[(int)EquipmentSlot.head] = player.transform.Find("Head").GetComponent<Animator>();
        equipmentAnimators[(int)EquipmentSlot.chest] = player.transform.Find("Chest").GetComponent<Animator>();
        equipmentAnimators[(int)EquipmentSlot.weapon] = player.transform.Find("Weapon").GetComponent<Animator>();
        equipmentAnimators[(int)EquipmentSlot.legs] = player.transform.Find("Legs").GetComponent<Animator>();
        equipmentAnimators[(int)EquipmentSlot.essence] = player.transform.Find("Essence").GetComponent<Animator>();
    }

    private void Start()
    {
        inventory = Inventory.instance;
        playerCombat = player.GetComponent<PlayerCombat>();

        for (int i = 0; i < equipmentAnimators.Length; i++)
        {
            if (currentEquipment[i] == null)
            {
                equipmentAnimators[i].enabled = false;
            }
        }

        EquipDefaultItems();
    }

    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipSlot;

        if(currentEquipment[slotIndex] != null)
        {
            replace = true;
        }
        else
        {
            replace = false;
        }

        Equipment oldItem = Unequip(slotIndex);

        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }

        currentEquipment[slotIndex] = newItem;
        equipmentRenderers[slotIndex].sprite = newItem.sprite;
        equipmentUI.slots[slotIndex].AddItem(newItem);

        if (replace)
        {
            print("enter");
            inventory.AddOnIndexOf(inventory.latestIndex, oldItem);
        }

        if (newItem.animatorOverride == null)
        {
            equipmentAnimators[slotIndex].enabled = false;
        }
        else if (newItem.animatorOverride.Length > 0)
        {
            equipmentAnimators[slotIndex].enabled = true;
            equipmentAnimators[slotIndex].gameObject.GetComponent<SetAnimations>().overrideControllers = new AnimatorOverrideController[newItem.animatorOverride.Length];
            for (int i = 0; i < newItem.animatorOverride.Length; i++)
            {
                equipmentAnimators[slotIndex].gameObject.GetComponent<SetAnimations>().overrideControllers[i] = newItem.animatorOverride[i];
            }
            equipmentAnimators[slotIndex].gameObject.GetComponent<SetAnimations>().Set((int)AnimationVariation.none);
        }
        else if (newItem.sprite == null)
        {
            Debug.LogWarning(newItem.name + " has no graphics!");
        }
    }

    public Equipment Unequip(int slotIndex)
    {
        if (currentEquipment[slotIndex] != null && !inventory.isFull)
        {
            Equipment oldItem = currentEquipment[slotIndex];

            if (!replace)
            {
                inventory.Add(oldItem);
            }

            if (currentEquipment[slotIndex].equipSlot == EquipmentSlot.weapon)
            {
                playerCombat.SetAttackType(AttackType.melee);
                //change to default item? VVV
                playerCombat.SetCurrentWeapon(playerCombat.emptyWeapon);
                playerCombat.attackRange = playerCombat.baseAttackRange;
            }
            else if (currentEquipment[slotIndex].equipSlot == EquipmentSlot.essence)
            {
                playerCombat.SetEssenceType(EssenceType.none);
            }

            if (equipmentRenderers[slotIndex].sprite != null)
            {
                equipmentRenderers[slotIndex].sprite = null;
            }

            equipmentAnimators[slotIndex].enabled = false;
            equipmentUI.slots[slotIndex].ClearSlot();

            currentEquipment[slotIndex] = null;

            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldItem);
            }

            return oldItem;
        }

        if (inventory.isFull)
        {
            Debug.Log("Cannot unequip; inventory was full");
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
        foreach (Equipment e in defaultItems)
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

    public Animator[] GetEquipmentAnimators()
    {
        return equipmentAnimators;
    }
}

public enum AnimationVariation
{
    none, superAttack
}
