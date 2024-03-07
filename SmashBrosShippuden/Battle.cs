using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace SmashBrosShippuden
{
    internal class Battle : BaseModel
    {
        //Displaying the characters
        Rectangle[] PlayerPicBox = new Rectangle[4];
        Character[] PlayerClass = new Character[4];
        string[] character = new string[4];
        int[] playerLastHit = new int[4];
        int[] score = new int[4];
        Boolean player2Bot;

        //background images
        Texture2D finalDestination;
        Texture2D background;
        Rectangle finalDestinationRec;
        Rectangle backgroundRec;

        int stageIndex;
        int stageHeightAdjustment = 0;

        string[] direction = new string[4] { "Right", "Right", "Left", "Left" };

        Song[] backgroundMusic = new Song[3];
        int song = 0;
        int displayWidth;
        int displayHeight;
        Random NumberGenerator = new Random();

        List<Projectiles> projectiles = new List<Projectiles>();
        List<Enemy> companions = new List<Enemy>();

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
                backgroundMusic[1] = Content.Load<Song>("musicsonic2");
                backgroundMusic[2] = Content.Load<Song>("musicsonic3");
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

            this.content = Content;

            // TODO: move this to initialize, remove Content argument from Character class
            for (int k = 0; k < 4; k++)
            {
                if (character[k] != null)
                {
                    PlayerClass[k] = new Character(200 * (k + 1), finalDestinationRec.Top, direction[k], k, character[k], displayWidth, displayHeight, finalDestinationRec, stageHeightAdjustment, false);
                    PlayerClass[k].LoadContent(content);

                    if (character[k] == "Pichu")
                    {
                        Enemy companion = new Enemy(PlayerPicBox[k].X - 50, PlayerPicBox[k].Y, "Left", k, character[k], displayWidth, displayHeight, finalDestinationRec, stageHeightAdjustment, true);
                        companion.LoadContent(this.content);
                        this.companions.Add(companion);
                    }
                }
            }

            if (character[0] != null && character[1] == null && character[2] == null && character[3] == null)
            {
                player2Bot = true;
                character[1] = "Shadow";
                // character[2] = "Kirby";
                // character[3] = "Sasuke";

                for (int i = 1; i < 2; i++)
                {
                    PlayerClass[i] = new Character(200 * (i + 1), finalDestinationRec.Top, direction[i], i, character[i], displayWidth, displayHeight, finalDestinationRec, stageHeightAdjustment, true);
                    PlayerClass[i].LoadContent(this.content);
                }
            }

            this.song = NumberGenerator.Next(0, backgroundMusic.Length);
        }

        public override void Update(GamePadState[] gamePad, GameTime gameTime)
        {
            base.Update(gamePad, gameTime);

            for (int i = 0; i < 4; i++)
            {
                if (character[i] != null)
                {
                    if (i >= 1 && this.player2Bot)
                    {
                        // this.controlBot();
                    }
                    else
                    {
                        PlayerClass[i].getInput(gamePad[i]);
                    }

                    //update the characters locations
                    PlayerClass[i].Update(gameTime);
                    PlayerPicBox[i] = PlayerClass[i].getRectangle();
                    direction[i] = PlayerClass[i].directionPlayer();
                    damage(PlayerClass[i]);

                    //update the projectiles location
                    if (PlayerClass[i].createProjectile())
                    {
                        this.createProjectile(PlayerClass[i], i);
                    }
                    else if (PlayerClass[i].createCompanion())
                    {
                        Enemy companion = new Enemy(PlayerPicBox[i].X + (PlayerPicBox[i].Width / 2), PlayerPicBox[i].Y + (PlayerPicBox[i].Height / 2), "Left", i, "waddle", displayWidth, displayHeight, finalDestinationRec, stageHeightAdjustment, true);
                        companion.LoadContent(this.content);
                        this.companions.Add(companion);
                    }
                }
            }

            for (int i = 0; i < this.companions.Count; i++)
            {
                Enemy companion = this.companions[i];
                companion.getRec(this.PlayerPicBox);
                companion.Update(gameTime);
                damage(companion);

                // delete any projectiles that have moved offscreen
                if (this.isOffscreen(companion))
                {
                    this.companions.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < this.projectiles.Count; i++)
            {
                Projectiles p = this.projectiles[i];
                p.Update(gameTime);

                // delete any projectiles that have moved offscreen
                if (this.isOffscreen(p))
                {
                    this.projectiles.RemoveAt(i);
                    i--;
                }
            }

            lives();

            for (int i = 0; i < this.PlayerClass.Length; i++)
            {
                if (this.PlayerClass[i] != null && this.isOffscreen(this.PlayerClass[i]))
                {
                    this.PlayerClass[i] = new Character(200 * (i + 1), finalDestinationRec.Top, "Right", i, character[i], displayWidth, displayHeight, finalDestinationRec, stageHeightAdjustment, false);
                    this.PlayerClass[i].LoadContent(this.content);
                }
            }

            if (MediaPlayer.State != MediaState.Playing && backgroundMusic.Length > 0)
            {
                song += 1;

                if (song >= backgroundMusic.Length)
                {
                    song = 0;
                }

                MediaPlayer.Play(backgroundMusic[song]);
            }

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
            foreach (Character c in this.PlayerClass)
            {
                if (c != null)
                {
                    c.Draw(_spriteBatch);

                    // health
                    _spriteBatch.Draw(c.seriesSymbol, c.seriesSymbolRec, Color.White);
                    _spriteBatch.DrawString(font1, c.damageTaken.ToString() + "%", new Vector2(205 + (c.player * 200), 5 + (displayHeight * 4) / 5), Color.Black);
                    _spriteBatch.DrawString(font1, c.damageTaken.ToString() + "%", new Vector2(200 + (c.player * 200), (displayHeight * 4) / 5), Color.White);
                    _spriteBatch.Draw(c.LivesIcon, c.LivesIconRec, Color.White);
                }
            }

            foreach (Character c in this.companions)
            {
                if (c != null)
                {
                    c.Draw(_spriteBatch);
                }
            }

            foreach (Projectiles p in this.projectiles)
            {
                p.Draw(_spriteBatch);
            }

            //foreach (Rectangle r in this.PlayerPicBox)
            //{
            //    _spriteBatch.Draw(finalDestination, r, Color.Red);
            //}
        }

        public void damage(Character playerClass)
        {
            if (playerClass != null)
            {
                Rectangle hitbox = playerClass.getRectangle();
                Attack attack = playerClass.attack;
                int i = playerClass.player;
                List<Character> characters = new List<Character>();
                characters.AddRange(this.companions);
                characters.AddRange(this.PlayerClass);

                //deal different amounts of damage based on who the character is
                if (playerClass.isAttackFrame() || (playerClass.character == "waddle" && playerClass.isNewFrame()))
                {
                    Rectangle attackHitbox = playerClass.character == "waddle" ? playerClass.getRectangle() : playerClass.getAttackHitboxRectangle();

                    //check if the players attacks hit anyone
                    foreach (Character target in characters)
                    {
                        if (target != null && target.player != i && !target.shieldBlock())
                        {
                            if (attackHitbox.Intersects(target.getRectangle()))
                            {
                                int damage = playerClass.character == "waddle" ? 1 : attack.damage + NumberGenerator.Next(0, 2);
                                int knockup = playerClass.character == "waddle" ? 0 : attack.knockup;
                                int knockback;
                                if (playerClass.character == "waddle")
                                {
                                    if (playerClass.x > target.x)
                                    {
                                        knockback = -1;
                                    }
                                    else
                                    {
                                        knockback = 1;
                                    }
                                }
                                else
                                {
                                    knockback = playerClass.getKnockback();
                                }
                                target.getDamage(damage, knockback, knockup);
                                playerLastHit[target.player] = i;
                            }
                        }
                    }
                }

                // check if any players were hit by projectiles
                for (int j = 0; j < this.projectiles.Count; j++)
                {
                    if (this.projectiles[j].player != i && this.projectiles[j].getRectangle().Intersects(hitbox))
                    {
                        //check if the projectile has hit a character
                        if (intersection(this.projectiles[j].getRectangle(), hitbox, "Left") || intersection(this.projectiles[j].getRectangle(), hitbox, "Right"))
                        {
                            //projectile gets destroyed if it hits shadow's shield
                            if (PlayerClass[i].shieldBlock())
                            {
                                this.projectiles.RemoveAt(j);
                            }
                            else
                            {
                                //player takes damage from projectile
                                if (this.projectiles[j].direction == "Left")
                                {
                                    playerClass.getDamage(this.projectiles[j].damage, -1 * this.projectiles[j].knockback, 1);
                                }
                                else
                                {
                                    playerClass.getDamage(this.projectiles[j].damage, this.projectiles[j].knockback, 1);
                                }

                                playerLastHit[i] = this.projectiles[j].player;

                                //projectile gets destoryed after making contact
                                if (character[this.projectiles[j].player] == "Blastoise" || (character[this.projectiles[j].player] == "Sasuke" && this.projectiles[j].attackType == AttackType.Special))
                                {
                                    // persistent projectiles are not destroyed
                                }
                                else
                                {
                                    this.projectiles.RemoveAt(j);
                                }
                            }
                        }
                    }
                }
            }
        }

        //check if two rectanges intersect
        public bool intersection(Rectangle rec1, Rectangle rec2, string direction)
        {
            if (rec1.Intersects(rec2))
            {
                if (direction == "Left")
                {
                    if ((rec1.Left > rec2.Left && rec1.Left < rec2.Right) || (rec1.Left + (rec1.Width / 5) > rec2.Left && rec1.Left + (rec1.Width / 5) < rec2.Right))
                    {
                        return true;
                    }
                }
                if (direction == "Right")
                {
                    if ((rec1.Right > rec2.Left && rec1.Right < rec2.Right) || (rec1.Right - (rec1.Width / 5) > rec2.Left && rec1.Right - (rec1.Width / 5) < rec2.Right))
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

                        for (int j = 0; j < this.companions.Count; j++)
                        {
                            // delete any companions spawn the player that died
                            if (this.companions[j].player == i)
                            {
                                this.companions.RemoveAt(j);

                                if (character[i] == "Pichu")
                                {
                                    Enemy companion = new Enemy(PlayerPicBox[i].X - 50, PlayerPicBox[i].Y, "Left", i, character[i], displayWidth, displayHeight, finalDestinationRec, stageHeightAdjustment, true);
                                    companion.LoadContent(this.content);
                                    this.companions.Insert(0, companion);
                                }
                            }

                        }
                    }
                }
            }
        }

        private void createProjectile(Character character, int player)
        {
            Rectangle characterRec = character.getRectangle();
            Rectangle projectileRec;

            //Mewtwo fires a projectile
            if (character.character == "Mewtwo")
            {
                if (character.direction == "Left")
                {
                    projectileRec = new Rectangle(characterRec.X - (characterRec.Width / 4), characterRec.Y + (characterRec.Height / 4), 10, 10);
                }
                else
                {
                    projectileRec = new Rectangle(characterRec.X + (characterRec.Width / 4), characterRec.Y + (characterRec.Height / 4), 10, 10);
                }
            }

            //Charizard fires a projectile
            else if (character.character == "Charizard")
            {
                if (character.direction == "Left")
                {
                    projectileRec = new Rectangle(characterRec.X - (characterRec.Width / 4), characterRec.Y + (characterRec.Height / 4), 10, 10);
                }
                else
                {
                    projectileRec = new Rectangle(characterRec.X + (characterRec.Width / 4), characterRec.Y + (characterRec.Height / 4), 10, 10);
                }
            }

            //Mario and Luigi fire projectiles
            else if (character.character == "Mario" || character.character == "Luigi")
            {
                if (character.direction == "Left")
                {
                    projectileRec = new Rectangle(characterRec.X + (characterRec.Width / 4), characterRec.Y + (characterRec.Height / 2), 10, 10);
                }
                else
                {
                    projectileRec = new Rectangle(characterRec.Right - (characterRec.Width / 4), characterRec.Y + (characterRec.Height / 2), 10, 10);
                }
            }

            else if (character.character == "Pichu")
            {
                projectileRec = new Rectangle(characterRec.X + (characterRec.Width / 4), characterRec.Y + (characterRec.Height / 4), 10, 10);
            }

            //blastoise fires a projectile
            else if (character.character == "Blastoise")
            {
                if (character.direction == "Left")
                {
                    projectileRec = new Rectangle(characterRec.Left + (characterRec.Width / 4) - (characterRec.Width), characterRec.Y, characterRec.Width, characterRec.Height);
                }
                else
                {
                    projectileRec = new Rectangle(characterRec.Right - (characterRec.Width / 4), characterRec.Y, characterRec.Width, characterRec.Height);
                }
            }

            else if (character.character == "Sasuke" && character.attack.attackType == AttackType.Special)
            {
                if (character.direction == "Left")
                {
                    projectileRec = new Rectangle(characterRec.Left - 3 * characterRec.Width, characterRec.Y + (characterRec.Height * 5) / 4, characterRec.Width, characterRec.Height);
                }
                else
                {
                    projectileRec = new Rectangle(characterRec.Right + 3 * characterRec.Width, characterRec.Y + (characterRec.Height * 5) / 4, characterRec.Width, characterRec.Height);
                }
            }

            else if (character.character == "Sasuke" && character.attack.attackType == AttackType.SideSpecial)
            {
                if (character.direction == "Left")
                {
                    projectileRec = new Rectangle(characterRec.Left - characterRec.Width, characterRec.Y + characterRec.Height / 2, characterRec.Width, characterRec.Height);
                }
                else
                {
                    projectileRec = new Rectangle(characterRec.Right + characterRec.Width, characterRec.Y + characterRec.Height / 2, characterRec.Width, characterRec.Height);
                }
            }

            else if (character.character == "Knuckles")
            {
                if (character.direction == "Left")
                {
                    projectileRec = new Rectangle(characterRec.Left - characterRec.Width, characterRec.Y + characterRec.Height / 2, characterRec.Width, characterRec.Height);
                }
                else
                {
                    projectileRec = new Rectangle(characterRec.Right + characterRec.Width, characterRec.Y + characterRec.Height / 2, characterRec.Width, characterRec.Height);
                }
            }

            else if (character.character == "Metaknight")
            {
                if (character.direction == "Left")
                {
                    projectileRec = new Rectangle(characterRec.Left - characterRec.Width, characterRec.Y + characterRec.Height, characterRec.Width, characterRec.Height);
                }
                else
                {
                    projectileRec = new Rectangle(characterRec.Right + characterRec.Width, characterRec.Y + characterRec.Height, characterRec.Width, characterRec.Height);
                }
            }

            else
            {
                return;
            }

            Projectiles p = new Projectiles(projectileRec.X, projectileRec.Y, character.direction, player, character.character, character.attack.attackType);
            p.LoadContent(this.content);
            this.projectiles.Add(p);
        }

        private bool isOffscreen(Sprite sprite)
        {
            Rectangle rectangle = sprite.getRectangle();
            if (rectangle.Right < 0 || rectangle.Left > displayWidth || rectangle.Bottom < -100 || rectangle.Top > displayHeight)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
