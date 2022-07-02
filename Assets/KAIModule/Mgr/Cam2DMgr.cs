using UnityEngine;

namespace KAIModule
{
    namespace Mgr
    {
        public class Cam2DMgr
        {
            public Transform T_target { get; set; }
            public Vector3 T_off { get; set; }

            public Cam2DMgr()
            {

            }

            public void LateUpdate()
            {
                if (cam != null && T_target != null)
                    cam.transform.position = Vector3.SmoothDamp(cam.transform.position, T_target.position + T_off, ref speed, 0.5f, 100.0f, Time.deltaTime);
            }

            public void SetCam(Camera cam)
            {
                this.cam = cam;
                speed = Vector3.zero;
            }

            public void TP(Vector3 pos)
            {
                speed = Vector3.zero;
                if (cam != null)
                    cam.transform.position = pos + T_off;
            }

            private Camera cam;
            private Vector3 speed;
        }
    }
}