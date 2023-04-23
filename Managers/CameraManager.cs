using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CookingGame.Managers
{
    public class CameraManager
    {
        private Vector3 position = new(0,0,0);

        Rectangle viewportSize = new Rectangle(0,0,1280,720);
        public Matrix TransformMatrix { get; private set; }

        public CameraManager()
        {
            ResetCamera();
        }

        public void MoveCamera(Vector3 newPosition)
        {
            position = newPosition;
            var matrix = Matrix.CreateTranslation(0, 0, 0);
            TransformMatrix = Matrix.CreateTranslation(position) * matrix;
        }

        public void ResetCamera()
        {
            var matrix = Matrix.CreateTranslation(0, 0, 0);
            TransformMatrix = Matrix.CreateTranslation(position) * matrix;
        }

    }
}
