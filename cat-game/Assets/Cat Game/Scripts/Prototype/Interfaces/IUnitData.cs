using UnityEngine;
using CatGame.Data;

namespace CatGame.Units
{
    public interface IUnitData
    {
        Player GetOwner();
        Player GetCurrentPlayer();
    }
}