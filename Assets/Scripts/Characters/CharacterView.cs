using UnityEngine;

/// <summary>
/// View of a character instantiated in the Unity scene.
/// </summary>
/// <remarks>In our architecture, the view also contains the model.
/// This allows regrouping variables.</remarks>
[RequireComponent(typeof(EventForwarder))]
[RequireComponent(typeof(SpriteRenderer))]
public class CharacterView : MonoBehaviour {

    public CharacterMovement MyCharacterMovement;

    public EventForwarder MyEventForwarder { get; private set; }
    public SpriteRenderer MySpriteRenderer { get; private set; }

    [HideInInspector]
    public int Health;
    [HideInInspector]
    public bool IsAlive;
    [HideInInspector]
    public int Damage;
    [HideInInspector]
    public Sprite DefaultSprite;
    [HideInInspector]
    public Sprite CharacterWithFlagSprite;
    [HideInInspector]
    public bool HasFlag;

    private void Awake()
    {
        MyEventForwarder = GetComponent<EventForwarder>();
        MySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeFlag()
    {
        MyEventForwarder.OnTakeFlag();
    }

}
