using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    public float baseSpeed = 5f;
    public float scaleSpeed = 2f;
    public float minScaleY = 1f;
    public float maxScaleY = 4f;
    public bool isGrowing = true;
    public bool isGroundObstacle = true;
    public float groundY = -4f;
    public float ceilingY = 4f;
    private bool isReverseObstacle = false;

    void Start()
    {
        isReverseObstacle = GameManager.instance.isReverseMode;
    }

    void Update()
    {
        float horizontalSpeed = baseSpeed * GameManager.gameSpeed;

        if (isReverseObstacle)
        {
            transform.position += Vector3.right * horizontalSpeed * Time.deltaTime;

            if (transform.position.x > 15f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            transform.position += Vector3.left * horizontalSpeed * Time.deltaTime;

            if (transform.position.x < -15f)
            {
                Destroy(gameObject);
            }
        }

        // Change scale
        Vector3 currentScale = transform.localScale;

        if (isGrowing)
        {
            currentScale.y += scaleSpeed * Time.deltaTime;
            if (currentScale.y >= maxScaleY)
            {
                currentScale.y = maxScaleY;
                isGrowing = false;
            }
        }
        else
        {
            currentScale.y -= scaleSpeed * Time.deltaTime;
            if (currentScale.y <= minScaleY)
            {
                currentScale.y = minScaleY;
                isGrowing = true;
            }
        }

        transform.localScale = currentScale;

        // Reposition so it stays attached to ground or ceiling
        if (isGroundObstacle)
        {
            transform.position = new Vector3(transform.position.x, groundY + (currentScale.y / 2f), transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, ceilingY - (currentScale.y / 2f), transform.position.z);
        }
    }
}