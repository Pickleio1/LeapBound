using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class bulletshoot : MonoBehaviour
{
    public GameObject bullet;
    public float cooldown = 2f;
    public float force;
    public GameObject enemy;

    [SerializeField] private GameObject player;
    [SerializeField] private bool canattack;
    private Rigidbody2D rb;

    public enemyhealth enemyhealth;

    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2 (direction.x, direction.y).normalized * force;
    }

    // Update is called once per frame
    void Update()
    {
        
         AttackPlayer();

        

    }


    
     void AttackPlayer()
    {


        if (cooldown > 2)
        {
            canattack = false;
            cooldown = 0;
        }
        else
        {
            canattack = true;
            Vector2 direction = (player.transform.position - transform.position);

        }


    }

    void ResetCooldown()
    {
        canattack = true;
    }

    
}