using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyController : MonoBehaviour
{
    public float speed = 1.0f;
    public float minimaleDistanz = 0.1f;
    Rigidbody2D rigidbody2d;
    Vector2 position;
    public int bewWeite = 5;
    public Vector2 bewDelta;
    int bewZiel = 1;
    Vector2 positionZiel;
    Vector2 positionQuelle;

    // Start is called before the first frame update
    void Start( )
    {
        // f�r Rigidbody und FixedUpdate
        rigidbody2d = GetComponent<Rigidbody2D>( );
        positionQuelle = rigidbody2d.position;
        positionZiel = positionQuelle + ( bewDelta * bewWeite );

    }   // Ende: void Start

    // Update is called once per frame
    void Update( )
    {

    }   // Ende: void Update

    void FixedUpdate( )
    {
        // Bewegung von Quelle zu Ziel und zur�ck
        if ( bewZiel == 1 )
        {   // so lange bis das Ziel nahe genug ist
            if ( Vector2.Distance( position, positionZiel ) < minimaleDistanz )
                bewZiel = -1;

        }
        else if ( bewZiel == -1 )
        {   // sol lange bis die Quelle nahe genug ist
            if ( Vector2.Distance( position, positionQuelle ) < minimaleDistanz )
                bewZiel = 1;
        
        }
        
        // die Bewegung ausf�hren mit Strecke pro Zeiteinheit
        float deltaZeit = Time.deltaTime;
        position = rigidbody2d.position + 
            ( bewDelta * bewZiel * speed * deltaZeit );
        // die Bewegung speichern
        rigidbody2d.MovePosition( position );

    }   // Ende: void FixedUpdate

    void OnCollisionEnter2D( Collision2D collision )
    {
        Debug.Log( "Enemy -> Kollision erkannt: " + collision );
        PlayerController player =
            collision.gameObject.GetComponent<PlayerController>();
        if ( player != null )
            player.ChangeHealth( -1 );

    }   // Ende: void OnCollisionEnter2D

}   // Ende: public class EnemyController
