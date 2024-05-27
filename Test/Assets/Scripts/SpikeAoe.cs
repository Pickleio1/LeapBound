using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeAoe : MonoBehaviour
{
    private Spike spike;

    private void Start()
    {
        spike = GetComponentInParent<Spike>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !spike.isOperating)
        {
            StartCoroutine(spike.HandleSpikeExtension());
        }
    }
}