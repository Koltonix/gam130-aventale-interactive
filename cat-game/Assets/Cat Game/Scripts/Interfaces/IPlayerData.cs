using CatGame.Data;

public interface IPlayerData
{
    Player GetPlayerReference();
    int GetCurrentActionPoints();
    int GetDefaultActionPoints();
    bool GetActiveState();
}
