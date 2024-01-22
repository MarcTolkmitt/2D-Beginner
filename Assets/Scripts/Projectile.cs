using UnityEngine;

/// <summary>
/// script for the repair magic to happen
/// </summary>
public class Projectile : MonoBehaviour
{
    Rigidbody2D thisRiBody;
    public float seconds = 5f;
    float lifeLength;

    /// <summary>
    /// Awake happens on Prefab init before Start()
    /// </summary>
    void Awake()
    {
        thisRiBody = GetComponent<Rigidbody2D>();
        lifeLength = Time.time;

    }   // end: void Awake

    /// <summary>
    /// implementing a timer for the Prefabs lifetime
    /// </summary>
    void Update( )
    {
        // a countdown for the lifespan is better
        lifeLength -= Time.deltaTime;
        if ( lifeLength < 0 )
        {
            Destroy( gameObject );

        }

    }   // end: void Update

    /// <summary>
    /// after the creation the Prefab is being 
    /// given force to move
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="force"></param>
    public void Launch( Vector2 direction, float force )
    {
        thisRiBody.AddForce( direction * force );

    }   // end: public void Launch

    /// <summary>
    /// using the physics engine for collision - here a repair
    /// </summary>
    /// <param name="collision">the gameObject being hit</param>
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

    }   // end: void OnCollisionEnter2D

}   // end: public class Projectile

