namespace CatGame.Data
{
    /// <summary>Used to get access to the Player references.</summary>
    public interface IPlayerManager
    {
        Player GetCurrentPlayer();
        Player[] GetAllPlayers();
        Player GetPlayerFromIndex(int index);
    }
}
