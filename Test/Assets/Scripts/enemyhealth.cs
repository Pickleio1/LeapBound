using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyhealth : MonoBehaviour
{
    public int maxhealth = 3;
    private int currenthealth;
    public GameObject playerbullet;

    // Start is called before the first frame update
    void Start()
    {
        currenthealth = maxhealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage) //dmg and destroy enemy
    {
        currenthealth -= damage;
        if (currenthealth < 0)
        {
            Destroy(gameObject);
        }
    }

}
