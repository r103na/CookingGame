using Microsoft.Xna.Framework;

namespace CookingGame.Managers;

public class CameraManager
{
    public Vector3 Position = new(0,0,0);

    public Matrix TransformMatrix { get; private set; }

    public CameraManager()
    {
        ResetCamera();
    }

    public void MoveCamera(Vector3 newPosition)
    {
        Position = newPosition;
        var matrix = Matrix.CreateTranslation(0, 0, 0);
        TransformMatrix = Matrix.CreateTranslation(Position) * matrix;
    }

    public void ResetCamera()
    {
        var matrix = Matrix.CreateTranslation(0, 0, 0);
        TransformMatrix = Matrix.CreateTranslation(Position) * matrix;
    }

}