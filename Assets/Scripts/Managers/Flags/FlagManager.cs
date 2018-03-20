using UnityEngine;

/// <summary>
/// Manages flag instantiations and flag sprites.
/// </summary>
public class FlagManager : Singleton<FlagManager> {

    [Tooltip("Transform of the object which will contain flags as children objects.")]
    [SerializeField]
    private Transform FlagContainer;

    private GameObject KnightFlagPrefab = null;
    private GameObject SamuraiFlagPrefab = null;

    private Sprite SamuraiFlagSprite = null;
    private Sprite KnightFlagSprite = null;
    private Sprite TakenFlagSprite = null;

    private SpriteRenderer KnightSpriteRenderer;
    private SpriteRenderer SamuraiSpriteRenderer;


    private void Awake()
    {
        KnightFlagPrefab = (GameObject)Resources.Load("Prefabs/KnightFlag");
        SamuraiFlagPrefab = (GameObject)Resources.Load("Prefabs/SamuraiFlag");

        if (KnightFlagPrefab == null || SamuraiFlagPrefab == null)
        {
            Debug.LogError("Couldn't load either or both of the flag prefabs from Resources folder.");
        }
    }

    private void Start()
    {
        KnightFlagSprite = SpriteLoader.Instance.KnightFlagSprite;
        SamuraiFlagSprite = SpriteLoader.Instance.SamuraiFlagSprite;
        TakenFlagSprite = SpriteLoader.Instance.TakenFlagSprite;
    }

    public void DestroyFlag(GameObject flag)
    {
        Destroy(flag);
    }

    /// <summary>
    /// Instantiates flag of given type.
    /// </summary>
    /// <param name="flagType">Type of flag to instantiate.</param>
    /// <returns>Instantiated object.</returns>
    /// <remarks>Also stores flag's spriteRendrer for later uses.</remarks>
    public GameObject SpawnFlag(GameParameters.Team flagType)
    {
        GameObject newFlagObject = null;
        switch(flagType)
        {
            case GameParameters.Team.Knight:
                newFlagObject = Instantiate(KnightFlagPrefab, FlagContainer);
                KnightSpriteRenderer = newFlagObject.GetComponent<SpriteRenderer>();
                break;
            case GameParameters.Team.Samurai:
                newFlagObject = Instantiate(SamuraiFlagPrefab, FlagContainer);
                SamuraiSpriteRenderer = newFlagObject.GetComponent<SpriteRenderer>();
                break;
        }
        if(newFlagObject.GetComponent<SpriteRenderer>() == null)
        {
            Debug.LogWarning("No spriterenderer found on one of the instantiated flag."
                + " Won't be able to perform flag sprite management.");
        }
        return newFlagObject;
    }

    #region Sprite management

    /// <summary>
    /// Sets taken flag sprite on flag of given type.
    /// </summary>
    /// <param name="flagType">Type of flag whose sprite should be changed.</param>
    public void SetTakenFlagSprite(GameParameters.Team flagType)
    {
        switch(flagType)
        {
            case GameParameters.Team.Knight:
                if(KnightSpriteRenderer != null)
                {
                    KnightSpriteRenderer.sprite = TakenFlagSprite;
                }
                break;
            case GameParameters.Team.Samurai:
                if (SamuraiSpriteRenderer != null)
                {
                    SamuraiSpriteRenderer.sprite = TakenFlagSprite;
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Sets default flag sprite on flag of given type.
    /// </summary>
    /// <param name="flagType">Type of flag whose sprite should be changed.</param>
    public void SetDefaultFlagSprite(GameParameters.Team flagType)
    {
        switch(flagType)
        {
            case GameParameters.Team.Knight:
                if (KnightSpriteRenderer != null)
                {
                    KnightSpriteRenderer.sprite = KnightFlagSprite;
                }
                break;
            case GameParameters.Team.Samurai:
                if (SamuraiSpriteRenderer != null)
                {
                    SamuraiSpriteRenderer.sprite = SamuraiFlagSprite;
                }
                break;
            default:
                break;
        }
    }
    #endregion
}
