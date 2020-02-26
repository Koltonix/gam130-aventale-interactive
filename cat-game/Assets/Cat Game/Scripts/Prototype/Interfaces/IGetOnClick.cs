using UnityEngine;

namespace CatGame.Controls
{
    public interface IGetOnClick
    {
        RaycastHit GetRaycastHit();
        Ray GetRay();
        bool IsMovementSelected();
    }
}
