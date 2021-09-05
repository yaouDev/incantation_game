using UnityEngine;
using UnityEngine.EventSystems;

public abstract class WeaponAttack : MonoBehaviour
{
    public static WeaponAttack instance;
    //public bool inputEnable = true;

    protected PlayerCombatManager pcm;
    protected bool canAttack;
    protected Transform attackPoint;
    protected LayerMask enemyLayers;
    protected PlayerStats playerStats;

    private void Awake()
    {
        #region Singleton
        if (instance != null)
        {
            Debug.LogWarning("Found more than one special weapon");
        }
        instance = this;
        #endregion
    }

    private void Start()
    {
        pcm = PlayerCombatManager.instance;
        attackPoint = pcm.attackPoint;
        enemyLayers = pcm.enemyLayers;
        playerStats = playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
    }

    private void Update()
    {
        //VVV insert things like damagetimer
        if (GameManager.instance.isInputEnabled)
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }

        //VVV Already exists in MouseManager?
        /*
        if (EventSystem.current.IsPointerOverGameObject())
        {
            inputEnable = true;
        }
        else
        {
            inputEnable = false;
        }*/
    }
}
