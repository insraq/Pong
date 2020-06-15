using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private PlayerType type = PlayerType.Player;
    [SerializeField] private Logic logic = default;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        logic.LoseScore(type);
    }
}