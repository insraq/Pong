using System;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField] private GameConfig config = default;
    
    private float _height;

    private void Awake()
    {
        _height = GetComponent<BoxCollider2D>().size.y * transform.localScale.y;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            var v = other.rigidbody.velocity;
            
            // Apparently Pong's physics is a bit different. The "reflection" depends on which part of the paddle
            // is being hit. This normalize the relatively position of the ball to the paddle to [-0.5, 0.5] 
            var percent = (other.transform.position.y - transform.position.y) / _height;
            var vy = Math.Abs(v.x) * percent * config.CollisionMultiplier;
            other.rigidbody.velocity = new Vector2(-v.x, vy);
        }
    }
}