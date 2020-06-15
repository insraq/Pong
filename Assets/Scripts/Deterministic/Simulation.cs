using System;
using UnityEngine;

namespace Deterministic
{
    // If we want to be fully deterministic, we should use Fixed64/32 instead of float. Here for simplicity and
    // illustration, float is used.
    // Also for simplicity, the collision system assumes the walls and paddles are lines and the ball is a point.
    // However, they do have sizes in Unity rendering so it will look a bit weird (there will be some overlap)
    public class Simulation
    {
        // This should be moved to a separate data class for bigger projects

        #region Game Entity State

        public Vector2 BallPosition { get; private set; }

        public float AIPaddleVelocity { get; set; }

        public Vector2 AIPaddlePosition => _aiPaddlePosition;

        public float PlayerPaddleVelocity { get; set; }

        public Vector2 PlayerPaddlePosition => _playerPaddlePosition;

        public readonly GameConfig Config;

        #endregion

        private readonly Vector2 _size;
        private readonly float _paddleLength;
        private Vector2 _ballVelocity;
        private float _paddleMaxY;
        private float _paddleMinY;
        private Vector2 _playerPaddlePosition;
        private Vector2 _aiPaddlePosition;
        private float _paddleDistance;
        private float _startCountdown;
        
        // Threshold allowed for collision detection
        private const float Threshold = 0.01f;

        public Simulation(GameConfig config, Vector2 size, float paddleLength)
        {
            Config = config;
            _size = size;
            _paddleLength = paddleLength;
            _paddleMinY = -_size.y + _paddleLength / 2;
            _paddleMaxY = _size.y - _paddleLength / 2;
            _paddleDistance = size.y * 4 / 5;
            ResetBallAndPaddles();
        }

        public void Tick(float dt)
        {
            if (_startCountdown > 0)
            {
                _startCountdown -= dt;
                return;
            }

            if (_ballVelocity.Equals(Vector2.zero))
            {
                _ballVelocity = Config.InitialBallSpeed;
            }

            BallPosition += _ballVelocity * dt;
            _aiPaddlePosition.y =
                Mathf.Clamp(_aiPaddlePosition.y + AIPaddleVelocity * dt * Config.MaxAIMovementSpeed,
                    _paddleMinY, _paddleMaxY);
            _playerPaddlePosition.y =
                Mathf.Clamp(_playerPaddlePosition.y + PlayerPaddleVelocity * dt * Config.PlayerMovementSpeed,
                    _paddleMinY, _paddleMaxY);

            CheckPaddleCollision(_playerPaddlePosition);
            CheckPaddleCollision(_aiPaddlePosition);
            CheckTopBottomWallCollision();

            if (Mathf.Abs(BallPosition.x - _size.x) < Threshold)
            {
                // For this version, UI and score logic is not implemented
                Debug.Log("AI Won");
                ResetBallAndPaddles();
            }

            if (Mathf.Abs(BallPosition.x + _size.x) < Threshold)
            {
                Debug.Log("Player Won");
                ResetBallAndPaddles();
            }
        }

        private void ResetBallAndPaddles()
        {
            _aiPaddlePosition = new Vector2(-_paddleDistance, 0);
            _playerPaddlePosition = new Vector2(_paddleDistance, 0);
            _ballVelocity = Vector2.zero;
            _startCountdown = Config.WaitBeforeServe;
            BallPosition = Vector2.zero;
        }

        private void CheckTopBottomWallCollision()
        {
            if (Mathf.Abs(BallPosition.y + _size.y) < Threshold || Mathf.Abs(BallPosition.y - _size.y) < Threshold)
            {
                _ballVelocity.y = -_ballVelocity.y;
            }
        }

        private void CheckPaddleCollision(Vector2 paddlePosition)
        {
            if (Mathf.Abs(BallPosition.x - paddlePosition.x) < Threshold)
            {
                var percent = (BallPosition.y - paddlePosition.y) / _paddleLength;
                // There's collision!
                if (Mathf.Abs(percent) <= 0.5)
                {
                    var vy = Mathf.Abs(_ballVelocity.x) * percent * Config.CollisionMultiplier;
                    _ballVelocity = new Vector2(-_ballVelocity.x, vy);
                }
            }
        }
    }
}