using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace MyTPS
{
    public class CustomTPSCameraAuthoring : MonoBehaviour
    {
        public GameObject target;
        [Tooltip("offset")]
        public Vector3 normalOffset;
        public Vector3 aimPositionValue;
        public float rotateSpeed;
        public float elevationMin;
        public float elevationMax;
        public Vector2 zoomMinMax = new Vector2(5f, 12f);
        public float distance;
        public float startPolar;

        public void OnDrawGizmos()
        {
            if (null == target)
            {
                return;
            }
            Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
            Gizmos.DrawSphere(target.transform.position, distance);
        }

        public class Baker : Baker<CustomTPSCameraAuthoring>
        {
            public override void Bake(CustomTPSCameraAuthoring authoring)
            {
                if(authoring.elevationMin >= authoring.elevationMax)
                {
                    Debug.LogWarning("authoring.phiMin >= authroing.phiMax");

                    var temp = authoring.elevationMin;
                    authoring.elevationMin = authoring.elevationMax;
                    authoring.elevationMax = temp;
                }

                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new CustomTPSCamera()
                {
                    mode = CameraMode.Normal,
                    aimOffset = new float3(authoring.aimPositionValue),
                    rotateSpeed = authoring.rotateSpeed,
                    elevationMin = authoring.elevationMin,
                    elevationMax = authoring.elevationMax,
                    zoomMinMax = authoring.zoomMinMax,
                    distance = authoring.distance,
                    polar = authoring.startPolar,
                    normalOffset = authoring.normalOffset
                });

                AddComponent(entity, new CustomTPSCameraTarget()
                {
                    target = GetEntity(authoring.target, TransformUsageFlags.Dynamic)
                });

                AddComponent(entity, new MainEntityCamera());
            }
        }
    }
}
