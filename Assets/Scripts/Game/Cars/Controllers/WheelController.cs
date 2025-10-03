using UnityEngine;

namespace CrashyChasy.Game.Cars.Controllers
{
    public sealed class WheelController : MonoBehaviour
    {
        public float RollingSpeed = 20f;
        
        public void Update()
        {
            transform.Rotate(Vector3.right * RollingSpeed, Space.Self);
        }
    }
}