using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.WiiU;

namespace WiiVRU.Tracking
{
    public class TrackedObject : MonoBehaviour
    {
        GamePad gamePad;

        public trackedDevice trackedDevice;

        void Start()
        {
            // Initializes the game pad for use in code
            gamePad = GamePad.Access;
        }
        void Update()
        {
            GamePadState gamePadState = gamePad.state;
            switch (trackedDevice)
            {
                case trackedDevice.Head:
                    if (gamePadState.gamePadErr == GamePadError.None)
                    {
                        Vector3 gyroData = gamePadState.gyro;
                        Vector3 gyroDegree = new Vector3(Mathf.Rad2Deg * gyroData.x, -Mathf.Rad2Deg * gyroData.z, Mathf.Rad2Deg * gyroData.y) / 10f;
                        
                        transform.Rotate (gyroDegree);
                    }
                case trackedDevice.LeftHand:
                    handTrack(0);
                    break;
                case trackedDevice.RightHand:
                    handTrack(1);
                    break;
            }
        }

        public void handTrack(int num)
        {
            // Initializes the motion plus on the WiiMote
            MotionPlusState data = Remote.Access(num).state.motionPlus;
            Remote.Access(num).motionPlus.Enable(MotionPlusMode.Standard);

            var look = -data.dir.Y;
            var up = data.dir.Z;

            look.x *= -1;
            up.x *= -1;

            transform.localRotation = Quaternion.LookRotation(up, look);
        }
    }

    public enum trackedDevice
    {
        Head = 0,
        LeftHand = 1,
        RightHand = 2
    }
}