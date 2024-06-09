using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletshoot : MonoBehaviour
{

    public float range;
    public float cooldown;
    public float force;

    private GameObject player;
    private Rigidbody2D rb2d;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");                          //direct the
        Vector3 direction = player.transform.position - transform.position;           //attack towards
        rb2d.velocity = new Vector2(direction.x, direction.y).normalized * force;      //the player

    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(player.transform.position, transform.position);

        if (distance < range)
        {
            AttackPlayer();
        }


    }



    void AttackPlayer()  //attacks and cooldown between attacks
    {



        if (cooldown > 2)
        {

            cooldown = 0;
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}