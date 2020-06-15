namespace Deterministic
{
    public interface IGameAI
    {
        void Tick(Simulation sim);
    }

    public class ImpossibleAI : IGameAI
    {
        public void Tick(Simulation sim)
        {
            var diff = sim.BallPosition.y - sim.AIPaddlePosition.y;
            sim.AIPaddleVelocity = sim.Config.MaxAIMovementSpeed * diff;
        }
    }
}