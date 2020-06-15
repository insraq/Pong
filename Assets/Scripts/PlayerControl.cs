using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private GameConfig config = default;

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        var input = Input.GetAxis("Vertical");
        _rb.velocity = new Vector2(0, config.PlayerMovementSpeed * input);
    }
}