using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemymove : MonoBehaviour
{
    public GameObject PointA;
    public GameObject PointB;
    public float speed;
    private Rigidbody2D rigidbody2;
    private Transform startingpoint;
    private Transform startingpoint2;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
        startingpoint = PointB.transform;
        startingpoint2 = PointA.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 point = startingpoint.position - transform.position;
        if (startingpoint == PointB.transform)
        {
            transform.position = Vector2.MoveTowards(transform.position, PointA.transform.position, speed * Time.deltaTime);

        }
        else
        {
            startingpoint = startingpoint2;
        }



    }
}
