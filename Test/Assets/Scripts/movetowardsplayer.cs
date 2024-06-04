using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movetowardsplayer : MonoBehaviour
{
    public float speed;
    public float force;

    private GameObject player;
    private Rigidbody2D rb2d;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");                          
        Vector3 direction = player.transform.position - transform.position;          
        rb2d.velocity = new Vector2(direction.x, direction.y).normalized * force;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = (player.transform.position - transform.position); 
    }
}
