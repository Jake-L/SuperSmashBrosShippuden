using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace SmashBrosShippuden
{
    class Sprite
    {
        protected Rectangle rectangle;
        protected Texture2D texture;
        public string direction = "Right";
        public int dx = 0;
        public int? dy = 0;
        public int x;
        public int y;
        protected int hitboxYOffset = 0;
        protected int hitboxWidth;
        protected int hitboxHeight;
        // graphics scaling must be multiple of 3!!
        protected int graphicsScaling = 3;
        protected int spriteScaling = 3;
        protected float rotation = 0f;


        public Sprite(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public virtual void LoadContent(ContentManager content)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (texture != null)
            {
                this.rectangle = new Rectangle(
                    this.x - (this.texture.Width * spriteScaling) / 2,
                    this.y + (this.hitboxYOffset - this.texture.Height) * spriteScaling,
                    this.texture.Width * spriteScaling,
                    this.texture.Height * spriteScaling
                );

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
                    new Vector2(x, y - (hitboxHeight * spriteScaling / 2)), 
                    null, 
                    Color.White, 
                    rotation, 
                    new Vector2(texture.Width / 2, texture.Height - hitboxHeight / 2 - hitboxYOffset),
                    spriteScaling,
                    spriteEffects, 
                    0f
                );
            }
        }

        //send the players rectangle
        public virtual Rectangle getRectangle()
        {
            return rectangle;
        }
    }
}
