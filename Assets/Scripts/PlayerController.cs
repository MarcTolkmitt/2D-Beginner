using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Geschwindigkeit
    public float geschw = 1f;
    // Healthsystem
    public int maxHealth = 5;
    int currentHealth;

    // Eingabesysteme
    public InputAction moveAction;
    public InputAction arrowAction;
    public InputAction xboxAction;
    //public int frameRate = 60;
    Rigidbody2D rigidbody2d;
    Vector2 move;
    Vector2 arrow;
    Vector2 xbox;
    Vector2 position;
    // Variables related to temporary invincibility
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float damageCooldown;
    public float timeAfterHeal = 1.0f;
    bool isHealing;
    float healingCooldown;

    // Start is called before the first frame update
    void Start()
    {
        // Framerateeinstellungen sind eher schlecht
        // QualitySettings.vSyncCount = 0;
        // Application.targetFrameRate = frameRate;

        // Eingabequellen
        moveAction.Enable();
        arrowAction.Enable();
        xboxAction.Enable();
        // für Rigidbody und FixedUpdate
        rigidbody2d = GetComponent<Rigidbody2D>();
        // Healthsystem
        currentHealth = maxHealth - 1;

    }   // Ende: void Start

    // Update is called once per frame
    void Update()
    {
        // die Richtung direkt aus der moveAction bekommen
        move = moveAction.ReadValue<Vector2>();
        arrow = arrowAction.ReadValue<Vector2>();
        xbox = xboxAction.ReadValue<Vector2>();
        //Debug.Log ( $"Move-( X, Y ): ( {move.x}, {move.y} )" );
        //Debug.Log( $"Arrow-( X, Y ): ( {arrow.x}, {arrow.y} )" );
        //Debug.Log( $"XBox-( X, Y ): ( {xbox.x}, {xbox.y} )" );

        if ( isInvincible )
        {
            damageCooldown -= Time.deltaTime;
            if ( damageCooldown < 0 )
                isInvincible = false;

        }
        if ( isHealing )
        {
            healingCooldown -= Time.deltaTime;
            if ( healingCooldown < 0 )
                isHealing = false;

        }

    }   // Ende: void Update

    void FixedUpdate( )
    {
        // die Bewegung ausführen mit Strecke pro Zeiteinheit
        float deltaZeit = Time.deltaTime;
        position = ( Vector2 ) rigidbody2d.position +
            ( move * 0.1f * geschw * deltaZeit ) +
            ( arrow * 0.1f * geschw * deltaZeit ) +
            ( xbox * 0.1f * geschw * deltaZeit );
        // die Bewegung speichern
        rigidbody2d.MovePosition( position );

    }   // Ende: void FixedUpdate

    public void ChangeHealth( int amount  )
    {
        if ( amount < 0 )
        {
            if ( isInvincible )
                return;
            isInvincible = true;
            damageCooldown = timeInvincible;
       
        }
        else if ( amount > 0 )
        {
            if ( isHealing )
                return;
            isHealing = true;
            healingCooldown = timeAfterHeal;

        }
        currentHealth = (int)
            Mathf.Clamp( currentHealth + amount, 0, maxHealth );
        //Debug.Log( "Healthsystem: " + currentHealth + "/" + maxHealth );
        float localHealth = currentHealth / (float)maxHealth;
        UIHandler.instance.SetHealthValue( localHealth );

    }   // Ende: void ChangeHealth

    public int GetCurrentHealth()
    {
        return( currentHealth );

    }   // Ende: public int GetCurrentHealth

}   // Ende: public class PlayerController

