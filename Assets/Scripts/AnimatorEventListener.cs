using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTPS
{
    public class AnimatorEventListener : MonoBehaviour
    {
        public bool jump;
        public void Jump()
        {
            jump = true;
        }

        public void Ground()
        {
            jump = false;
        }

        public void ChangeAimMode()
        {

        }
    }
}
