namespace Scripts.StateMachine
{
    public interface IState
    {
        void Start();
        void Tick();
        void End();
    }
}