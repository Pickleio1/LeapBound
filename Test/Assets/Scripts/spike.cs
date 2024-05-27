using UnityEngine;
using System.Collections;

public class Spike : MonoBehaviour
{
    public float extendSpeed = 5f;
    public float extendDistance = 3f;
    public enum Direction { Up, Down, Left, Right }
    public Direction extendDirection;
    private Vector3 originalPosition;
    public bool isOperating = false;

    void Start()
    {
        originalPosition = transform.position;
    }

    public void ActivateSpike()
    {
        if (!isOperating)
        {
            StartCoroutine(HandleSpikeExtension());
        }
    }
    
    public IEnumerator HandleSpikeExtension()
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
            case Direction.Up:
                return Vector3.up * extendDistance;
            case Direction.Down:
                return Vector3.down * extendDistance;
            case Direction.Left:
                return Vector3.left * extendDistance;
            case Direction.Right:
                return Vector3.right * extendDistance;
            default:
                return Vector3.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<heartsgopoof>().TakeDamage(1);
        }
    }
}