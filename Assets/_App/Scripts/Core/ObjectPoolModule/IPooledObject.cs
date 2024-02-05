namespace VgGames.Core.ObjectPoolModule
{
    public interface IPooledObject
    {
        public bool GetIsEnabled();
        public void Enable();
        public void Disable();
    }
}