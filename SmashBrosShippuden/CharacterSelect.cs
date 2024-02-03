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
    internal class CharacterSelect : BaseModel
    {
        //create the character select screen        
        Texture2D[] characterIcons = new Texture2D[Constants.characterList.Length];
        Rectangle[] characterIconsRectangle = new Rectangle[Constants.characterList.Length];
        Texture2D[] charSelectTokens = new Texture2D[4];
        Rectangle[] charSelectTokensRec = new Rectangle[4];
        Texture2D[] characterSplash = new Texture2D[Constants.characterList.Length];
        Rectangle[] characterSplashRec = new Rectangle[4];
        Texture2D[] characterSelect = new Texture2D[4];
        Rectangle[] characterSelectRec = new Rectangle[4];

        public string[] character = new string[4];
        string[] tempCharacter = new string[4];
        int displayWidth;
        int displayHeight;

        public CharacterSelect(int displayWidth, int displayHeight) {
            this.displayWidth = displayWidth;
            this.displayHeight = displayHeight;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);

            for (int i = 0; i < Constants.characterList.Length; i++)
            {
                //loading character select icons
                characterIcons[i] = Content.Load<Texture2D>("Icon/" + Constants.characterList[i].ToLower() + "Icon");
                characterIconsRectangle[i] = new Rectangle(200 + characterIcons[0].Width * (i / 3), 50 + characterIcons[0].Height * (i % 3), characterIcons[0].Width, characterIcons[0].Height);

                //loading splash arts
                characterSplash[i] = Content.Load<Texture2D>("Splash/" + Constants.characterList[i].ToLower() + "Art");
            }

            //display a token for each player to choose his character
            charSelectTokens[0] = Content.Load<Texture2D>("Icon/player1Icon");
            charSelectTokens[1] = Content.Load<Texture2D>("Icon/player2Icon");
            charSelectTokens[2] = Content.Load<Texture2D>("Icon/player3Icon");
            charSelectTokens[3] = Content.Load<Texture2D>("Icon/player4Icon");

            //rectangles to display the characters artwork
            characterSplashRec[0] = new Rectangle(50, (displayHeight * 3) / 5, characterSplash[0].Width / 2, characterSplash[0].Height / 2);
            characterSplashRec[1] = new Rectangle(((displayWidth) / 5) + 50, (displayHeight * 3) / 5, characterSplash[0].Width / 2, characterSplash[0].Height / 2);
            characterSplashRec[2] = new Rectangle(((displayWidth * 2) / 5) + 50, (displayHeight * 3) / 5, characterSplash[0].Width / 2, characterSplash[0].Height / 2);
            characterSplashRec[3] = new Rectangle(((displayWidth * 3) / 5) + 50, (displayHeight * 3) / 5, characterSplash[0].Width / 2, characterSplash[0].Height / 2);

            //loading character select backgrounds
            for (int i = 0; i < characterSelect.Length; i++)
            {
                characterSelect[i] = Content.Load<Texture2D>("characterselectbox" + (i + 1));
                characterSelectRec[i] = new Rectangle(((displayWidth * i) / 5) + 20, (displayHeight * 3) / 5, characterSelect[i].Width, characterSelect[i].Height);
                charSelectTokensRec[i] = new Rectangle(100, 100 + (i * 50), charSelectTokens[i].Width * 2, charSelectTokens[i].Height * 2);
            }
        }

        public override void Update(GamePadState[] gamePad, GameTime gameTime)
        {
            //check if the player has selected a character
            for (int i = 0; i < gamePad.Length; i++)
            {
                int midPointX = charSelectTokensRec[i].Left + (charSelectTokensRec[i].Width / 2);
                int midPointY = charSelectTokensRec[i].Top + (charSelectTokensRec[i].Height / 2);

                charSelectTokensRec[i].X += this.getDx(gamePad[i]);
                charSelectTokensRec[i].Y += this.getDy(gamePad[i]);


                for (int j = 0; j < Constants.characterList.Length; j++)
                {
                    if (charSelectTokensRec[i].Intersects(characterIconsRectangle[j]))
                    {
                        if (midPointX > characterIconsRectangle[j].Left && midPointX < characterIconsRectangle[j].Right && midPointY > characterIconsRectangle[j].Top && midPointY < characterIconsRectangle[j].Bottom)
                        {
                            tempCharacter[i] = Constants.characterList[j];
                        }
                    }
                }
            }

            if (gamePad[0].Buttons.Start == ButtonState.Pressed)
            {
                //lock in their character when they press start
                for (int k = 0; k < gamePad.Length; k++)
                {
                    if (tempCharacter[k] != null)
                    {
                        character[k] = tempCharacter[k];

                    }
                }

                this.complete = true;
            }
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            for (int i = 0; i < Constants.characterList.Length; i++)
            {
                _spriteBatch.Draw(characterIcons[i], characterIconsRectangle[i], Color.White);
            }

            for (int i = 0; i < 4; i++)
            {
                _spriteBatch.Draw(characterSelect[i], characterSelectRec[i], Color.White);
                _spriteBatch.Draw(charSelectTokens[i], charSelectTokensRec[i], Color.White);

                for (int j = 0; j < Constants.characterList.Length; j++)
                {
                    if (tempCharacter[i] == Constants.characterList[j])
                    {
                        _spriteBatch.Draw(characterSplash[j], characterSplashRec[i], Color.White);
                    }
                }
            }
        }
    }
}
