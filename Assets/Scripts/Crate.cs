using UnityEngine;

public class Crate : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bird"))
        {
            Invoke(nameof(DestroyCrate), 2);
        }
    }

    void DestroyCrate()
    {
        Destroy(gameObject);
    }
}