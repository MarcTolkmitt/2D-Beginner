using UnityEngine;

/// <summary>
/// general script for the healing zones - they are numbered 
/// as the first character of the name
/// </summary>
public class HealingZone : MonoBehaviour
{
    // audio
    public AudioClip damageZoneAudioClip;
    public float playPause = 1.0f;
    float playTime = 0;

    // Update is called once per frame
    void Update( )
    {
        // audio sound countdown
        if ( playTime > 0 )
            playTime -= Time.deltaTime;


    }   // Ende: void Update

    /// <summary>
    /// the seen sprite works as trigger and this is handled
    /// in this script via the physics engine
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerStay2D( Collider2D collision )
    {
        //Debug.Log( "object that entered the trigger: " + collision );
        PlayerController controller =
            collision.GetComponent<PlayerController>();

        //Debug.Log( $"name of the object: {gameObject.name} - > {gameObject.name[ 0 ]}" );
        // searched for script only exists at the player
        if ( controller != null )
        {
            if ( controller.GetCurrentHealth( ) < controller.maxHealth )
            {
                if ( gameObject.name[ 0 ] == '1' )
                {
                    controller.ChangeHealth( 1 );
                    if ( playTime <= 0 )
                    {
                        playTime = playPause;
                        controller.PlaySound( damageZoneAudioClip );

                    }

                }
                else if ( gameObject.name[ 0 ] == '2' )
                {
                    controller.ChangeHealth( 2 );
                    if ( playTime <= 0 )
                    {
                        playTime = playPause;
                        controller.PlaySound( damageZoneAudioClip );

                    }

                }

            }

        }

    }   // end: void OnTriggerEnter2D

}   // end: public class HealingZone

