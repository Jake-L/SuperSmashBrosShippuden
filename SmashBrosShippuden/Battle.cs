using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SmashBrosShippuden
{
    internal class Battle : BaseModel
    {
        //Displaying the characters
        Rectangle[] PlayerPicBox = new Rectangle[4];
        Rectangle[] ProjectPicBox = new Rectangle[4];
        Character[] PlayerClass = new Character[4];
        string[] character = new string[4];
        int[] picBoxWidthScaling = new int[4];
        int[] playerLastHit = new int[4];
        int[] score = new int[4];
        int damageDealt;
        int knockbackAbility;
        Boolean player2Bot;

        //background images
        Texture2D finalDestination;
        Texture2D background;
        Rectangle finalDestinationRec;
        Rectangle backgroundRec;

        //create waddledees
        Enemy[] waddle = new Enemy[12];
        Enemy[] pichuBro = new Enemy[4];
        Texture2D waddleSprite;

        int stageIndex;
        int stageHeightAdjustment = 0;

        string[] direction = new string[4] { "Right", "Right", "Left", "Left" };
        int[] attackType = new int[4];
        int[] knockback = new int[4];
        int[] knockup = new int[4];

        Song[] backgroundMusic = new Song[3];
        int displayWidth;
        int displayHeight;
        Random NumberGenerator = new Random();

        // TODO: remove this
        ContentManager content;

        public Battle(int stageIndex, string[] character, int displayWidth, int displayHeight) 
        {
            this.stageIndex = stageIndex;
            this.character = character;
            this.displayWidth = displayWidth;
            this.displayHeight = displayHeight;
        }

        protected override void Initialize()
        {
            return;
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);

            //loading final destination stage
            if (this.stageIndex == 0)
            {
                finalDestination = Content.Load<Texture2D>("background1");
                background = Content.Load<Texture2D>("background");
                backgroundMusic[0] = Content.Load<Song>("backgroundMusic1");
                backgroundMusic[1] = Content.Load<Song>("backgroundMusic2");
                backgroundMusic[2] = Content.Load<Song>("backgroundMusic3");
                stageHeightAdjustment = finalDestinationRec.Height / 5;
            }
            //loading shrek stage
            if (this.stageIndex == 1)
            {
                finalDestination = Content.Load<Texture2D>("stageShrek");
                background = Content.Load<Texture2D>("backgroundshrek");
                backgroundMusic[0] = Content.Load<Song>("musicshrek1");
                backgroundMusic[1] = Content.Load<Song>("musicshrek1");
                backgroundMusic[2] = Content.Load<Song>("musicshrek1");
            }
            //loading pokemon stadium 1 stage
            if (this.stageIndex == 2)
            {
                finalDestination = Content.Load<Texture2D>("backgroundpokemon2");
                background = Content.Load<Texture2D>("backgroundpokemon3");
                backgroundMusic[0] = Content.Load<Song>("musicpokemon1");
                backgroundMusic[1] = Content.Load<Song>("musicpokemon1");
                backgroundMusic[2] = Content.Load<Song>("musicpokemon1");
                stageHeightAdjustment = finalDestinationRec.Height / 5;
            }
            //loading pokemon stadium 2 stage
            if (this.stageIndex == 3)
            {
                finalDestination = Content.Load<Texture2D>("backgroundpokemon");
                background = Content.Load<Texture2D>("backgroundpokemon3");
                backgroundMusic[0] = Content.Load<Song>("musicpokemon4");
                backgroundMusic[1] = Content.Load<Song>("musicpokemon5");
                backgroundMusic[2] = Content.Load<Song>("musicpokemon6");
                stageHeightAdjustment = finalDestinationRec.Height / 5;
            }
            //loading sonic stage
            if (this.stageIndex == 4)
            {
                finalDestination = Content.Load<Texture2D>("stageSonic");
                background = Content.Load<Texture2D>("backgroundsonic");
                backgroundMusic[0] = Content.Load<Song>("musicsonic1");
                backgroundMusic[1] = Content.Load<Song>("musicsonic1");
                backgroundMusic[2] = Content.Load<Song>("musicsonic1");
            }
            //loading mario stage
            if (this.stageIndex == 5)
            {
                finalDestination = Content.Load<Texture2D>("stageMario");
                background = Content.Load<Texture2D>("backgroundmario");
                backgroundMusic[0] = Content.Load<Song>("musicmario1");
                backgroundMusic[1] = Content.Load<Song>("musicmario2");
                backgroundMusic[2] = Content.Load<Song>("musicmario3");
                stageHeightAdjustment = (displayHeight / 5) + 10;
            }

            //create the stage rectangle
            finalDestinationRec = new Rectangle(displayWidth / 10, (displayHeight * 2) / 5, displayWidth - (displayWidth / 5), (int)((displayWidth - (displayWidth / 5)) * ((float)finalDestination.Height / finalDestination.Width)));
            backgroundRec = new Rectangle(0, 0, displayWidth, displayHeight);

            //creating the waddle dee
            waddleSprite = Content.Load<Texture2D>("waddle/waddleRunLeft1");
            this.content = Content;

            // TODO: move this to initialize, remove Content argument from Character class
            for (int k = 0; k < 4; k++)
            {
                if (character[k] != null)
                {
                    PlayerClass[k] = new Character(PlayerPicBox[k], null, direction[k], k, character[k], Content, displayWidth, displayHeight, finalDestinationRec, stageHeightAdjustment, false);

                    if (character[k] == "Pichu")
                    {
                        pichuBro[k] = new Enemy(new Rectangle(PlayerPicBox[k].X - 50, PlayerPicBox[k].Y, PlayerPicBox[k].Width, PlayerPicBox[k].Height), null, "Left", k, character[k], Content, displayWidth, displayHeight, finalDestinationRec, stageHeightAdjustment, true);
                    }
                }
            }

            if (character[0] != null && character[1] == null && character[2] == null && character[3] == null)
            {
                player2Bot = true;
                character[1] = "Mario";
                PlayerClass[1] = new Character(PlayerPicBox[1], null, direction[1], 1, character[1], Content, displayWidth, displayHeight, finalDestinationRec, stageHeightAdjustment, true);
            }

            for (int i = 0; i < 4; i++)
            {
                if (character[i] != null && picBoxWidthScaling[i] == 0)
                {
                    if (character[i] == "Knuckles" || character[i] == "Mewtwo" || character[i] == "King" || character[i] == "Sonic" || character[i] == "Pichu" || character[i] == "Charizard")
                    {
                        picBoxWidthScaling[i] = (PlayerPicBox[i].Width / 5);
                    }
                    else if (character[i] == "Shrek" || character[i] == "Shadow")
                    {
                        picBoxWidthScaling[i] = (PlayerPicBox[i].Width / 4);
                    }
                    else if (character[i] == "Metaknight" || character[i] == "Seiar" || character[i] == "Kirby" || character[i] == "Mario" || character[i] == "Luigi" || character[i] == "Link")
                    {
                        picBoxWidthScaling[i] = (PlayerPicBox[i].Width / 3);
                    }
                }
            }
        }

        public override void Update(GamePadState[] gamePad, GameTime gameTime)
        {
            base.Update(gamePad, gameTime);

            for (int i = 0; i < 4; i++)
            {
                if (character[i] != null)
                {
                    if (i == 1 && this.player2Bot)
                    {
                        this.controlBot();
                    }

                    //update the characters locations
                    PlayerClass[i].Update(gameTime);
                    PlayerPicBox[i] = PlayerClass[i].setRectangle();
                    direction[i] = PlayerClass[i].directionPlayer();

                    //update the projectiles location
                    if (PlayerClass[i].checkProjectile() == true)
                    {
                        ProjectPicBox[i] = PlayerClass[i].setProjectileRec();
                    }

                    if (character[i] == "King" && attackType[i] == 2)
                    {
                        for (int j = (i) * 3; j < ((i + 1) * 3); j++)
                        {
                            if (waddle[j] == null)
                            {
                                waddle[j] = new Enemy(new Rectangle(PlayerPicBox[i].X + (PlayerPicBox[i].Width / 2), PlayerPicBox[i].Y + (PlayerPicBox[i].Height / 2), 56, 52), null, "Left", i, "waddle", this.content, displayWidth, displayHeight, finalDestinationRec, stageHeightAdjustment, true);
                                break;
                            }
                        }
                    }
                }
            }

            lives();

            damage();
            damageWaddle(gameTime);
            damagePichu(gameTime);


            //correcting hitboxes, only character not adjusted here is blastoise
            if (counter == 7200)
            {
                this.complete = true;
            }
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            //backgrounds
            _spriteBatch.Draw(background, backgroundRec, Color.White);
            _spriteBatch.Draw(finalDestination, finalDestinationRec, Color.White);

            //health
            for (int i = 0; i < 4; i++)
            {
                if (character[i] != null)
                {
                    PlayerClass[i].DrawText(_spriteBatch);
                    _spriteBatch.Draw(PlayerClass[i].seriesSymbol, PlayerClass[i].seriesSymbolRec, Color.White);
                    _spriteBatch.DrawString(font1, PlayerClass[i].damageTaken.ToString() + "%", new Vector2(205 + (i * 200), 5 + (displayHeight * 4) / 5), Color.Black);
                    _spriteBatch.DrawString(font1, PlayerClass[i].damageTaken.ToString() + "%", new Vector2(200 + (i * 200), (displayHeight * 4) / 5), Color.White);
                    _spriteBatch.Draw(PlayerClass[i].LivesIcon, PlayerClass[i].LivesIconRec, Color.White);
                }
            }

            //display a timer at the top of the screen
            if ((((7200 - counter) / 60) % 60) > 9)
            {
                _spriteBatch.DrawString(font1, (((7200 - counter) / 3600).ToString() + ":" + (((7200 - counter) / 60) % 60)), new Vector2(5 + (displayWidth * 2 / 5), 5 + (displayHeight / 20)), Color.Black);
                _spriteBatch.DrawString(font1, (((7200 - counter) / 3600).ToString() + ":" + (((7200 - counter) / 60) % 60)), new Vector2(displayWidth * 2 / 5, displayHeight / 20), Color.White);
            }
            else
            {
                _spriteBatch.DrawString(font1, (((7200 - counter) / 3600).ToString() + ":0" + (((7200 - counter) / 60) % 60)), new Vector2(5 + (displayWidth * 2 / 5), 5 + (displayHeight / 20)), Color.Black);
                _spriteBatch.DrawString(font1, (((7200 - counter) / 3600).ToString() + ":0" + (((7200 - counter) / 60) % 60)), new Vector2(displayWidth * 2 / 5, displayHeight / 20), Color.White);
            }

            //draw the characters on the screen
            PlayerClass[0].Draw(_spriteBatch);

            if (character[1] != null)
            {
                PlayerClass[1].Draw(_spriteBatch);
            }
            if (character[2] != null)
            {
                PlayerClass[2].Draw(_spriteBatch);
            }
            if (character[3] != null)
            {
                PlayerClass[3].Draw(_spriteBatch);
            }

            for (int i = 0; i < waddle.Length; i++)
            {
                if (waddle[i] != null)
                {
                    waddle[i].Draw(_spriteBatch);
                }
            }
            for (int i = 0; i < pichuBro.Length; i++)
            {
                if (pichuBro[i] != null)
                {
                    pichuBro[i].Draw(_spriteBatch);
                }
            }
        }

        public void damage()
        {
            for (int i = 0; i < 4; i++)
            {
                if (PlayerPicBox[i].IsEmpty == false)
                {
                    attackType[i] = PlayerClass[i].attackType();

                    //deal different amounts of damage based on who the character is
                    if (attackType[i] == 1 || (attackType[i] == 2 && (character[i] == "Knuckles" || character[i] == "Kirby" || character[i] == "Link")))
                    {
                        if (character[i] == "Mario")
                        {
                            damageDealt = 10;
                            knockbackAbility = 2;
                        }
                        if (character[i] == "Luigi")
                        {
                            damageDealt = 10;
                            knockbackAbility = 2;
                        }
                        if (character[i] == "Mewtwo")
                        {
                            damageDealt = 13;
                            knockbackAbility = 2;
                        }
                        if (character[i] == "Shadow")
                        {
                            damageDealt = 13;
                            knockbackAbility = 2;
                        }
                        if (character[i] == "Knuckles")
                        {
                            if (attackType[i] == 1)
                            {
                                damageDealt = 9;
                                knockbackAbility = 2;
                            }
                            if (attackType[i] == 2)
                            {
                                damageDealt = 13;
                                knockbackAbility = 1;
                            }
                        }
                        if (character[i] == "Sonic")
                        {
                            damageDealt = 9;
                            knockbackAbility = 2;
                        }
                        if (character[i] == "Shrek")
                        {
                            damageDealt = 10;
                            knockbackAbility = 1;
                        }
                        if (character[i] == "Blastoise")
                        {
                            damageDealt = 8;
                            knockbackAbility = 2;
                        }
                        if (character[i] == "Pichu")
                        {
                            damageDealt = 8;
                            knockbackAbility = 1;
                        }
                        if (character[i] == "Charizard")
                        {
                            damageDealt = 10;
                            knockbackAbility = 2;
                        }
                        if (character[i] == "Kirby")
                        {
                            if (attackType[i] == 1)
                            {
                                damageDealt = 7;
                                knockbackAbility = 1;
                            }
                            if (attackType[i] == 2)
                            {
                                damageDealt = 9;
                                knockbackAbility = 2;
                            }
                        }
                        if (character[i] == "Metaknight")
                        {
                            damageDealt = 9;
                            knockbackAbility = 1;
                        }
                        if (character[i] == "King")
                        {
                            damageDealt = 15;
                            knockbackAbility = 2;
                        }
                        if (character[i] == "Link")
                        {
                            if (attackType[i] == 1)
                            {
                                damageDealt = 8;
                                knockbackAbility = 1;
                            }
                            else
                            {
                                damageDealt = 9;
                                knockbackAbility = 2;
                            }
                        }
                        if (character[i] == "Seiar")
                        {
                            if (attackType[i] == 1)
                            {
                                damageDealt = 7;
                                knockbackAbility = 1;
                            }
                            else if (attackType[i] == 2)
                            {
                                damageDealt = 12;
                                knockbackAbility = 3;
                            }
                        }

                        //check if the players attacks hit anyone
                        for (int j = 0; j < 4; j++)
                        {
                            if (j != i && PlayerPicBox[j].IsEmpty == false)
                            {
                                if (PlayerPicBox[i].Intersects(PlayerPicBox[j]))
                                {
                                    if ((direction[i] == "Left" || character[i] == "Blastoise" || character[i] == "Pichu") && intersection(PlayerPicBox[i], PlayerPicBox[j], picBoxWidthScaling[j], "Left"))
                                    {
                                        PlayerClass[j].getDamage(damageDealt + NumberGenerator.Next(-3, 4), -(knockbackAbility));
                                        playerLastHit[j] = i;
                                    }

                                    else if ((direction[i] == "Right" || character[i] == "Blastoise" || character[i] == "Pichu") && intersection(PlayerPicBox[i], PlayerPicBox[j], picBoxWidthScaling[j], "Right"))
                                    {
                                        PlayerClass[j].getDamage(damageDealt + NumberGenerator.Next(-3, 4), (knockbackAbility));
                                        playerLastHit[j] = i;
                                    }
                                }
                            }
                        }
                    }

                    //apply damage from special attacks
                    else if (attackType[i] == 2)
                    {
                        if (character[i] == "Shrek" || character[i] == "Metaknight" || character[i] == "Sonic")
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (j != i && PlayerPicBox[j].IsEmpty == false)
                                {
                                    if (PlayerPicBox[i].Intersects(PlayerPicBox[j]))
                                    {
                                        if (PlayerPicBox[i].Top < PlayerPicBox[j].Bottom && PlayerPicBox[i].Top > PlayerPicBox[j].Top)
                                        {
                                            if (direction[i] == "Right")
                                            {
                                                if (character[i] == "Shrek")
                                                {
                                                    PlayerClass[j].getDamage(15, 2);
                                                    playerLastHit[j] = i;
                                                }
                                                if (character[i] == "Metaknight")
                                                {
                                                    PlayerClass[j].getDamage(13, 2);
                                                    playerLastHit[j] = i;
                                                }
                                            }
                                            if (direction[i] == "Left")
                                            {
                                                if (character[i] == "Shrek")
                                                {
                                                    PlayerClass[j].getDamage(15, -2);
                                                    playerLastHit[j] = i;
                                                }
                                                if (character[i] == "Metaknight")
                                                {
                                                    PlayerClass[j].getDamage(13, -2);
                                                    playerLastHit[j] = i;
                                                }
                                            }
                                        }

                                        if (character[i] == "Sonic")
                                        {
                                            if (direction[i] == "Left" && (PlayerPicBox[i].Left > (PlayerPicBox[j].Left + picBoxWidthScaling[j]) && PlayerPicBox[i].Left < (PlayerPicBox[j].Right - picBoxWidthScaling[j]) || PlayerPicBox[i].Top < PlayerPicBox[j].Bottom && PlayerPicBox[i].Top > PlayerPicBox[j].Top))
                                            {
                                                PlayerClass[j].getDamage(14, 2);
                                                playerLastHit[j] = i;
                                            }
                                            else if (direction[i] == "Right" && (PlayerPicBox[i].Right > (PlayerPicBox[j].Left + picBoxWidthScaling[j]) && PlayerPicBox[i].Right < (PlayerPicBox[j].Right - picBoxWidthScaling[j]) || PlayerPicBox[i].Top < PlayerPicBox[j].Bottom && PlayerPicBox[i].Top > PlayerPicBox[j].Top))
                                            {
                                                PlayerClass[j].getDamage(14, -2);
                                                playerLastHit[j] = i;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (character[i] == "Shadow")
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (j != i && PlayerPicBox[j].IsEmpty == false)
                                {
                                    if (PlayerPicBox[i].Intersects(PlayerPicBox[j]))
                                    {
                                        PlayerClass[j].getDamage(2, 0);
                                        playerLastHit[j] = i;
                                    }
                                }
                            }
                        }
                    }

                    //check if Link's special attack hit anyone behind him
                    else if (attackType[i] == 4)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (j != i && PlayerPicBox[j].IsEmpty == false)
                            {
                                if (PlayerPicBox[i].Intersects(PlayerPicBox[j]))
                                {
                                    if ((direction[i] == "Left" && intersection(PlayerPicBox[i], PlayerPicBox[j], picBoxWidthScaling[j], "Right")))
                                    {
                                        PlayerClass[j].getDamage(9 + NumberGenerator.Next(-3, 4), 2);
                                        playerLastHit[j] = i;
                                    }

                                    else if ((direction[i] == "Right" && intersection(PlayerPicBox[i], PlayerPicBox[j], picBoxWidthScaling[j], "Left")))
                                    {
                                        PlayerClass[j].getDamage(9 + NumberGenerator.Next(-3, 4), -2);
                                        playerLastHit[j] = i;
                                    }
                                }
                            }
                        }
                    }

                    for (int j = 0; j < 4; j++)
                    {
                        if (j != i && ProjectPicBox[j].IsEmpty == false && ProjectPicBox[j].Intersects(PlayerPicBox[i]))
                        {
                            //check if the projectile has hit a character
                            if ((ProjectPicBox[j].Left < (PlayerPicBox[i].Right - picBoxWidthScaling[i]) && ProjectPicBox[j].Left > (PlayerPicBox[i].Left + picBoxWidthScaling[i])) || (ProjectPicBox[j].Right < (PlayerPicBox[i].Right - picBoxWidthScaling[i]) && ProjectPicBox[j].Right > (PlayerPicBox[i].Left + picBoxWidthScaling[i])) || (ProjectPicBox[j].Left < (PlayerPicBox[i].Left + picBoxWidthScaling[i]) && ProjectPicBox[j].Right > (PlayerPicBox[i].Right - picBoxWidthScaling[i])))
                            {
                                //projectile gets destroyed if it hits shadow's shield
                                if (character[i] == "Shadow" && (PlayerClass[i].attackType() == 2 || PlayerClass[i].attackType() == 3))
                                {
                                }
                                else
                                {
                                    //player takes damage from projectile
                                    if (character[j] == "Blastoise" && attackType[j] == 2)
                                    {
                                        PlayerClass[i].getDamage(6, 0);
                                    }
                                    else if (character[j] == "Pichu")
                                    {
                                        if (direction[i] == "Left")
                                        {
                                            PlayerClass[i].getDamage(2, 1);
                                        }
                                        else if (direction[i] == "Right")
                                        {
                                            PlayerClass[i].getDamage(2, -1);
                                        }
                                    }
                                    else if (character[j] != "Blastoise")
                                    {
                                        PlayerClass[i].getDamage(4, 0);
                                    }
                                    playerLastHit[i] = j;
                                }

                                //projectile gets destoryed after making contact
                                if (character[j] != "Blastoise")
                                {
                                    PlayerClass[j].destroyProjectileRec();
                                }
                            }
                        }
                    }
                }
            }
        }

        //damage waddles
        public void damageWaddle(GameTime gameTime)
        {
            for (int i = 0; i < waddle.Length; i++)
            {
                if (waddle[i] != null)
                {
                    waddle[i].Update(gameTime);

                    //send every players position to the waddle, so he can find the closest enemy
                    if (gameTime.TotalGameTime.Milliseconds % 1000 == 0)
                    {
                        waddle[i].getRec(PlayerPicBox[0], PlayerPicBox[1], PlayerPicBox[2], PlayerPicBox[3]);
                    }

                    for (int j = 0; j < 4; j++)
                    {
                        //check if anyone has hit a waddle with an attack
                        if (j != Math.Floor(i / 3.0) && PlayerPicBox[j].IsEmpty == false && PlayerPicBox[j].Intersects(waddle[i].setRec()) && (attackType[j] == 1 || (attackType[j] == 2 && (character[j] == "Knuckles" || character[j] == "Kirby"))))
                        {
                            if (intersection(PlayerPicBox[j], waddle[i].setRec(), 0, "Left"))//(PlayerPicBox[j].Left < waddle[i].setRec().Right && PlayerPicBox[j].Left > waddle[i].setRec().Left && direction[j] == "Left") 
                            {
                                waddle[i].getDamage(100, -10);
                            }
                            else if (intersection(PlayerPicBox[j], waddle[i].setRec(), 0, "Right"))//(PlayerPicBox[j].Right < waddle[i].setRec().Right && PlayerPicBox[j].Right > waddle[i].setRec().Left && direction[j] == "Right") 
                            {
                                waddle[i].getDamage(100, 10);
                            }
                        }

                        //check if anyone has hit a waddle with a projectile
                        else if (j != Math.Floor(i / 3.0) && ProjectPicBox[j].IsEmpty == false && ProjectPicBox[j].Intersects(waddle[i].setRec()))
                        {
                            if (intersection(ProjectPicBox[j], waddle[i].setRec(), 0, "Left")) //(ProjectPicBox[j].Left < waddle[i].setRec().Right && ProjectPicBox[j].Left > waddle[i].setRec().Left)
                            {
                                waddle[i].getDamage(100, -10);
                                PlayerClass[j].destroyProjectileRec();
                            }
                            else if (intersection(ProjectPicBox[j], waddle[i].setRec(), 0, "Right"))//(ProjectPicBox[j].Right < waddle[i].setRec().Right && ProjectPicBox[j].Right > waddle[i].setRec().Left)
                            {
                                waddle[i].getDamage(100, 10);
                                PlayerClass[j].destroyProjectileRec();
                            }
                        }

                        //check if a waddle is hitting anyone
                        else if (waddle[i].setRec().Intersects(PlayerPicBox[j]) && j != Math.Floor(i / 3.0) && gameTime.TotalGameTime.Milliseconds % 100 == 0)
                        {
                            if (intersection(waddle[i].setRec(), PlayerPicBox[j], picBoxWidthScaling[j], "Left"))//(waddle[i].setRec().Left < PlayerPicBox[j].Right && waddle[i].setRec().Left > PlayerPicBox[j].Left)
                            {
                                PlayerClass[j].getDamage(1, 0);
                            }
                            else if (intersection(waddle[i].setRec(), PlayerPicBox[j], picBoxWidthScaling[j], "Right"))//(waddle[i].setRec().Right < PlayerPicBox[j].Right && waddle[i].setRec().Right > PlayerPicBox[j].Left)
                            {
                                PlayerClass[j].getDamage(1, 0);
                            }
                        }
                    }

                    //check if a waddle has died
                    if (waddle[i].setDeath() == true)
                    {
                        waddle[i] = null;
                    }
                }
            }
        }

        //damage pichu
        public void damagePichu(GameTime gameTime)
        {
            for (int i = 0; i < 4; i++)
            {
                if (character[i] == "Pichu" && pichuBro[i] != null)
                {
                    if (PlayerClass[i].pichuAttack() == 1)
                    {
                        pichuBro[i].getAttack1();
                    }
                    else if (PlayerClass[i].pichuAttack() == 2)
                    {
                        pichuBro[i].getAttack2();
                    }
                    pichuBro[i].Update(gameTime);

                    //send every players position to pichu, so he can find the closest enemy
                    if (gameTime.TotalGameTime.Milliseconds % 10 == 0)
                    {
                        pichuBro[i].getRec(PlayerPicBox[0], PlayerPicBox[1], PlayerPicBox[2], PlayerPicBox[3]);
                    }

                    for (int j = 0; j < 4; j++)
                    {
                        //check if anyone has hit pichu with an attack
                        if (j != i && PlayerPicBox[j].IsEmpty == false && PlayerPicBox[j].Intersects(pichuBro[i].setRec()) && (attackType[j] == 1 || (attackType[j] == 2 && (character[j] == "Knuckles" || character[j] == "Kirby" || character[j] == "Seiar" || character[j] == "Link"))))
                        {
                            if (intersection(PlayerPicBox[j], pichuBro[i].setRec(), 0, "Left") && direction[j] == "Left")
                            {
                                pichuBro[i].getDamage(10, -2);
                            }
                            else if (intersection(PlayerPicBox[j], pichuBro[i].setRec(), 0, "Right") && direction[j] == "Right")
                            {
                                pichuBro[i].getDamage(10, 2);
                            }
                        }

                        //check if anyone has hit a pichuBro with a projectile
                        else if (j != i && ProjectPicBox[j].IsEmpty == false && ProjectPicBox[j].Intersects(pichuBro[i].setRec()))
                        {
                            if (intersection(ProjectPicBox[j], pichuBro[i].setRec(), 0, "Left"))
                            {
                                pichuBro[i].getDamage(6, 0);
                                PlayerClass[j].destroyProjectileRec();
                            }
                            else if (intersection(ProjectPicBox[j], pichuBro[i].setRec(), 0, "Right"))
                            {
                                pichuBro[i].getDamage(6, 0);
                                PlayerClass[j].destroyProjectileRec();
                            }
                        }

                        //check if pichuBro is hitting anyone
                        else if (pichuBro[i].setRec().Intersects(PlayerPicBox[j]) && j != i)
                        {
                            if (intersection(pichuBro[i].setRec(), PlayerPicBox[j], picBoxWidthScaling[j], "Left"))
                            {
                                if (pichuBro[i].attackType() > 0)
                                {
                                    PlayerClass[j].getDamage(5, -1);
                                }
                                else
                                {
                                    pichuBro[i].getAttack1();
                                }
                            }
                            else if (intersection(pichuBro[i].setRec(), PlayerPicBox[j], picBoxWidthScaling[j], "Right"))
                            {
                                if (pichuBro[i].attackType() > 0)
                                {
                                    PlayerClass[j].getDamage(5, 1);
                                }
                                else
                                {
                                    pichuBro[i].getAttack1();
                                }
                            }

                        }

                        if (pichuBro[i].checkProjectile())
                        {
                            //check if pichu's lightning is hitting anyone
                            if (intersection(pichuBro[i].setProjectileRec(), PlayerPicBox[j], picBoxWidthScaling[j], "Left") && i != j)
                            {
                                PlayerClass[j].getDamage(1, 0);
                            }
                            else if (intersection(pichuBro[i].setProjectileRec(), PlayerPicBox[j], picBoxWidthScaling[j], "Right") && i != j)
                            {
                                PlayerClass[j].getDamage(1, 0);
                            }
                        }
                    }

                    //check if pichu has died
                    if (pichuBro[i].setDeath() == true)
                    {
                        pichuBro[i] = null;
                    }
                }
            }
        }

        //check if two rectanges intersect
        public bool intersection(Rectangle rec1, Rectangle rec2, int hitboxCorrection, string direction)
        {
            if (rec1.Intersects(rec2))
            {
                if (direction == "Left")
                {
                    if ((rec1.Left > rec2.Left + hitboxCorrection && rec1.Left < rec2.Right - hitboxCorrection) || (rec1.Left + (rec1.Width / 5) > rec2.Left + hitboxCorrection && rec1.Left + (rec1.Width / 5) < rec2.Right - hitboxCorrection))
                    {
                        return true;
                    }
                }
                if (direction == "Right")
                {
                    if ((rec1.Right > rec2.Left + hitboxCorrection && rec1.Right < rec2.Right - hitboxCorrection) || (rec1.Right - (rec1.Width / 5) > rec2.Left + hitboxCorrection && rec1.Right - (rec1.Width / 5) < rec2.Right - hitboxCorrection))
                    {
                        return true;
                    }
                }
            }
            return false;

        }

        //control the bots
        private void controlBot()
        {
            int dx = 0;
            bool jump = false;
            bool attack = false;
            string direction = "Left";

            //make the bot run towards you
            if (((PlayerPicBox[1].Left + (PlayerPicBox[1].Width / 2)) >= PlayerPicBox[0].Right) || (PlayerPicBox[1].Left + (PlayerPicBox[1].Width / 2) >= finalDestinationRec.Right))
            {
                dx = -3;
                direction = "Left";
            }
            else if (((PlayerPicBox[1].Right - (PlayerPicBox[1].Width / 2)) <= PlayerPicBox[0].Left) || (PlayerPicBox[1].Right - (PlayerPicBox[1].Width / 2) <= finalDestinationRec.Left))
            {
                dx = 3;
                direction = "Right";
            }

            //make the bot attack
            if (PlayerPicBox[1].Left > PlayerPicBox[0].Left && PlayerPicBox[1].Left < PlayerPicBox[0].Right)
            {
                direction = "Left";
                attack = true;
            }
            else if (PlayerPicBox[1].Right > PlayerPicBox[0].Left && PlayerPicBox[1].Right < PlayerPicBox[0].Right)
            {
                direction = "Right";
                attack = true;
            }

            //make the bot jump
            if ((PlayerPicBox[1].Top > finalDestinationRec.Top) || (PlayerPicBox[1].Top - 50 > PlayerPicBox[0].Bottom))
            {
                jump = true;
            }

            //send the bots input to the player class
            PlayerClass[1].getInputBots(dx, jump, direction, attack);
        }

        //keep scores
        private void lives()
        {
            //if someone gets a kill, they get two points, and the player who died loses a point
            for (int i = 0; i < 4; i++)
            {
                if (PlayerClass[i] != null)
                {
                    if (PlayerClass[i].setDeath() == true)
                    {
                        score[playerLastHit[i]] += 2;
                        score[i]--;

                        if (character[i] == "Pichu")
                        {
                            //if (pichuBro[i] != null)
                            {
                                pichuBro[i] = null;
                            }

                            pichuBro[i] = new Enemy(new Rectangle(PlayerPicBox[i].X - 50, PlayerPicBox[i].Y, PlayerPicBox[i].Width, PlayerPicBox[i].Height), null, "Left", i, character[i], this.content, displayWidth, displayHeight, finalDestinationRec, stageHeightAdjustment, true);
                        }
                    }
                }
            }
        }
    }
}
