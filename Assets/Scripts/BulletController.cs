using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject player;

    private SpriteRenderer playerSR;
    public float speed = 10f;

    void Start()
    {
        player = GameObject.Find("Player");
        playerSR = player.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Move the bullet horizontally
        transform.Translate(((playerSR.flipX) ? Vector2.left : Vector2.right) * speed * Time.deltaTime);

        // Destroy the bullet if it goes off-screen
        if (!GetComponent<Renderer>().isVisible)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
