using UnityEngine;

public class AIControl : MonoBehaviour
{
    [SerializeField] private GameObject ball = default;
    [SerializeField] private GameConfig config = default;

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        var diff = ball.transform.position.y - transform.position.y;
        _rb.velocity = new Vector2(0, config.MaxAIMovementSpeed * diff);
    }
}