using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class bulletshoot : MonoBehaviour
{
    public GameObject bullet;
    public float cooldown = 2f;
    public float force;
    public float rangee;
    

    [SerializeField] private GameObject player;
    [SerializeField] private bool canattack;
    private Rigidbody2D rb;
    private float btimer;

    // Start is called before the first frame update
    void Start()
    {
        
        
            rb = GetComponent<Rigidbody2D>();
            player = GameObject.FindGameObjectWithTag("Player");                          //direct the
            Vector3 direction = player.transform.position - transform.position;           //bullet towards
            rb.velocity = new Vector2(direction.x, direction.y).normalized * force;      //the player
       
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
        AttackPlayer();

      
    }

    void AttackPlayer()  //attacks and cooldown between attacks
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}