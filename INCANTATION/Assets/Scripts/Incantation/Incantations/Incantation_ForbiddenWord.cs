using UnityEngine;

[CreateAssetMenu(fileName = "Forbidden Word", menuName = "Incantation/Forbidden Word")]
public class Incantation_ForbiddenWord : Incantation
{
    public GameObject radialIndicator;
    public float radius;

    public override void Cast()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(PlayerManager.instance.player.transform.position, radius);

        RadialIndicator riInstance = Instantiate(radialIndicator, PlayerManager.instance.player.transform.position, Quaternion.Euler(0f, 0f, 0f)).GetComponent<RadialIndicator>();

        riInstance.radius = radius;

        foreach (Collider2D col in hitEnemies)
        {
            if (col.gameObject.TryGetComponent(out EnemyStats enemy))
            {
                enemy.effectState.ApplyEffect(Effect.freeze, 3f);
            }
        }
    }
}
