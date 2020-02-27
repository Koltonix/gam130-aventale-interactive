namespace CatGame.Data
{
    public interface IPlayerManager
    {
        Player GetCurrentPlayer();
        Player[] GetAllPlayers();
        Player GetPlayerFromIndex(int index);
    }
}
