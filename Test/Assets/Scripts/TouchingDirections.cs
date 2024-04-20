using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    CapsuleCollider2D touchingCol;
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.2f;
    public float ceilingDistance = 0.05f;

    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];
    private bool _isGrounded;
    
    public bool IsGrounded { get {
        return _isGrounded;
    } private set {
        _isGrounded = value;
    }}

    private bool _isOnWall;
    
    public bool IsOnWall { get {
        return _isOnWall;
    } private set {
        _isOnWall = value;
    }}
    
    private bool _isOnCeiling;
    
    public bool IsOnCeiling { get {
        return _isOnCeiling;
    } private set {
        _isOnCeiling = value;
    }}

    private UnityEngine.Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? UnityEngine.Vector2.right : UnityEngine.Vector2.left;

    private void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
       IsGrounded = touchingCol.Cast(UnityEngine.Vector2.down, castFilter, groundHits, groundDistance) > 0;
       IsOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) >0;
       IsOnCeiling = touchingCol.Cast(UnityEngine.Vector2.up, castFilter, ceilingHits, ceilingDistance) >0;
    }
}
