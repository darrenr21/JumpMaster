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
                spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
            }
            else
            {
                spriteRenderer.color = Color.white;
            }
        }
        else if (isTransitioning)
        {
            // Flash during transition
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
            spriteRenderer.color = Color.white;
        }

        // Handle transitioning between X positions only
        if (isTransitioning)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            float currentX = transform.position.x;
            float newX = Mathf.MoveTowards(currentX, targetXPosition, 100f * Time.deltaTime);

            // Only change X, keep current Y
            rb.MovePosition(new Vector2(newX, transform.position.y));

            if (Mathf.Abs(newX - targetXPosition) < 0.1f)
            {
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
        // Play jump sound
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

        // Flip sprite upside down
        Vector3 scale = transform.localScale;
        scale.y = -Mathf.Abs(scale.y);
        transform.localScale = scale;
    }

    public void ResetGravity()
    {
        isGravityFlipped = false;
        rb.gravityScale = normalGravity;

        // Flip sprite right side up
        Vector3 scale = transform.localScale;
        scale.y = Mathf.Abs(scale.y);
        transform.localScale = scale;
    }

    public void StartReverseMode()
    {
        isTransitioning = true;
        targetXPosition = reverseXPosition;

        // Flip sprite to face left (moving backwards)
        Vector3 scale = transform.localScale;
        scale.x = -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    public void StopReverseMode()
    {
        isTransitioning = true;
        targetXPosition = normalXPosition;

        // Flip sprite to face right (normal direction)
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        transform.localScale = scale;
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

                // Update power-up text when shield breaks
                if (hasMagnet)
                    GameManager.instance.UpdatePowerUpText("Magnet");
                else if (isShrunk)
                    GameManager.instance.UpdatePowerUpText("Shrink");
                else
                    GameManager.instance.UpdatePowerUpText("None");

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
                GameManager.instance.UpdatePowerUpText("Shield");
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
        GameManager.instance.UpdatePowerUpText("Magnet");

        yield return new WaitForSeconds(duration);

        hasMagnet = false;

        // Check if any other power-up is active
        if (hasShield)
            GameManager.instance.UpdatePowerUpText("Shield");
        else if (isShrunk)
            GameManager.instance.UpdatePowerUpText("Shrink");
        else
            GameManager.instance.UpdatePowerUpText("None");
    }

    System.Collections.IEnumerator ShrinkPowerUp(float duration)
    {
        isShrunk = true;
        GameManager.instance.UpdatePowerUpText("Shrink");

        // Remember current orientation
        float currentXSign = Mathf.Sign(transform.localScale.x);
        float currentYSign = Mathf.Sign(transform.localScale.y);

        // Apply shrink while keeping orientation
        transform.localScale = new Vector3(
            shrunkScale.x * currentXSign,
            shrunkScale.y * currentYSign,
            shrunkScale.z
        );

        yield return new WaitForSeconds(duration);

        isShrunk = false;

        // Restore normal size while keeping current orientation
        float endXSign = Mathf.Sign(transform.localScale.x);
        float endYSign = Mathf.Sign(transform.localScale.y);

        transform.localScale = new Vector3(
            normalScale.x * endXSign,
            normalScale.y * endYSign,
            normalScale.z
        );

        // Check if any other power-up is active
        if (hasShield)
            GameManager.instance.UpdatePowerUpText("Shield");
        else if (hasMagnet)
            GameManager.instance.UpdatePowerUpText("Magnet");
        else
            GameManager.instance.UpdatePowerUpText("None");
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