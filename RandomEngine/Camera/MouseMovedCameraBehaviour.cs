using RandomEngine.Input;

namespace RandomEngine.Camera;

public class MouseMovedCameraBehaviour : ICameraBehaviour
{
    private readonly MouseEngine _mouse;

    public MouseMovedCameraBehaviour(MouseEngine mouse)
    {
        _mouse = mouse;
    }

    public int CameraBorderTriggerSize { get; set; } = 5;

    public void Update(CameraEngine camera)
    {
        var cameraMovement = Vector2.Zero;
        int moveSpeed;

        if (camera.Zoom > .8f)
            moveSpeed = 8;
        else if (camera.Zoom is < .8f and >= .6f)
            moveSpeed = 13;
        else if (camera.Zoom is < .6f and > .35f)
            moveSpeed = 18;
        else
            moveSpeed = camera.Zoom <= .35f ? 33 : 13;

        if (_mouse.PositionScreen.Y < CameraBorderTriggerSize)
            camera.Y -= moveSpeed;

        if (_mouse.PositionScreen.Y > camera.Viewport.Height - CameraBorderTriggerSize)
            camera.Y += moveSpeed;

        if (_mouse.PositionScreen.X < CameraBorderTriggerSize)
            camera.X -= moveSpeed;

        if (_mouse.PositionScreen.X > camera.Viewport.Width - CameraBorderTriggerSize)
            camera.X += moveSpeed;

        if (_mouse.WheelUp)
            camera.ZoomIn();
        else if (_mouse.WheelDown)
            camera.ZoomOut();
    }
}