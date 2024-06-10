using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemydealdmg : MonoBehaviour
{
    public heartsgopoof playerhealth;
    public int damage = 1;
    public bulletshoot bullet;
    private bool canDealDamage = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DisableDamage());
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && canDealDamage)
        {
            playerhealth.TakeDamage(damage);
        }
    }

    private IEnumerator DisableDamage()
    {
        yield return new WaitForSeconds(5f);
        canDealDamage = false;
    }
}