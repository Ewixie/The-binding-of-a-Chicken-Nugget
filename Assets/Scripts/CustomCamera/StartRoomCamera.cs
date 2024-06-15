namespace CustomCamera
{
    public class StartRoomCamera : RoomCamera
    {
        protected override void Start()
        {
            base.Start();
            CameraController.Instance.SetCamera(virtualCamera);
        }
    }
}