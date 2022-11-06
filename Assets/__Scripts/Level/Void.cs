using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Void : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.collider.GetComponent<Enemy>();
        PlayerSystem player = collision.gameObject.GetComponent<PlayerSystem>();

        if (enemy != null)
        {
            enemy.DamageEnemy(1000000, "void");
        }
        else if (player != null)
        {
            player.DamagePlayer(1000000, "void");
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        Enemy enemy = collider.GetComponent<Enemy>();
        PlayerSystem player = collider.GetComponent<PlayerSystem>();

        if (enemy != null)
        {
            enemy.DamageEnemy(1000000, "void");
        }
        else if (player != null)
        {
            player.DamagePlayer(1000000, "void");
        }
    }
}
