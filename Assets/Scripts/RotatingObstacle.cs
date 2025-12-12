using UnityEngine;

public class RotatingObstacle : MonoBehaviour
{
    public float baseSpeed = 5f;
    public float rotationSpeed = 100f;
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

        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}