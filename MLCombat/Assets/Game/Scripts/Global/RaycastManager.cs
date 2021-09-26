using UnityEngine;

namespace MiddleAges.Manager
{
    public class RaycastManager : MonoBehaviour
    {
        public LayerMask IgnoreMask;

        private static RaycastHit crosshairTarget;

        void Start()
        {
        }

        void Update()
        {
            Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out crosshairTarget, 5000, ~IgnoreMask);
        }

        public static RaycastHit GetCrosshairTarget() => crosshairTarget;

        public static bool GetCrosshairTarget(LayerMask layerMask, out RaycastHit target)
        {
            target = crosshairTarget;
            if (crosshairTarget.transform && ((1 << crosshairTarget.transform.gameObject.layer) & layerMask) != 0)
            {
                return true;
            }
            return false;
        }

        public static Vector3 GetCrosshairTargetPoint() => crosshairTarget.point;

        public static float GetCrosshairTargetDistance(Transform distanceFrom)
        {
            Debug.DrawLine(distanceFrom.position, crosshairTarget.point);
            return Vector3.Distance(crosshairTarget.point, distanceFrom.position);
        }
    }
}