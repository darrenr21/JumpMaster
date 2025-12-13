using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed = 2f;

    [Header("World Backgrounds")]
    public Sprite skyBackground;
    public Sprite sunsetBackground;
    public Sprite nightBackground;
    public Sprite spaceBackground;
    public Sprite chaosBackground;

    private SpriteRenderer spriteRenderer;
    private int currentWorld = -1;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateBackground();
    }

    void Update()
    {
        // Check for world change
        UpdateBackground();
    }

    void UpdateBackground()
    {
        if (GameManager.instance == null) return;

        int newWorld = GameManager.instance.GetCurrentWorld();

        if (newWorld != currentWorld)
        {
            currentWorld = newWorld;

            switch (currentWorld)
            {
                case 0:
                    if (skyBackground != null) spriteRenderer.sprite = skyBackground;
                    break;
                case 1:
                    if (sunsetBackground != null) spriteRenderer.sprite = sunsetBackground;
                    break;
                case 2:
                    if (nightBackground != null) spriteRenderer.sprite = nightBackground;
                    break;
                case 3:
                    if (spaceBackground != null) spriteRenderer.sprite = spaceBackground;
                    break;
                default:
                    if (chaosBackground != null) spriteRenderer.sprite = chaosBackground;
                    break;
            }
        }
    }
}