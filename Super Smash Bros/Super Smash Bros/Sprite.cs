//Jake Loftus
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Super_Smash_Bros
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
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }
}
