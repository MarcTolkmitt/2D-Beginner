using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Projectile : MonoBehaviour
{
    Rigidbody2D thisRiBody;
    public float seconds = 5f;
    float lifeLength;

    // Awake happens on Prefab init before Start()
    void Awake()
    {
        thisRiBody = GetComponent<Rigidbody2D>();
        lifeLength = Time.time;

    }   // Ende: void Awake

    void Update( )
    {
        // a countdown for the lifespan is better
        lifeLength -= Time.deltaTime;
        if ( lifeLength < 0 )
        {
            Destroy( gameObject );

        }

    }   // Ende: void Update

    public void Launch( Vector2 direction, float force )
    {
        thisRiBody.AddForce( direction * force );

    }   // Ende: public void Launch

    void OnCollisionEnter2D( Collision2D collision )
    {
        //Debug.Log( "Projectile collision with " + collision.gameObject );
        EnemyController enemy = 
            collision.gameObject.GetComponent<EnemyController>();
        if ( enemy != null )
        {
            enemy.Fix( );

        }
        Destroy( gameObject );

    }   // Ende: void OnCollisionEnter2D

}   // Ende: public class Projectile

