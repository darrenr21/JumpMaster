using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float baseScrollSpeed = 2f;
    public float backgroundWidth = 25f;

    void Update()
    {
        float scrollSpeed = baseScrollSpeed * GameManager.gameSpeed;
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;

        if (transform.position.x <= -backgroundWidth)
        {
            Vector3 newPos = transform.position;
            newPos.x += backgroundWidth * 2;
            transform.position = newPos;
        }
    }
}