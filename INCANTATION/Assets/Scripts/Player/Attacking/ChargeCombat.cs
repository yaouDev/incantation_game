using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeCombat : UnlockedCombat
{/*
    [Range(0, 10)]
    [SerializeField] private float chargePower;

    [Range(1, 4)]
    [SerializeField] private float maxCharge;

    private bool movePenaltyActive;
    private int movePenalty;
    public int chargeMoveDivider = 2;

    private float currentCharge;
    private float baseCharge = 1f;

    [SerializeField] private Slider chargeSlider;

    PlayerCombat playerCombat;

    // Start is called before the first frame update
    void Start()
    {
        playerCombat = PlayerCombat.instance;

        chargeSlider.value = 0f;
        chargeSlider.maxValue = maxCharge;
        chargeSlider.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (playerCombat.isInputEnabled)
        {
            if (Input.GetButton("Fire1") && playerCombat.damageTimer <= 0f)
            {
                ChargeAttack(chargeSlider);
                if (!movePenaltyActive)
                {
                    movePenalty = -(playerCombat.playerStats.movementSpeed.GetValue() / chargeMoveDivider);
                    playerCombat.playerStats.movementSpeed.AddModifier(movePenalty);
                    movePenaltyActive = true;
                }
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                ReleaseAttack();
                playerCombat.AttackDelay();
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

                chargeSlider.value = currentCharge;
            }
        }

    }

    private void ChargeAttack(Slider slider)
    {
        //weaponAnimator.SetTrigger("Attack");

        //increase charge VVV
        //currentCharge += Time.deltaTime;
        currentCharge += playerCombat.currentWeapon.chargeRate / 30;
        playerCombat.weaponAnimator.SetFloat("Speed", currentCharge / maxCharge);

        float newAttackRange;

        //make the attack area larger VVV
        /*
        if (currentCharge <= maxCharge)
        {
            newAttackRange = attackRange + (currentWeapon.chargeMultiplier) * currentCharge / 100;
            attackRange = newAttackRange;
        }*/
    /*
        slider.value = currentCharge;

        if (currentCharge > maxCharge)
        {
            slider.value = maxCharge;
            currentCharge = maxCharge;
        }

        //cap max attackrange charge for free range
    }

    private void ReleaseAttack()
    {
        //Add knockback to melee

        if (currentCharge > maxCharge)
        {
            currentCharge = maxCharge;
        }

        int chargeDamage = Mathf.RoundToInt(playerCombat.playerStats.damage.GetValue() * currentCharge / 2 * playerCombat.currentWeapon.chargeMultiplier);
        if (chargeDamage < playerCombat.playerStats.damage.GetValue())
        {
            chargeDamage = playerCombat.playerStats.damage.GetValue();
        }

        playerCombat.Attack(chargeDamage);

        playerCombat.attackRange = playerCombat.originalAttackRange;
        if (movePenaltyActive)
        {
            playerCombat.playerStats.movementSpeed.RemoveModifier(movePenalty);
            movePenaltyActive = false;
        }
    }*/
}
