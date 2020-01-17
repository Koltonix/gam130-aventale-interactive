using UnityEngine;

namespace SiegeOfAshes.Input
{
    public interface IGetInput
    {
        Ray GetRay();
        RaycastHit GetRaycastHit();
        bool HasClicked();
    }
}

