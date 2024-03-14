using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashBrosShippuden
{
    internal class StageObject : Sprite
    {
        string spriteName;

        public StageObject(int x, int y, string spriteName) : base(x, y)
        {
            this.spriteName = spriteName;
        }

        public override void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>(this.spriteName);
        }
    }
}
