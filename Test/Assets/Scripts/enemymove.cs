using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class enemymove : MonoBehaviour
{
    public GameObject PointA;
    public GameObject PointB;
    public float speed;
    private Rigidbody2D rigidbody2;
    private Transform startingpoint;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
        startingpoint = PointB.transform;

    }

    // Update is called once per frame
    void Update()
    {
        //move to a point
        Vector2 point = startingpoint.transform.position - transform.position;
        if (startingpoint == PointB.transform)
        {
            rigidbody2.velocity = new Vector2(speed, 0);
        }
        if (startingpoint == PointA.transform)
        {
            rigidbody2.velocity = new Vector2(-speed, 0);
        }
        //making it move between two points
        if (startingpoint == PointA.transform && Vector2.Distance(transform.position, startingpoint.position) < 0.5f)
        {
            flip();
            startingpoint = PointB.transform;
        }
        if (startingpoint == PointB.transform && Vector2.Distance(transform.position, startingpoint.position) < 0.5f)
        {
            flip();
            startingpoint = PointA.transform;
        }


    }

    private void flip()  //flip the enemy
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}