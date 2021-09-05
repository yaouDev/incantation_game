using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Spin : WeaponAttack
{
    public float radius = 1.5f;
    public GameObject hitbox;

    private float rotateSpeed = 5f;

    private Vector2 center;
    private float angle;

    private void Start()
    {
        InitializeWeapon();

        hitbox.SetActive(false);
        rotateSpeed = playerStats.attackSpeed.GetValue() / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            //spawn animation
            hitbox.SetActive(true);
        }

        if (Input.GetButton("Fire1"))
        {
            //hitbox.transform.localPosition = new Vector3(hitbox.transform.localPosition.x, attackRange);
            Spin();
        }

        if (Input.GetButtonUp("Fire1"))
        {
            //despawn animation
            hitbox.SetActive(false);
        }
    }

    private void Spin()
    {
        center = pcm.player.transform.position;
        angle += rotateSpeed * Time.deltaTime;

        var offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
        hitbox.transform.position = center + offset;
    }
}
