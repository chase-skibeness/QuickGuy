using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordControllerV2 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("hit");

            // Calculate direction from player to enemy
            Vector2 direction = collision.transform.position - transform.parent.position;
            direction = direction.normalized; // Normalize the direction vector

            // Apply force in the calculated direction
            collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction.x * 5, 4), ForceMode2D.Impulse);
        }
    }
}

