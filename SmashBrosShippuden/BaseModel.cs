using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SmashBrosShippuden
{
    internal class BaseModel
    {
        protected bool complete = false;
        protected SpriteFont font1;
        protected int counter = 0;

        protected virtual void Initialize()
        {
            return;
        }

        public virtual void LoadContent(ContentManager Content)
        {
            font1 = Content.Load<SpriteFont>("SpriteFont1");

            return;
        }

        public virtual void Update(GamePadState[] gamePad, GameTime gameTime)
        {
            this.counter++;
            return;
        }

        public virtual void Draw(SpriteBatch _spriteBatch)
        {
            return;
        }

        public virtual bool isComplete()
        {
            return complete;
        }

        protected int getDx(GamePadState gamePad)
        {
            if (gamePad.ThumbSticks.Left.X > 0)
            {
                return 4;
            }
            else if (gamePad.ThumbSticks.Left.X < 0)
            {
                return -4;
            }
            else
            {
                return 0;
            }
        }

        protected int getDy(GamePadState gamePad)
        {
            if (gamePad.ThumbSticks.Left.Y > 0)
            {
                return -4;
            }
            else if (gamePad.ThumbSticks.Left.Y < 0)
            {
                return 4;
            }
            else
            {
                return 0;
            }
        }
    }
}
