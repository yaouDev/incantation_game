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

    public GameObject[] equipmentObjects;

    private SpriteRenderer[] equipmentRenderers;
    [HideInInspector]
    public Animator[] equipmentAnimators;

    private Incantation incantation;

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    Inventory inventory;
    public GameObject player;
    private PlayerCombat playerCombat;

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
        equipmentObjects = new GameObject[numSlots];

        equipmentObjects[(int)EquipmentSlot.head] = player.transform.Find("Head").gameObject;
        equipmentObjects[(int)EquipmentSlot.chest] = player.transform.Find("Chest").gameObject;
        equipmentObjects[(int)EquipmentSlot.weapon] = player.transform.Find("Weapon").gameObject;
        equipmentObjects[(int)EquipmentSlot.legs] = player.transform.Find("Legs").gameObject;
        equipmentObjects[(int)EquipmentSlot.essence] = player.transform.Find("Essence").gameObject;

        for (int i = 0; i < equipmentObjects.Length; i++)
        {
            equipmentRenderers[i] = equipmentObjects[i].GetComponent<SpriteRenderer>();
            equipmentAnimators[i] = equipmentObjects[i].GetComponent<Animator>();
        }

        /*
        equipmentRenderers[(int)EquipmentSlot.head] = player.transform.Find("Head").GetComponent<SpriteRenderer>();
        equipmentRenderers[(int)EquipmentSlot.chest] = player.transform.Find("Chest").GetComponent<SpriteRenderer>();
        equipmentRenderers[(int)EquipmentSlot.weapon] = player.transform.Find("Weapon").GetComponent<SpriteRenderer>();
        equipmentRenderers[(int)EquipmentSlot.legs] = player.transform.Find("Legs").GetComponent<SpriteRenderer>();
        equipmentRenderers[(int)EquipmentSlot.essence] = player.transform.Find("Essence").GetComponent<SpriteRenderer>();

        equipmentAnimators[(int)EquipmentSlot.head] = player.transform.Find("Head").GetComponent<Animator>();
        equipmentAnimators[(int)EquipmentSlot.chest] = player.transform.Find("Chest").GetComponent<Animator>();
        equipmentAnimators[(int)EquipmentSlot.weapon] = player.transform.Find("Weapon").GetComponent<Animator>();
        equipmentAnimators[(int)EquipmentSlot.legs] = player.transform.Find("Legs").GetComponent<Animator>();
        equipmentAnimators[(int)EquipmentSlot.essence] = player.transform.Find("Essence").GetComponent<Animator>();*/

        incantation = player.GetComponent<Incantation>();
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

        Equipment oldItem = Unequip(slotIndex);

        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }

        if (inventory.onItemChangedCallback != null)
        {
            inventory.onItemChangedCallback.Invoke();
        }

        //Actual equip
        currentEquipment[slotIndex] = newItem;

        //Set sprite
        equipmentRenderers[slotIndex].sprite = newItem.sprite;
        
        //Set incantation
        if(newItem.specialIncantation != "")
        {
            if (incantation.allIncantations.Contains(newItem.specialIncantation))
            {
                incantation.AddIncantation(newItem.specialIncantation);
            }
            else
            {
                Debug.LogWarning("Tried to add an incantation that doesn't exist");
            }
        }

        //Set animation
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
            inventory.Add(oldItem);

            if (currentEquipment[slotIndex].equipSlot == EquipmentSlot.weapon)
            {
                playerCombat.SetAttackType(AttackType.melee);
                //change to default item? VVV
                playerCombat.SetCurrentWeapon(playerCombat.emptyWeapon);
                playerCombat.attackRange = playerCombat.baseAttackRange;
                playerCombat.attackPoint.gameObject.GetComponent<SpriteRenderer>().sprite = playerCombat.defaultAttackPointGFX;
            }
            else if (currentEquipment[slotIndex].equipSlot == EquipmentSlot.essence)
            {
                player.GetComponent<PlayerStats>().SetEssenceType(EssenceType.none);
            }

            if (equipmentRenderers[slotIndex].sprite != null)
            {
                equipmentRenderers[slotIndex].sprite = null;
            }

            equipmentAnimators[slotIndex].enabled = false;
            currentEquipment[slotIndex] = null;

            if(oldItem.specialIncantation != "")
            {
                //possible problem if the items have the same incantation
                if (incantation.currentIncantations.Contains(oldItem.specialIncantation))
                {
                    incantation.RemoveIncantation(oldItem.specialIncantation);
                }
                else
                {
                    Debug.LogWarning("Tried to remove an incantation that wasn't in use");
                }
            }
            
            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldItem);
            }

            EquipDefaultItems();

            return oldItem;
        }

        if (inventory.isFull)
        {
            Debug.Log("Cannot unequip; inventory was full");
        }
        return null;
    }

    //obsolete!!! doesnt updateUI
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
            if(currentEquipment[(int)e.equipSlot] == null)
            {
                Equip(e);
            }
        }
    }

    private void Update()
    {
        //Insert input for unequip all
        if (Input.GetKeyDown(KeyCode.U) && !GetComponent<GameManager>().chatBox.isFocused)
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
