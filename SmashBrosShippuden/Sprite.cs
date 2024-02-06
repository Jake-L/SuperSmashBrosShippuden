using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SmashBrosShippuden
{
    class Sprite
    {
        protected Rectangle rectangle;
        protected Texture2D texture;
        public int dx = 0;
        public int dy = 0;

        public Sprite(Texture2D newTexture, Rectangle newRectangle)
        {
            rectangle = newRectangle;
            texture = newTexture;
        }

        public virtual void LoadContent(ContentManager content)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (texture != null)
            {
                spriteBatch.Draw(texture, rectangle, Color.White);
            }
        }
    }
}
