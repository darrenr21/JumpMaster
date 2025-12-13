using UnityEngine;

public class NeonGlow : MonoBehaviour
{
    [Header("Neon Colors")]
    public Color coreColor = Color.white;
    public Color glowColor = Color.cyan;
    public Color outerGlowColor = Color.blue;

    [Header("Pulse Settings")]
    public float pulseSpeed = 3f;
    public float minGlow = 0.7f;
    public float maxGlow = 1.3f;

    [Header("Outline Settings")]
    public bool showOutline = true;
    public float outlineWidth = 0.05f;

    [Header("Edge Spikes Settings")]
    public bool showSpikes = true;
    public int spikesPerSide = 5;
    public float spikeSize = 0.15f;

    private SpriteRenderer spriteRenderer;
    private GameObject[] spikes;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (showOutline)
        {
            CreateOutline();
        }

        if (showSpikes)
        {
            CreateEdgeSpikes();
        }
    }

    void CreateOutline()
    {
        Vector3[] offsets = {
            new Vector3(0, outlineWidth, 0),
            new Vector3(0, -outlineWidth, 0),
            new Vector3(-outlineWidth, 0, 0),
            new Vector3(outlineWidth, 0, 0)
        };

        for (int i = 0; i < 4; i++)
        {
            GameObject outline = new GameObject("Outline" + i);
            outline.transform.SetParent(transform);
            outline.transform.localPosition = offsets[i];
            outline.transform.localScale = Vector3.one;
            outline.transform.localRotation = Quaternion.identity;

            SpriteRenderer outlineSR = outline.AddComponent<SpriteRenderer>();
            outlineSR.sprite = spriteRenderer.sprite;
            outlineSR.color = outerGlowColor;
            outlineSR.sortingOrder = spriteRenderer.sortingOrder - 1;
        }
    }

    void CreateEdgeSpikes()
    {
        Sprite triangleSprite = CreateTriangleSprite();
        Vector3 size = Vector3.one;

        int totalSpikes = spikesPerSide * 4;
        spikes = new GameObject[totalSpikes];
        int spikeIndex = 0;

        // Top edge - pointing up
        for (int i = 0; i < spikesPerSide; i++)
        {
            float xPos = Mathf.Lerp(-0.5f, 0.5f, (i + 0.5f) / spikesPerSide);
            CreateSpike(triangleSprite, new Vector3(xPos, 0.5f, 0), 0, spikeIndex++);
        }

        // Bottom edge - pointing down
        for (int i = 0; i < spikesPerSide; i++)
        {
            float xPos = Mathf.Lerp(-0.5f, 0.5f, (i + 0.5f) / spikesPerSide);
            CreateSpike(triangleSprite, new Vector3(xPos, -0.5f, 0), 180, spikeIndex++);
        }

        // Left edge - pointing left
        for (int i = 0; i < spikesPerSide; i++)
        {
            float yPos = Mathf.Lerp(-0.5f, 0.5f, (i + 0.5f) / spikesPerSide);
            CreateSpike(triangleSprite, new Vector3(-0.5f, yPos, 0), 90, spikeIndex++);
        }

        // Right edge - pointing right
        for (int i = 0; i < spikesPerSide; i++)
        {
            float yPos = Mathf.Lerp(-0.5f, 0.5f, (i + 0.5f) / spikesPerSide);
            CreateSpike(triangleSprite, new Vector3(0.5f, yPos, 0), -90, spikeIndex++);
        }
    }

    void CreateSpike(Sprite sprite, Vector3 localPos, float rotation, int index)
    {
        GameObject spike = new GameObject("Spike" + index);
        spike.transform.SetParent(transform);
        spike.transform.localPosition = localPos;
        spike.transform.localRotation = Quaternion.Euler(0, 0, rotation);
        spike.transform.localScale = new Vector3(spikeSize, spikeSize, 1);

        SpriteRenderer spikeSR = spike.AddComponent<SpriteRenderer>();
        spikeSR.sprite = sprite;
        spikeSR.color = glowColor;
        spikeSR.sortingOrder = spriteRenderer.sortingOrder + 1;

        spikes[index] = spike;
    }

    Sprite CreateTriangleSprite()
    {
        int size = 32;
        Texture2D texture = new Texture2D(size, size);

        Color transparent = new Color(0, 0, 0, 0);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float normalizedY = (float)y / size;
                float halfWidth = (size / 2f) * (1f - normalizedY);
                float centerX = size / 2f;

                if (x >= centerX - halfWidth && x <= centerX + halfWidth)
                {
                    texture.SetPixel(x, y, Color.white);
                }
                else
                {
                    texture.SetPixel(x, y, transparent);
                }
            }
        }

        texture.Apply();
        texture.filterMode = FilterMode.Point;

        return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0));
    }

    void Update()
    {
        float pulse = Mathf.Lerp(minGlow, maxGlow, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);

        Color pulsedCore = coreColor * pulse;
        pulsedCore.a = 1f;
        spriteRenderer.color = pulsedCore;

        if (showSpikes && spikes != null)
        {
            foreach (GameObject spike in spikes)
            {
                if (spike != null)
                {
                    SpriteRenderer spikeSR = spike.GetComponent<SpriteRenderer>();
                    Color spikeColor = glowColor * pulse;
                    spikeColor.a = 1f;
                    spikeSR.color = spikeColor;
                }
            }
        }
    }

    void OnDestroy()
    {
        if (spikes != null)
        {
            foreach (GameObject spike in spikes)
            {
                if (spike != null)
                {
                    Destroy(spike);
                }
            }
        }
    }
}