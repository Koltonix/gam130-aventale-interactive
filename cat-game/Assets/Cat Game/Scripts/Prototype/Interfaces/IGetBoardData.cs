using UnityEngine;

namespace CatGame.Pathfinding
{
    public interface IGetBoardData
    {
        Tile[,] GetTiles();
        Tile GetTileFromWorldPosition(Vector3 position);
        Tile[] GetNeighbouringTiles(Tile tile);

        int GetBoardWidth();
        int GetBoardHeight();

        Vector3 GetBoardCentre();
    }
}