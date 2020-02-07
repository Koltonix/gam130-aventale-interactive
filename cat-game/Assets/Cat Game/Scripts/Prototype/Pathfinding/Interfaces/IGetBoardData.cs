using UnityEngine;

namespace SiegeOfAshes.Pathfinding
{
    public interface IGetBoardData
    {
        Tile[,] GetTiles();
        int GetBoardWidth();
        int GetBoardHeight();
        Vector3 GetBoardCentre();
    }
}