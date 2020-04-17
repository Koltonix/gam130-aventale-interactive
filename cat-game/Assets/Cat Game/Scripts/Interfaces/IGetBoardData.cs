using UnityEngine;
using CatGame.Tiles;

namespace CatGame.Pathfinding
{
    /// <summary>
    /// Used to get all of the Tile Data and also to convert data
    /// relevant to the tile and board.
    /// </summary>
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