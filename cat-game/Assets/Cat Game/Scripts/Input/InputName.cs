using System;

namespace CatGame.Controls
{
    /// <summary>
    /// A Data holder for the name of the Input in the InputManager
    /// and the data that is provided to it.
    /// </summary>
    [Serializable]
    public struct InputName
    {
        public string name;
        public float value;
    }
}

