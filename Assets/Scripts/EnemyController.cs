using UnityEngine;

/// <summary>
/// controls the enemies
/// </summary>
public class EnemyController : MonoBehaviour
{
    // audio
    public AudioClip enemyHitAudioClip;
    
    public float speed = 1.0f;
    public float minimalDistance = 0.1f;
    Rigidbody2D rigidbody2d;
    Vector2 position;
    public int movDist = 5;    // total distance in unity meters
    public Vector2 movDelta;    // free 2d-MoveVector
    int moveTarget = 1;
    Vector2 positionTarget;
    Vector2 positionSource;
    Animator animator;
    bool aggressive = true;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start( )
    {
        // für Rigidbody und FixedUpdate
        rigidbody2d = GetComponent<Rigidbody2D>( );
        positionSource = rigidbody2d.position;
        positionTarget = positionSource + ( movDelta * movDist );
        animator = GetComponent<Animator>();
        enemyHitAudioClip = GetComponent<AudioClip>( );

    }   // end: void Start

    // Update is called once per frame
    void Update( )
    {

    }   // Ende: void Update

    /// <summary>
    /// the physic engine's 'Update()' working in its own
    /// framerate
    /// </summary>
    void FixedUpdate( )
    {
        if ( !aggressive )
            return;

        // movement from source to target and back
        if ( moveTarget == 1 )
        {   // move as long as distance is too big
            if ( Vector2.Distance( position, positionTarget ) < minimalDistance )
                moveTarget = -1;

        }
        else if ( moveTarget == -1 )
        {   // movement back towards the source
            if ( Vector2.Distance( position, positionSource ) < minimalDistance )
                moveTarget = 1;
        
        }
        
        // the movement relativ to the time passed
        Vector2 locMove = 
            movDelta * moveTarget * speed * Time.deltaTime;
        position = rigidbody2d.position + locMove;
        // inform the Animator
        animator.SetFloat( "Move X", locMove.x );
        animator.SetFloat( "Move Y", locMove.y );
        // save the movement
        rigidbody2d.MovePosition( position );

    }   // end: void FixedUpdate

    /// <summary>
    /// using the physic engine to hit the player
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter2D( Collision2D collision )
    {
        //Debug.Log( "enemy -> got a collision: " + collision );
        PlayerController player =
            collision.gameObject.GetComponent<PlayerController>();
        if ( player != null )
        {
            player.ChangeHealth( -1 );
            player.PlaySound( enemyHitAudioClip );

        }

    }   // end: void OnCollisionEnter2D

    /// <summary>
    /// actionfunction for the character's weapon
    /// </summary>
    public void Fix( )
    {
        aggressive = false;
        rigidbody2d.simulated = false;
        animator.SetTrigger( "Fixed" );

    }   // end: public void Fix

}   // end: public class EnemyController
