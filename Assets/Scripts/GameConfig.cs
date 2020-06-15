using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "GameConfig", order = 0)]
public class GameConfig : ScriptableObject
{
    public float PlayerMovementSpeed = 5;
    
    // This limits the max AI speed so the player has a chance to win
    public float MaxAIMovementSpeed = 5;
    
    public float CollisionMultiplier = 2;
    
    public int MaxScore = 10;

    public Vector2 InitialBallSpeed = new Vector2(4, 2);

    public int WaitBeforeServe = 1;
}