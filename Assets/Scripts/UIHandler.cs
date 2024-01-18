using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIHandler : MonoBehaviour
{
    // füre ienen sehr speziellen Zweck im Tutorial
    public static UIHandler instance { get; private set; }
    VisualElement m_HealthBar;

    // wird bei der Erstellung des Oberflächenelements zuerst aufgerufen
    void Awake( )
    {
        instance = this;

    }   // Ende: void Awake

    // Start is called before the first frame update
    void Start()
    {
        // aus dem UXML der Szene wird das spezielle Kind geholt
        UIDocument document = GetComponent<UIDocument>();
        m_HealthBar =
            document.rootVisualElement.Q<VisualElement>( "HealthBar" );
        //SetHealthValue( 0.5f );

    }   // Ende: void Start

    public void SetHealthValue( float percentage )
    {
        float localPercent =
            Mathf.Clamp( percentage, 0.0f, 1.0f );
        m_HealthBar.style.width = Length.Percent( localPercent * 100.0f );

    }   // Ende: public void SetHealthValue

}   // Ende: public class UIHandler

