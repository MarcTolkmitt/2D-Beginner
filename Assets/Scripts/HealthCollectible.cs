using UnityEngine;

/// <summary>
/// general script for the collectibles - they are numbered 
/// as the first character of the name
/// </summary>
public class HealthCollectible : MonoBehaviour
{
    /// <summary>
    /// the seen sprite works as trigger and this is handled
    /// in this script via the physics engine
    /// </summary>
    /// <param name="collision">the event giving gameObject</param>
    void OnTriggerEnter2D( Collider2D collision )
    {
        //Debug.Log( "object that entered the trigger: " + collision );
        PlayerController controller = 
            collision.GetComponent<PlayerController>();

        //Debug.Log( $"name of the object: {gameObject.name} - > {gameObject.name[ 0 ]}" );
        // searched script only exists at the player
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

    }   // end: void OnTriggerEnter2D

}   // end: public class HealthCollectible

