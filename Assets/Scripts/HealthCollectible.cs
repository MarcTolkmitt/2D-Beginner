using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    void OnTriggerEnter2D( Collider2D collision )
    {
        //Debug.Log( "Object that entered the trigger: " + collision );
        PlayerController controller = 
            collision.GetComponent<PlayerController>();

        Debug.Log( $"Name des Objekts: {gameObject.name} - > {gameObject.name[ 0 ]}" );
        // gesuchtes Skript existiert nur beim Player
        if ( controller != null )
        {
            if ( controller.GetCurrentHealth() < controller.maxHealth )
            {
                if ( gameObject.name[ 0 ] == '1' )
                    controller.ChangeHealth( 1 );
                else if ( gameObject.name[ 0 ] == '2' )
                    controller.ChangeHealth( 2 );
                Destroy( gameObject );

            }

        }

    }   // Ende: void OnTriggerEnter2D

}   // Ende: public class HealthCollectible

