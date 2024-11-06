namespace Assets.Scripts
{
    public class UpdateScoreCommand : IUICommand
    {
        public int NewScore;

        public UpdateScoreCommand(int newScore)
        {
            this.NewScore = newScore;
        }
    }
}
