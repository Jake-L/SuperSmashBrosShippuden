using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SmashBrosShippuden
{
    class Sprite
    {
        protected Rectangle rectangle;
        protected Texture2D texture;
        public string direction = "Right";
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
                SpriteEffects spriteEffects;

                if (this.direction == "Right")
                {
                    spriteEffects = SpriteEffects.None;
                }
                else
                {
                    spriteEffects = SpriteEffects.FlipHorizontally;
                }

                spriteBatch.Draw(
                    texture, 
                    rectangle, 
                    null,
                    Color.White,
                    0f,
                    new Vector2(0, 0),
                    spriteEffects,
                    0f
                );
            }
        }

        //send the players rectangle
        public Rectangle getRectangle()
        {
            return rectangle;
        }
    }
}
