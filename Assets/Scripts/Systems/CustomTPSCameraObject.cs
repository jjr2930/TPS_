using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTPS
{
    public class CustomTPSCameraObject : MonoBehaviour
    {
        public static Camera instance;

        public void Awake()
        {
            instance = GetComponent<Camera>();
        }

        public void Update()
        {
            Debug.DrawRay(transform.position, transform.forward * 10f);
        }

        public void LateUpdate()
        {
           //transform.LookAt(Vector3.zero);
        }
    }
}
