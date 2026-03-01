using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal"); // A/D, Left/Right
        float v = Input.GetAxis("Vertical");   // W/S, Up/Down

        Vector3 force = new Vector3(h, 0f, v) * moveSpeed;
        rb.AddForce(force, ForceMode.Force);
    }
}