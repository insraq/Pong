using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Logic : MonoBehaviour
{
    [SerializeField] private GameObject[] paddles = default;
    [SerializeField] private Rigidbody2D ball = default;

    [SerializeField] private Text playerScoreText = default;
    [SerializeField] private Text aiScoreText = default;

    [SerializeField] private GameObject gameOverScreen = default;
    [SerializeField] private Text resultText = default;
    
    [SerializeField] private GameConfig config = default;

    private int _playerScore;
    private int _aiScore;

    private int PlayerScore
    {
        get => _playerScore;
        set
        {
            _playerScore = value;
            playerScoreText.text = _playerScore.ToString();
        }
    }

    private int AIScore
    {
        get => _aiScore;
        set
        {
            _aiScore = value;
            aiScoreText.text = _aiScore.ToString();
        }
    }


    private void Start()
    {
        gameOverScreen.SetActive(false);
        ResetBallAndPaddles();
    }

    private void ResetBallAndPaddles()
    {
        foreach (var paddle in paddles)
        {
            paddle.transform.position = new Vector3(paddle.transform.position.x, 0);
        }

        ball.gameObject.transform.position = Vector2.zero;
        ball.velocity = Vector2.zero;
        StartCoroutine(Serve());
    }

    private IEnumerator Serve()
    {
        yield return new WaitForSeconds(config.WaitBeforeServe);
        ball.velocity = config.InitialBallSpeed;
    }

    public void LoseScore(PlayerType playerType)
    {
        switch (playerType)
        {
            case PlayerType.Player:
                AIScore++;
                break;
            case PlayerType.AI:
                PlayerScore++;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(playerType), playerType, null);
        }

        if (AIScore >= config.MaxScore)
        {
            resultText.text = "AI Won";
            gameOverScreen.SetActive(true);
        }
        else if (PlayerScore >= config.MaxScore)
        {
            resultText.text = "Player Won";
            gameOverScreen.SetActive(true);
        }
        else
        {
            ResetBallAndPaddles();
        }
    }

    public void Restart()
    {
        AIScore = 0;
        PlayerScore = 0;
        ResetBallAndPaddles();
        gameOverScreen.SetActive(false);
    }
}