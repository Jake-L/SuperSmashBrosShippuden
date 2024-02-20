//Jake Loftus
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SmashBrosShippuden
{
    class Projectiles : Sprite
    {
        string character;
        public int player;
        int startpointY;
        int startpointX;
        int startpointbot;
        int counter;
        int dx = 0;
        int damage;
        int knockback;

        Texture2D[] Projectile;
        int spriteLength = 1;

        Texture2D[] PichuLightning = new Texture2D[3];
        Texture2D[] PichuCloud = new Texture2D[4];

        Texture2D PichuCloudDraw;

        Rectangle cloudRec;

        //Game1 game = new Game1();

        public Projectiles(Rectangle newRectangle, Texture2D newTexture, string newDirection, int newPlayer, string newCharacter) : base(newTexture, newRectangle)
        {
            direction = newDirection;
            player = newPlayer;
            character = newCharacter;
            startpointY = newRectangle.Top;
            startpointX = newRectangle.Left + (newRectangle.Width / 2);
            startpointbot = newRectangle.Bottom;

            if (character == "Blastoise")
            {
                damage = 4;
                knockback = 0;
            }
            else if (character == "Mario" || character == "Luigi")
            {
                damage = 4;
                knockback = 0;
            }
            else if (character == "Mewtwo")
            {
                damage = 4;
                knockback = 1;
            }
            else if (character == "Pichu")
            {
                damage = 2;
                knockback = 1;
            }

            Initialize();
        }

        public void Update(GameTime gameTime)
        {
            if (character != "Pichu")
            {
                texture = Projectile[(gameTime.TotalGameTime.Milliseconds / 100) % spriteLength];

                if (direction == "Left")
                {
                    rectangle.X -= dx;
                }

                if (direction == "Right")
                {
                    rectangle.X += dx;
                }

                rectangle.Width = Projectile[0].Width * 4;
                rectangle.Height = Projectile[0].Height * 4;
            }

            if (new[] { "Mario", "Luigi", "Mewtwo" }.Contains(character))
            {
                rectangle.Y = startpointY + (int)(Math.Sin((gameTime.TotalGameTime.Milliseconds / 100)) * 20);
            }

            else if (character == "Pichu")
            {
                if (gameTime.TotalGameTime.Milliseconds % 100 == 0)
                {
                    counter++;
                }
                Pichu();
            }
        }

        public void Initialize()
        {
            if (character == "Mewtwo")
            {
                spriteLength = 2;
                dx = 6;
                damage = 4;
                knockback = 1;
            }

            else if (character == "Pichu")
            {
                damage = 2;
                knockback = 1;
            }

            else if (character == "Blastoise")
            {
                spriteLength = 4;
                damage = 4;
                knockback = 0;
            }

            else // Mario, Luigi, Charizard
            {
                spriteLength = 4;
                dx = 4;
                damage = 4;
                knockback = 0;
            }
        }

        public override void LoadContent(ContentManager content)
        {
            Projectile = new Texture2D[spriteLength];

            if (character == "Mewtwo")
            {
                Projectile[0] = content.Load<Texture2D>("Mewtwo/mewtwoBall1");
                Projectile[1] = content.Load<Texture2D>("Mewtwo/mewtwoBall2");
            }

            else if (character == "Pichu")
            {
                PichuCloudDraw = content.Load<Texture2D>("Pichu/cloud1");
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
                for (int i = 0; i < spriteLength; i++)
                {
                    if (character == "Mario")
                    {
                        Projectile[i] = content.Load<Texture2D>("Mario/marioFire" + (i + 1));
                    }
                    if (character == "Luigi")
                    {
                        Projectile[i] = content.Load<Texture2D>("Luigi/luigiFire" + (i + 1));
                    }
                    if (character == "Blastoise")
                    {
                        Projectile[i] = content.Load<Texture2D>("Blastoise/blastoiseWater" + (i + 1));
                    }
                    if (character == "Charizard")
                    {
                        Projectile[i] = content.Load<Texture2D>(character + "/fire" + (i + 1));
                    }
                }
            }
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (character == "Pichu" && counter < PichuCloud.Length + PichuLightning.Length)
            {
                spriteBatch.Draw(PichuCloudDraw, cloudRec, Color.White);
            }
            base.Draw(spriteBatch);
        }
    }
}