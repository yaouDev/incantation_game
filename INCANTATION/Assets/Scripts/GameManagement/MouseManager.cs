using UnityEngine;
using UnityEngine.EventSystems;

public class MouseManager : MonoBehaviour
{
    public static MouseManager instance;

    public Transform weapon;
    public Transform attackPoint;
    public bool lockedCombat;

    private Rigidbody2D rb;
    private GameObject player;
    private GameManager gameManager;
    private PlayerCombatManager pcm;
    private Camera cam;

    private Vector2 mousePos;
    private Vector2 lookDir;
    private float angle;

    public bool lockPos;
    public bool inverseLookDir;

    private void Awake()
    {
        #region Singleton
        if(instance != null)
        {
            Debug.LogWarning("More than one Mouse Managers found");
        }
        instance = this;
        #endregion
    }

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerManager.instance.player;
        pcm = PlayerCombatManager.instance;

        cam = Camera.main;
        gameManager = GameManager.instance;
        rb = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        //Flip weapon sprite when looking left/right
        Vector2 relativePosition = mousePos - new Vector2(player.transform.position.x, player.transform.position.y);
        if (relativePosition.x < 0)
        {
            weapon.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            weapon.GetComponent<SpriteRenderer>().flipX = false;
        }

        /*
        if (Input.GetButtonDown("Fire1"))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                gameManager.isInputEnabled = false;
            }
        }
        else if (Input.GetButton("Fire1"))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                gameManager.isInputEnabled = true;
            }
        }*/

        if (EventSystem.current.IsPointerOverGameObject())
        {
            gameManager.isInputEnabled = false;
        }
        else
        {
            gameManager.isInputEnabled = true;
        }

        if (!lockPos)
        {
            

            lookDir = mousePos - rb.position;
            angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

            if (inverseLookDir)
            {
                lookDir = -lookDir;
            }

            weapon.localPosition = new Vector3(lookDir.x, lookDir.y);
            weapon.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));
            weapon.localPosition = Vector3.ClampMagnitude(new Vector3(lookDir.x, lookDir.y), pcm.currentWeaponOffset);
        }

        if (lockedCombat)
        {
            if (attackPoint.transform.position != weapon.transform.position)
            {
                attackPoint.transform.position = weapon.transform.position;
            }
            attackPoint.rotation = weapon.rotation;
        }
        else
        {
            attackPoint.position = mousePos;
        }
    }

    public Vector2 GetLookDir()
    {
        return lookDir;
    }

    public float GetAngle()
    {
        return angle;
    }

    public bool IsLookingRight()
    {
        if(lookDir.x >= 0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, 1f);
    }
}
