using CatGame.Data;

/// <summary>Used to get the relevant data from the Player.</summary>
public interface IPlayerData
{
    Player GetPlayerReference();
    int GetCurrentActionPoints();
    int GetDefaultActionPoints();
    bool GetActiveState();
}
