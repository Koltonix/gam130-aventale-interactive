using UnityEngine;
using CatGame.Data;

namespace CatGame.Units
{
    /// <summary>Used to get all of the relevant data for a Unit.</summary>
    public interface IUnitData
    {
        Player GetOwner();
        Player GetCurrentPlayer();
    }
}