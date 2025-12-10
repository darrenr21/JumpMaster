using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f;
    private Rigidbody2D rb;

    // Power-up states
    public bool hasShield = false;
    public bool hasMagnet = false;
    public bool isShrunk = false;
    public float magnetRange = 5f;

    private Vector3 normalScale;
    private Vector3 shrunkScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        normalScale = transform.localScale;
        shrunkScale = normalScale * 0.5f;
    }

    void Update()
    {
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
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (hasShield)
            {
                hasShield = false;
                Destroy(collision.gameObject);
            }
            else
            {
                GameManager.instance.GameOver();
            }
        }
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