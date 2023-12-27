using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            // Calculate direction from player to enemy
            Vector2 direction = collision.transform.position - transform.parent.position;
            direction = direction.normalized; // Normalize the direction vector

            // Apply force in the calculated direction
            Rigidbody2D enemyRB = collision.GetComponent<Rigidbody2D>();
            enemyRB.velocity = Vector2.zero;
            enemyRB.AddForce(new Vector2(direction.x * 2, 2), ForceMode2D.Impulse);

            collision.GetComponent<PController2>().ApplyDamage(1);
        }
    }
}
