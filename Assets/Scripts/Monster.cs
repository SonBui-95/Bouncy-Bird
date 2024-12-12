using UnityEngine;

public class Monster : MonoBehaviour
{
    float destroyImpactMagnitude = 6;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Bird>())
        {
            Destroy(gameObject);
        }

        if(collision.relativeVelocity.magnitude > destroyImpactMagnitude)
        {
            Destroy(gameObject);
        }
    }
}
