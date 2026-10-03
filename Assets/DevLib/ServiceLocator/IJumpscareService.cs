namespace DevLib.ServiceLocator
{
    public interface IJumpscareService
    {
        public void ShowJumpscare(string targetJumpName);
        public void ShowSpotEffect(string targetJumpName);
        public void DeadEffect(string targetJumpName);

    }
}