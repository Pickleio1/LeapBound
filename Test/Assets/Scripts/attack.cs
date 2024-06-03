using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class attack : MonoBehaviour
{
    public GameObject bullettt;
    public Transform bulletposition;
    public float range;

    private float spawntime;
    private GameObject player;
    private float btimer;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        spawntime += Time.deltaTime;
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < range)   // r a n g e
        {
            if (spawntime > 2)   //spawns bullet
            {
                spawntime = 0;
                copyshoot();
            }
        }
    }

    void copyshoot()   //copy the bullet to shoot more bullets
    {
        Instantiate(bullettt, bulletposition.position, Quaternion.identity);

        
    }

}
