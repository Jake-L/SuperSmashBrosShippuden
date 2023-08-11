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
    class Projectiles:Sprite
    {
        string direction;
        string character;
        int player;
        int startpointY;
        int startpointX;
        int startpointbot;
        int counter;

        Texture2D[] ProjectileLeft;
        Texture2D[] ProjectileRight;
        int spriteLength = 1;

        Texture2D[] PichuLightning = new Texture2D[3];
        Texture2D[] PichuCloud = new Texture2D[4];

        Texture2D PichuCloudDraw;

        ContentManager content;
        Rectangle cloudRec;

        //Game1 game = new Game1();

        public Projectiles(Rectangle newRectangle, Texture2D newTexture, string newDirection, int newPlayer, string newCharacter, ContentManager cnt):base(newTexture, newRectangle)
        {
            direction = newDirection;
            player = newPlayer;
            content = cnt;
            character = newCharacter;
            startpointY = newRectangle.Top;
            startpointX = newRectangle.Left + (newRectangle.Width / 2);
            startpointbot = newRectangle.Bottom;

            if (character == "Mewtwo")
            {
                spriteLength = 2;
            }
            if (character == "Luigi" || character == "Mario" || character == "Blastoise" || character == "Charizard" || character == "CharizardX")
            {
                spriteLength = 4;
            }
            if (character == "Pichu")
            {
                PichuCloudDraw = content.Load<Texture2D>("Pichu/cloud1");
            }
            else
            {
                ProjectileLeft = new Texture2D[spriteLength];
                ProjectileRight = new Texture2D[spriteLength];
            }
            
            Initialize();
        }

        public void Update(GameTime gameTime)
        {      
            if (character == "Mewtwo" || character == "Blastoise" || character == "Charizard" || character == "CharizardX")
            {
                if (direction == "Left")
                {
                    texture = ProjectileLeft[(gameTime.TotalGameTime.Milliseconds / 100) % 2];
                }

                if (direction == "Right")
                {
                    texture = ProjectileRight[(gameTime.TotalGameTime.Milliseconds / 100) % 2];
                }
            }

            if (character == "Mario")
            {
                texture = ProjectileLeft[(gameTime.TotalGameTime.Milliseconds / 100) % 4];
                rectangle.Y = startpointY + (int)(Math.Sin((gameTime.TotalGameTime.Milliseconds / 100)) * 15); 
            }

            if (character == "Luigi")
            {
                texture = ProjectileLeft[(gameTime.TotalGameTime.Milliseconds / 100) % 4];
                rectangle.Y = startpointY + (int)(Math.Sin((gameTime.TotalGameTime.Milliseconds / 100)) * 15);  
            }

            if (character != "Pichu")
            {
                rectangle.Width = ProjectileLeft[0].Width * 4;
                rectangle.Height = ProjectileLeft[0].Height * 4;
            }

            if (character == "Mewtwo")
            {
                if (direction == "Right")
                {
                    rectangle.X += 6;
                }

                if (direction == "Left")
                {
                    rectangle.X -= 6;
                }
            }

            else if (character == "Pichu")
            {
                if (gameTime.TotalGameTime.Milliseconds % 100 == 0)
                {
                    counter++;                    
                }
                Pichu();
            }

            else if (direction == "Right" && character != "Blastoise")
            {
                rectangle.X += 4;
            }

            else if (direction == "Left" && character != "Blastoise")
            {
                rectangle.X -= 4;
            }
        }

        public void Initialize()
        {
            if (character == "Mewtwo")
            {
                ProjectileLeft[0] = content.Load<Texture2D>("Mewtwo/mewtwoBallLeft1");
                ProjectileLeft[1] = content.Load<Texture2D>("Mewtwo/mewtwoBallLeft2");
                ProjectileRight[0] = content.Load<Texture2D>("Mewtwo/mewtwoBallRight1");
                ProjectileRight[1] = content.Load<Texture2D>("Mewtwo/mewtwoBallRight2");
            }

            else if (character == "Pichu")
            {
                PichuCloud[0] = content.Load<Texture2D>("Pichu/cloud1");
                PichuCloud[1] = content.Load<Texture2D>("Pichu/cloud2");
                PichuCloud[2] = content.Load<Texture2D>("Pichu/cloud3");
                PichuCloud[3] = content.Load<Texture2D>("Pichu/cloud4");
                PichuLightning[0] = content.Load<Texture2D>("Pichu/lightning1");
                PichuLightning[1] = content.Load<Texture2D>("Pichu/lightning2");
                PichuLightning[2] = content.Load<Texture2D>("Pichu/lightning3");
                cloudRec = new Rectangle(startpointX - (PichuCloud[0].Width), 0, PichuCloud[0].Width * 2, PichuCloud[0].Height * 2);
                rectangle.Y = 9999;
                rectangle.X = 9999;
            }

            else
            {
                for (int i = 0; i < 4; i++)
                {
                    if (character == "Mario")
                    {
                        ProjectileLeft[i] = content.Load<Texture2D>("Mario/marioFire" + (i + 1));
                    }
                    if (character == "Luigi")
                    {
                        ProjectileLeft[i] = content.Load<Texture2D>("Luigi/luigiFire" + (i + 1));
                    }
                    if (character == "Blastoise")
                    {
                        ProjectileLeft[i] = content.Load<Texture2D>("Blastoise/blastoiseWaterLeft" + (i + 1));
                        ProjectileRight[i] = content.Load<Texture2D>("Blastoise/blastoiseWaterRight" + (i + 1));
                    }
                    if (character == "Charizard")
                    {
                        ProjectileLeft[i] = content.Load<Texture2D>(character + "/fireLeft" + (i + 1));
                        ProjectileRight[i] = content.Load<Texture2D>(character + "/fireRight" + (i + 1));
                    }
                }
            }
        }

        public Rectangle getRectangle()
        {
            return rectangle;
        }

        //display Pichu's cloud and lightning
        public void Pichu()
        {
            if (counter < PichuCloud.Length)
            {
                PichuCloudDraw = PichuCloud[counter];
            }
            else if (counter < PichuCloud.Length + PichuLightning.Length)
            {
                rectangle.X = startpointX - (rectangle.Width / 2);
                rectangle.Y = 50;
                rectangle.Width = PichuLightning[0].Width * 2;
                rectangle.Height = startpointbot - 50;
                texture = PichuLightning[counter - PichuCloud.Length];
                PichuCloudDraw = PichuCloud[3];
            }
            else
            {
                destroyRec();
            }
        }

        //make the projectile disappear
        public void destroyRec()
        {
            rectangle.Y = 9999;
            rectangle.X = 9999;
            startpointY = 9999;
        }

        public void Draw2(SpriteBatch spriteBatch)
        {
            if (character == "Pichu" && counter < PichuCloud.Length + PichuLightning.Length)
            {
                spriteBatch.Draw(PichuCloudDraw, cloudRec, Color.White);
            }
        }
    }
}
