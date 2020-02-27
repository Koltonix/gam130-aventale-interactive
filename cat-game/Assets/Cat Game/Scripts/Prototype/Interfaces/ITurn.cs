namespace CatGame.Data
{
    public interface ITurn
    {
        event OnPlayerCycle AddToListener;
        int GetCurrentPlayerIndex();
    }
}
