using UnityEngine;
using UnityEngine.EventSystems;

public class MouseManager : MonoBehaviour
{
    public Transform weapon;
    public Transform attackPoint;
    public bool lockedCombat;
    [SerializeField] private float weaponOffset;

    private Rigidbody2D rb;
    private GameObject player;
    private GameManager gameManager;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerManager.instance.player;

        cam = Camera.main;
        gameManager = GameManager.instance;
        rb = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

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
        }

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        weapon.localPosition = new Vector3(lookDir.x, lookDir.y);
        weapon.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));
        weapon.localPosition = Vector3.ClampMagnitude(new Vector3(lookDir.x, lookDir.y), weaponOffset);

        if (lockedCombat)
        {
            attackPoint.parent = weapon.transform;

            if (attackPoint.transform.position != weapon.transform.position)
            {
                attackPoint.transform.position = weapon.transform.position;
            }
            attackPoint.transform.localPosition = new Vector3(attackPoint.transform.localPosition.x, (PlayerCombatManager.instance.attackRange * 0.85f) - PlayerCombatManager.instance.baseAttackRange);
        }
        else
        {
            if(attackPoint.parent == weapon.transform)
            {
                attackPoint.parent = player.transform;
            }

            attackPoint.position = mousePos;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, PlayerCombatManager.instance.attackRange);
    }
}
