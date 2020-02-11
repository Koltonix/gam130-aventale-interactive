using UnityEngine;

namespace SiegeOfAshes.Controls
{
    public interface IGetOnClick
    {
        RaycastHit GetRaycastHit();
        Ray GetRay();
    }
}
