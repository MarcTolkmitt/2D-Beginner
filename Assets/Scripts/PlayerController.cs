using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// gives reason to the character
/// </summary>
public class PlayerController : MonoBehaviour
{
    // speed
    public float geschw = 1f;
    // healthsystem
    public int maxHealth = 5;
    int currentHealth;
    // inputsystems
    public InputAction moveAction;
    public InputAction arrowAction;
    public InputAction xboxAction;
    //public int frameRate = 60;
    Rigidbody2D rigidbody2d;
    Vector2 move;
    Vector2 arrow;
    Vector2 xbox;
    Vector2 position;
    // variables related to temporary invincibility
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float damageCooldown;
    public float timeAfterHeal = 1.0f;
    bool isHealing;
    float healingCooldown;
    // additions for the animator
    Animator animator;
    Vector2 moveDirection = new Vector2( 0, 0 );
    // binding in of Prefabs and actions
    public GameObject projectilePrefab;
    public InputAction launchAction;
    public InputAction talkAction;
    // audio
    AudioSource audioSource;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        // framerate settings are sort of bad
        // QualitySettings.vSyncCount = 0;
        // Application.targetFrameRate = frameRate;

        // inputsources
        moveAction.Enable();
        arrowAction.Enable();
        xboxAction.Enable();
        // for Rigidbody und FixedUpdate
        rigidbody2d = GetComponent<Rigidbody2D>();
        // healthsystem
        currentHealth = maxHealth - 1;
        // animator
        animator = GetComponent<Animator>();
        // the callback function gets special treatment
        launchAction.Enable();
        launchAction.performed += Launch;
        talkAction.Enable();
        talkAction.performed += FindFriend;
        // the audio
        audioSource = GetComponent<AudioSource>();

    }   // end: void Start

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        // get the direction right from the moveAction, ...
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

    }   // end: void Update

    /// <summary>
    /// this is 'Update()' in the physics engine speed
    /// <para/>won't be called as often as 'Update()'
    /// </summary>
    void FixedUpdate( )
    {
        // a time dependent move of the character
        float deltaZeit = Time.deltaTime;
        Vector2 locMove =
            ( move * 0.1f * geschw * deltaZeit ) +
            ( arrow * 0.1f * geschw * deltaZeit ) +
            ( xbox * 0.1f * geschw * deltaZeit );
        // for the animator concerning 'locMove'
        if ( !Mathf.Approximately( locMove.x, 0.0f )
            || !Mathf.Approximately( locMove.y, 0.0f ) )
        {
            moveDirection.Set( locMove.x, locMove.y );
            moveDirection.Normalize();  // only working for pointing a direction

        }
        animator.SetFloat( "Look X", moveDirection.x );
        animator.SetFloat( "Look Y", moveDirection.y );
        animator.SetFloat( "Speed", locMove.magnitude ); // on the normalized Vector

        // this is for the real move in the scene
        position = ( Vector2 ) rigidbody2d.position + locMove;
            
        // save the move
        rigidbody2d.MovePosition( position );

    }   // end: void FixedUpdate

    /// <summary>
    /// enemies or health events alter the charcters health here
    /// </summary>
    /// <param name="amount">value change to happen</param>
    public void ChangeHealth( int amount  )
    {
        if ( amount < 0 )
        {
            if ( isInvincible )
                return;
            isInvincible = true;
            damageCooldown = timeInvincible;
            animator.SetTrigger( "Hit" );   // player was hit
       
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

    }   // end: void ChangeHealth

    /// <summary>
    /// classic get-function
    /// </summary>
    /// <returns>the current health</returns>
    public int GetCurrentHealth()
    {
        return( currentHealth );

    }   // end: public int GetCurrentHealth

    /// <summary>
    /// shooting repair on command ( space key )
    /// </summary>
    /// <param name="callback"></param>
    void Launch( InputAction.CallbackContext callback )
    {
        // create the gameobject
        GameObject projectileObject = 
            Instantiate( projectilePrefab, 
            rigidbody2d.position + Vector2.up * 0.5f, 
            Quaternion.identity);
        // get its script and use its function
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch( moveDirection, 300 );
        // aktivate the Animator
        animator.SetTrigger( "Launch" );

    }   // end: void Launch

    /// <summary>
    /// talking to NPCs will trigger their own text ( if you face them )
    /// </summary>
    /// <param name="callback">part of a callback ( listener )</param>
    void FindFriend( InputAction.CallbackContext callback )
    {
        //Debug.Log( "key 't' pressed..." );
        RaycastHit2D hit =
            Physics2D.Raycast( rigidbody2d.position + ( Vector2.up * 0.2f ),
                moveDirection,
                1.5f,
                LayerMask.GetMask( "NPC" ) );
        if ( hit.collider != null )
        {
            Debug.Log( "Raycast has hit the object " + hit.collider.gameObject );
            NonPlayerCharacter npc = hit.collider.GetComponent<NonPlayerCharacter>();
            if ( npc != null )
            {
                Debug.Log( "npc's message: " + npc.npcMessage );
                UIHandler.instance.DisplayDialogue( npc.npcMessage );

            }

        }
    
    }   // end: void FindFriend

    /// <summary>
    /// plays once the given AudioClip
    /// </summary>
    /// <param name="clip">sound to be played</param>
    public void PlaySound( AudioClip clip )
    {
        audioSource.PlayOneShot( clip );

    }   // end: public void PlaySound

}   // end: public class PlayerController


