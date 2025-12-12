using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private bool isGameOver = false;
    public float normalXPosition = -8.5f;
    public float reverseXPosition = 5f;

    public bool immortalMode = false;

    public bool hasShield = false;
    public bool hasMagnet = false;
    public bool isShrunk = false;
    public float magnetRange = 5f;

    private Vector3 normalScale;
    private Vector3 shrunkScale;

    private bool isGravityFlipped = false;
    private float normalGravity;
    private bool isTransitioning = false;
    private float targetXPosition;
    private float portalImmunityTimer = 0f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        normalScale = transform.localScale;
        shrunkScale = normalScale * 0.5f;
        normalXPosition = transform.position.x;
        targetXPosition = normalXPosition;
        normalGravity = rb.gravityScale;

        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        if (isGameOver) return;

        // Handle portal immunity timer
        if (portalImmunityTimer > 0)
        {
            portalImmunityTimer -= Time.deltaTime;

            // Flash the player
            float flash = Mathf.Sin(Time.time * 20f);
            if (flash > 0)
            {
                spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f); // Semi-transparent
            }
            else
            {
                spriteRenderer.color = Color.white; // Normal
            }
        }
        else if (isTransitioning)
        {
            // Also flash during transition
            float flash = Mathf.Sin(Time.time * 20f);
            if (flash > 0)
            {
                spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
            }
            else
            {
                spriteRenderer.color = Color.white;
            }
        }
        else
        {
            // Make sure color is normal when not immune
            spriteRenderer.color = Color.white;
        }

        // Handle transitioning between positions
        if (isTransitioning)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            float newX = Mathf.MoveTowards(transform.position.x, targetXPosition, 15f * Time.deltaTime);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);

            if (Mathf.Abs(transform.position.x - targetXPosition) < 0.1f)
            {
                transform.position = new Vector3(targetXPosition, transform.position.y, transform.position.z);
                isTransitioning = false;
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (hasMagnet)
        {
            AttractCoins();
        }
    }

    void Jump()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayJump();
        }

        if (isGravityFlipped)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -jumpForce);
        }
        else
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    public void FlipGravity()
    {
        isGravityFlipped = true;
        rb.gravityScale = -normalGravity;

        Vector3 scale = transform.localScale;
        scale.y = -Mathf.Abs(scale.y);
        transform.localScale = scale;
    }

    public void ResetGravity()
    {
        isGravityFlipped = false;
        rb.gravityScale = normalGravity;

        Vector3 scale = transform.localScale;
        scale.y = Mathf.Abs(scale.y);
        transform.localScale = scale;
    }

    public void StartReverseMode()
    {
        isTransitioning = true;
        targetXPosition = reverseXPosition;
    }

    public void StopReverseMode()
    {
        isTransitioning = true;
        targetXPosition = normalXPosition;
    }

    public void StartPortalImmunity(float duration)
    {
        portalImmunityTimer = duration;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (immortalMode || isTransitioning || portalImmunityTimer > 0)
            {
                Destroy(collision.gameObject);
                return;
            }

            if (hasShield)
            {
                hasShield = false;
                if (AudioManager.instance != null)
                {
                    AudioManager.instance.PlayShieldBreak();
                }
                Destroy(collision.gameObject);
            }
            else
            {
                GameOver();
            }
        }
    }

    void GameOver()
    {
        isGameOver = true;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;
        GameManager.instance.GameOver();
    }

    public void ActivatePowerUp(PowerUp.PowerUpType type, float duration)
    {
        switch (type)
        {
            case PowerUp.PowerUpType.Shield:
                hasShield = true;
                break;
            case PowerUp.PowerUpType.Magnet:
                StartCoroutine(MagnetPowerUp(duration));
                break;
            case PowerUp.PowerUpType.Shrink:
                StartCoroutine(ShrinkPowerUp(duration));
                break;
        }
    }

    System.Collections.IEnumerator MagnetPowerUp(float duration)
    {
        hasMagnet = true;
        yield return new WaitForSeconds(duration);
        hasMagnet = false;
    }

    System.Collections.IEnumerator ShrinkPowerUp(float duration)
    {
        isShrunk = true;
        transform.localScale = shrunkScale;
        yield return new WaitForSeconds(duration);
        isShrunk = false;
        transform.localScale = normalScale;
    }

    void AttractCoins()
    {
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
        foreach (GameObject coin in coins)
        {
            float distance = Vector2.Distance(transform.position, coin.transform.position);
            if (distance < magnetRange)
            {
                coin.transform.position = Vector2.MoveTowards(
                    coin.transform.position,
                    transform.position,
                    10f * Time.deltaTime
                );
            }
        }
    }
}