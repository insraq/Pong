using UnityEngine;

namespace Deterministic
{
    public class DeterministicLogic : MonoBehaviour
    {
        [SerializeField] private GameConfig config = default;
        [SerializeField] private GameObject ball = default;
        [SerializeField] private GameObject paddleLeft = default;
        [SerializeField] private GameObject paddleRight = default;

        private Simulation _simulation;
        private ImpossibleAI _gameAi;

        private void Awake()
        {
            var paddleLength = paddleLeft.GetComponent<SpriteRenderer>().size.y * paddleLeft.transform.localScale.y;
            _simulation = new Simulation(config, new Vector2(5, 4), paddleLength);
            _gameAi = new ImpossibleAI();
        }

        private void Update()
        {
            HandlePlayerInput();
            _gameAi.Tick(_simulation);
            _simulation.Tick(Time.deltaTime);
            RenderGameState();
        }

        private void RenderGameState()
        {
            ball.transform.position = _simulation.BallPosition;
            paddleLeft.transform.position = _simulation.AIPaddlePosition;
            paddleRight.transform.position = _simulation.PlayerPaddlePosition;
        }

        private void HandlePlayerInput()
        {
            _simulation.PlayerPaddleVelocity = Input.GetAxis("Vertical");
        }
    }
}