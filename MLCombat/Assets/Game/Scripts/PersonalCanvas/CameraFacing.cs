using UnityEngine;

namespace MiddleAges.PersonalCanvas
{
    public class CameraFacing : MonoBehaviour
    {
        void Update()
        {
            transform.rotation = new Quaternion(transform.rotation.x, Camera.main.transform.rotation.y, transform.rotation.z, Camera.main.transform.rotation.w);
        }
    }
}
