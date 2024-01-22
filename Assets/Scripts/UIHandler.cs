using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// for the UI there is this script. 
/// <para/>Beware only here is
/// a 'static' variable wanted as to give access to the UI
/// to everyone !
/// </summary>
public class UIHandler : MonoBehaviour
{
    /// <summary>
    /// 'static' is only to be used here for the UI !
    /// <para/>Can be called from everywhere in the scope of the project.
    /// </summary>
    public static UIHandler instance { get; private set; }
    
    VisualElement m_HealthBar;
    public float displayTime = 4.0f;
    VisualElement m_NPCdialogue;
    Label m_Label;
    float m_TimerDisplay;

    /// <summary>
    /// is called first on the creation of
    /// an UI object
    /// </summary>
    void Awake( )
    {
        instance = this;

    }   // end: void Awake

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        // get a special child out of the UXML by its name
        // remember to assign a name to them in the UXML!
        UIDocument document = GetComponent<UIDocument>();
        m_HealthBar =
            document.rootVisualElement.Q<VisualElement>( "HealthBar" );
        //SetHealthValue( 0.5f );
        m_NPCdialogue = 
            document.rootVisualElement.Q<VisualElement>( "NPC dialogue" );
        m_NPCdialogue.style.display = DisplayStyle.None;
        m_TimerDisplay = -1.0f;
        m_Label = 
            document.rootVisualElement.Q<Label>( "Label" );
        //if ( m_Label == null )
        //    Debug.Log( "label is null" );

    }   // end: void Start

    /// <summary>
    /// handles the timer for the shown dialogue
    /// </summary>
    void Update( )
    {
        if ( m_TimerDisplay > 0 )
        {
            m_TimerDisplay -= Time.deltaTime;
            if ( m_TimerDisplay < 0 )
                m_NPCdialogue.style.display = DisplayStyle.None;

        }

    }   // End: void Update

    /// <summary>
    /// is used to alter the characters health
    /// </summary>
    /// <param name="percentage">can add or subtract health from the char</param>
    public void SetHealthValue( float percentage )
    {
        float localPercent =
            Mathf.Clamp( percentage, 0.0f, 1.0f );
        m_HealthBar.style.width = Length.Percent( localPercent * 100.0f );

    }   // end: public void SetHealthValue

    /// <summary>
    /// is showing specialized dialoguetext
    /// </summary>
    /// <param name="message">the NPC's own text</param>
    public void DisplayDialogue( string message )
    {
        m_Label.text = message;
        m_TimerDisplay = displayTime;
        m_NPCdialogue.style.display = DisplayStyle.Flex;

    }   // end: public void DisplayDialogue

}   // end: public class UIHandler

