using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects;

public class PatienceBar : SplashImage
{
    private readonly Vector2 _position = new(50, 400);
    public PatienceBar(Texture2D texture) : base(texture)
    {
        Texture = texture;
        Position = _position;
    }
}