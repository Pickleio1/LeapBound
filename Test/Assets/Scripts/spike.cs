using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public float extendSpeed = 5f;
    public float extendDistance = 3f;
    public enum Direction { Up, Down, Left, Right }
    public Direction extendDirection;
    private Vector3 originalPosition;
    private bool isOperating = false;

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isOperating)
        {
            StartCoroutine(HandleSpikeExtension());
            collision.gameObject.GetComponent<heartsgopoof>()?.TakeDamage(1); 
        }
    }

    private IEnumerator HandleSpikeExtension()
    {
        isOperating = true;   
        yield return MoveSpike(originalPosition + GetExtensionVector(extendDirection));  
        yield return new WaitForSeconds(2);  
        yield return MoveSpike(originalPosition); 
        isOperating = false; 
    }

    private IEnumerator MoveSpike(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, extendSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private Vector3 GetExtensionVector(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:    return new Vector3(0, extendDistance, 0);
            case Direction.Down:  return new Vector3(0, -extendDistance, 0);
            case Direction.Left:  return new Vector3(-extendDistance, 0, 0);
            case Direction.Right: return new Vector3(extendDistance, 0, 0);
            default:              return Vector3.zero;
        }
    }
}