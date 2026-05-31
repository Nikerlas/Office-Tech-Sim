using UnityEngine;

public class CarMover : MonoBehaviour
{
    public float speed = 8f;

    void Update()
    {
        transform.Translate(
            Vector3.forward *
            speed *
            Time.deltaTime
        );
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}