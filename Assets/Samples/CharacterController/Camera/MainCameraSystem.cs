using MyTPS;
using Unity.Entities;
using Unity.Transforms;

//[UpdateInGroup(typeof(PresentationSystemGroup), OrderFirst = true)]
[UpdateInGroup(typeof(LateSimulationSystemGroup), OrderLast = true)]

public partial class MainCameraSystem : SystemBase
{
    protected override void OnUpdate()
    {
        return;
        if (MainGameObjectCamera.Instance != null && SystemAPI.HasSingleton<MainEntityCamera>())
        {
            Entity mainEntityCameraEntity = SystemAPI.GetSingletonEntity<MainEntityCamera>();
            LocalTransform targetLocalToWorld = SystemAPI.GetComponent<LocalTransform>(mainEntityCameraEntity);
            MainGameObjectCamera.Instance.transform.SetLocalPositionAndRotation(targetLocalToWorld.Position, targetLocalToWorld.Rotation);
        }
    }
}