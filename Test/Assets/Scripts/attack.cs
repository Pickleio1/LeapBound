using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class attack : MonoBehaviour
{
    public GameObject bullettt;
    public Transform bulletposition;
    public float range;
    public Animator animator;
    private float spawntime;
    private GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        spawntime += Time.deltaTime;
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < range)
        {
            if (spawntime > 2)
            {
                spawntime = 0;
                copyshoot();
            }
        }
    }

    void copyshoot()
    {
        animator.SetTrigger(AnimationStrings.enemyAttack);
        Time.timeScale = 1f;
        Instantiate(bullettt, bulletposition.position, Quaternion.identity);

    }

}