using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public GameObject healthBearer;

    private Slider healthbar;
    private CharacterStats charStats;

    void Start()
    {
        healthbar = GetComponent<Slider>();
        charStats = healthBearer.GetComponent<CharacterStats>();

        charStats.onDamageTakenCallback += UpdateHealth;
    }

    void UpdateHealth()
    {
        healthbar.maxValue = charStats.maxHealth.GetValue();
        healthbar.value = charStats.GetCurrentHealth();
    }
}
