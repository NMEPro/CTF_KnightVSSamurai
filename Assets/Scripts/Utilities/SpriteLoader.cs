using UnityEngine;

/// <summary>
/// Loads sprites for later dynamic uses.
/// </summary>
public class SpriteLoader : Singleton<SpriteLoader> {

    [HideInInspector]
    public Sprite DefaultKnightSprite { get; private set; }
    [HideInInspector]
    public Sprite DefaultSamuraiSprite { get; private set; }
    [HideInInspector]
    public Sprite KnightSpriteWithFlag { get; private set; }
    [HideInInspector]
    public Sprite SamuraiSpriteWithFlag { get; private set; }
    [HideInInspector]
    public Sprite KnightFlagSprite { get; private set; }
    [HideInInspector]
    public Sprite SamuraiFlagSprite { get; private set; }
    [HideInInspector]
    public Sprite TakenFlagSprite { get; private set; }

    private void Awake()
    {
        DefaultKnightSprite = Resources.Load<Sprite>("Sprites/Knight/Left_Knight");
        DefaultSamuraiSprite = Resources.Load<Sprite>("Sprites/Samurai/Right_Samurai");
        KnightSpriteWithFlag = Resources.Load<Sprite>("Sprites/Knight/Left_Knight_With_Flag");
        SamuraiSpriteWithFlag = Resources.Load<Sprite>("Sprites/Samurai/Right_Samurai_With_Flag");
        KnightFlagSprite = Resources.Load<Sprite>("Sprites/Flags/KnightFlag");
        SamuraiFlagSprite = Resources.Load<Sprite>("Sprites/Flags/SamuraiFlag");
        TakenFlagSprite = Resources.Load<Sprite>("Sprites/Flags/TakenFlag");
    }
}
