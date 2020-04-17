using UnityEngine;

namespace CatGame.Controls
{
    /// <summary>Used to get all of the relevant Input Data.</summary>
    public interface IGetOnClick
    {
        RaycastHit GetRaycastHit();
        Ray GetRay();
        bool IsMovementSelected();
    }
}
