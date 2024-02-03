using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashBrosShippuden
{
    internal class StageSelect : BaseModel
    {
        //create the stage select screen
        Texture2D[] stageIcons = new Texture2D[6];
        Rectangle[] stageIconsRec = new Rectangle[6];
        Texture2D cursorSprite;
        Rectangle cursorRec;
        public int stageIndex = -1;
        private int stageTimer = 0;

        public StageSelect() { }

        protected override void Initialize()
        {
            return;
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);

            //create icons for the stage select screen
            stageIcons[0] = Content.Load<Texture2D>("stageicon1");
            stageIcons[1] = Content.Load<Texture2D>("stageicon2");
            stageIcons[2] = Content.Load<Texture2D>("stageicon3");
            stageIcons[3] = Content.Load<Texture2D>("stageicon4");
            stageIcons[4] = Content.Load<Texture2D>("stageicon5");
            stageIcons[5] = Content.Load<Texture2D>("stageicon6");

            //create rectangles for the icons
            stageIconsRec[0] = new Rectangle(200, 100, stageIcons[0].Width, stageIcons[0].Height);
            stageIconsRec[1] = new Rectangle(stageIconsRec[0].Right, 100, stageIcons[0].Width, stageIcons[0].Height);
            stageIconsRec[2] = new Rectangle(200, stageIconsRec[0].Bottom, stageIcons[0].Width, stageIcons[0].Height);
            stageIconsRec[3] = new Rectangle(stageIconsRec[0].Right, stageIconsRec[0].Bottom, stageIcons[0].Width, stageIcons[0].Height);
            stageIconsRec[4] = new Rectangle(stageIconsRec[1].Right, stageIconsRec[0].Top, stageIcons[0].Width, stageIcons[0].Height);
            stageIconsRec[5] = new Rectangle(stageIconsRec[3].Right, stageIconsRec[0].Bottom, stageIcons[0].Width, stageIcons[0].Height);

            cursorSprite = Content.Load<Texture2D>("Icon/player1Icon");
            cursorRec = new Rectangle(100, 100, cursorSprite.Width * 2, cursorSprite.Height * 2);
        }

        public override void Update(GamePadState[] gamePad, GameTime gameTime)
        {
            this.stageTimer++;
            cursorRec.X += this.getDx(gamePad[0]);
            cursorRec.Y += this.getDy(gamePad[0]);
            int midPointX = cursorRec.Left + (cursorRec.Width / 2);
            int midPointY = cursorRec.Top + (cursorRec.Height / 2);

            for (int j = 0; j < stageIconsRec.Length; j++)
            {
                if (midPointX > stageIconsRec[j].Left && midPointX < stageIconsRec[j].Right && midPointY > stageIconsRec[j].Top && midPointY < stageIconsRec[j].Bottom && gamePad[0].Buttons.Start == ButtonState.Pressed && stageTimer > 60)
                {
                    this.stageIndex = j;
                    this.complete = true;
                }
            }
        }

        public int getStage()
        {
            return this.stageIndex;
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            for (int i = 0; i < stageIconsRec.Length; i++)
            {
                _spriteBatch.Draw(stageIcons[i], stageIconsRec[i], Color.White);
            }

            _spriteBatch.Draw(cursorSprite, cursorRec, Color.White);
        }
    }
}
