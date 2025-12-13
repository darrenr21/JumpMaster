using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float parallaxSpeed = 1f;
    public bool scrolls = true;

    private SpriteRenderer spriteRenderer;
    private GameObject cloneRight;
    private GameObject cloneLeft;
    private float spriteWidth;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = spriteRenderer.bounds.size.x;

        if (scrolls && spriteRenderer != null)
        {
            // Create clone on the RIGHT
            cloneRight = new GameObject(gameObject.name + "_CloneRight");
            cloneRight.transform.SetParent(transform.parent);
            cloneRight.transform.localScale = transform.localScale;
            cloneRight.transform.position = new Vector3(transform.position.x + spriteWidth, transform.position.y, transform.position.z);

            SpriteRenderer cloneRightSR = cloneRight.AddComponent<SpriteRenderer>();
            cloneRightSR.sprite = spriteRenderer.sprite;
            cloneRightSR.sortingOrder = spriteRenderer.sortingOrder;
            cloneRightSR.sortingLayerName = spriteRenderer.sortingLayerName;

            // Create clone on the LEFT
            cloneLeft = new GameObject(gameObject.name + "_CloneLeft");
            cloneLeft.transform.SetParent(transform.parent);
            cloneLeft.transform.localScale = transform.localScale;
            cloneLeft.transform.position = new Vector3(transform.position.x - spriteWidth, transform.position.y, transform.position.z);

            SpriteRenderer cloneLeftSR = cloneLeft.AddComponent<SpriteRenderer>();
            cloneLeftSR.sprite = spriteRenderer.sprite;
            cloneLeftSR.sortingOrder = spriteRenderer.sortingOrder;
            cloneLeftSR.sortingLayerName = spriteRenderer.sortingLayerName;
        }
    }

    void Update()
    {
        if (!scrolls) return;
        if (GameManager.instance == null) return;

        float speed = parallaxSpeed * GameManager.gameSpeed * Time.deltaTime;
        Vector3 movement;

        if (GameManager.instance.isReverseMode)
        {
            movement = Vector3.right * speed;
        }
        else
        {
            movement = Vector3.left * speed;
        }

        // Move all three sprites together
        transform.position += movement;
        if (cloneRight != null) cloneRight.transform.position += movement;
        if (cloneLeft != null) cloneLeft.transform.position += movement;

        // Check if main sprite needs to wrap
        float cameraX = Camera.main.transform.position.x;

        // If main is too far left, move it to the right
        if (transform.position.x < cameraX - spriteWidth * 1.5f)
        {
            transform.position = new Vector3(transform.position.x + spriteWidth * 3f, transform.position.y, transform.position.z);
        }
        // If main is too far right, move it to the left
        if (transform.position.x > cameraX + spriteWidth * 1.5f)
        {
            transform.position = new Vector3(transform.position.x - spriteWidth * 3f, transform.position.y, transform.position.z);
        }

        // Same for right clone
        if (cloneRight != null)
        {
            if (cloneRight.transform.position.x < cameraX - spriteWidth * 1.5f)
            {
                cloneRight.transform.position = new Vector3(cloneRight.transform.position.x + spriteWidth * 3f, cloneRight.transform.position.y, cloneRight.transform.position.z);
            }
            if (cloneRight.transform.position.x > cameraX + spriteWidth * 1.5f)
            {
                cloneRight.transform.position = new Vector3(cloneRight.transform.position.x - spriteWidth * 3f, cloneRight.transform.position.y, cloneRight.transform.position.z);
            }
        }

        // Same for left clone
        if (cloneLeft != null)
        {
            if (cloneLeft.transform.position.x < cameraX - spriteWidth * 1.5f)
            {
                cloneLeft.transform.position = new Vector3(cloneLeft.transform.position.x + spriteWidth * 3f, cloneLeft.transform.position.y, cloneLeft.transform.position.z);
            }
            if (cloneLeft.transform.position.x > cameraX + spriteWidth * 1.5f)
            {
                cloneLeft.transform.position = new Vector3(cloneLeft.transform.position.x - spriteWidth * 3f, cloneLeft.transform.position.y, cloneLeft.transform.position.z);
            }
        }
    }

    void OnDestroy()
    {
        if (cloneRight != null) Destroy(cloneRight);
        if (cloneLeft != null) Destroy(cloneLeft);
    }
}