using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SmashBrosShippuden
{
    class Sprite
    {
        protected Rectangle rectangle;
        protected Texture2D texture;

        public Sprite(Texture2D newTexture, Rectangle newRectangle)
        {
            rectangle = newRectangle;
            texture = newTexture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (texture != null)
            {
                spriteBatch.Draw(texture, rectangle, Color.White);
            }
        }
    }
}
