using System;
using UnityEngine;

namespace SiegeOfAshes.Data
{
    /// <summary>
    /// Stores the data of each player that is currently playing.
    /// </summary>
    [Serializable]
    public class Player
    {
        public int number;
        public Color32 colour;
    }
}
