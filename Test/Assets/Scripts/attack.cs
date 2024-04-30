using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class attack : MonoBehaviour
{
    public GameObject bullettt;
    public Transform bulletposition;

    private float spawntime;

    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
       spawntime += Time.deltaTime;

        if (spawntime > 2)
        {
            spawntime = 0;
            copyshoot();
        }
    }

    void copyshoot()
    {
        Instantiate(bullettt, bulletposition.position, Quaternion.identity);
    }

}
