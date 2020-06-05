namespace CardboardCore.Loop
{
    public interface IGameLoopable
    {
        void Start();
        void Stop();
        void Update(double deltaTime);
    }
}