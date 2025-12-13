using UnityEngine;

public class FitToParent : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (spriteRenderer != null && spriteRenderer.drawMode == SpriteDrawMode.Sliced)
        {
            Vector3 parentScale = transform.parent.localScale;
            spriteRenderer.size = new Vector2(parentScale.x, parentScale.y);
        }
    }
}