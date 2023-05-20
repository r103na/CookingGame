using CookingGame.Enum;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects.Base;

public class BaseSprite
{
    protected Texture2D Texture;

    public Vector2 Position;
    protected float Rotation = 0f;
    protected SpriteEffects Effects;
    protected Vector2 Center => new(Texture.Width / 2f, Texture.Height / 2f);
    protected Vector2 CenterPosition => Position + Center;

    public int Layer;

    protected Color Color = Color.White;

    public virtual void OnNotify(Events eventType) { }


    public void Render(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, CenterPosition, null, Color, Rotation, Center, 1, Effects, 0);
    }

    public void ChangeTexture(Texture2D texture)
    {
        Texture = texture;
    }

    public void ChangePositionSmoothly(Vector2 position, float amount)
    {
        Position.X = MathHelper.Lerp(Position.X, position.X, amount);
        Position.Y = MathHelper.Lerp(Position.Y, position.Y, amount);
    }

}