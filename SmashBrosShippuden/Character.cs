//Jake Loftus
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SmashBrosShippuden
{
    class Character : Sprite
    {
        //a ton of variables that make the game work
        protected string direction;
        protected string character;
        protected int player;
        protected int knockback;
        protected int knockup;
        protected int counter;
        protected int counterSprite;
        protected int counterSprite2;
        protected int counterSpriteModifier;
        protected int counterSpriteModRun;
        protected int counterSpriteAttack;
        protected int counterTaunt;
        protected int dx;
        protected int dy;
        protected int spriteRunLength;
        protected int spriteAttack1Length;
        protected int spriteAttack2Length;
        protected int jumpHeight;
        protected int damageTaken;
        protected int picboxHeightModifier;
        protected bool attack;
        protected bool attack2;
        protected bool jump;
        protected bool taunt;
        protected bool attackFrame;
        protected bool isBot;
        protected bool isDead;
        protected float aspectRatio;
        protected float widthScaling;
        protected int displayWidth;
        protected int displayHeight;
        protected int stageHeightAdjustment;
        protected Random NumberGenerator = new Random();

        //all the sprites
        Texture2D[] spriteRunLeft;
        Texture2D[] spriteRunRight;
        Texture2D[] spriteAttackLeft;
        Texture2D[] spriteAttackRight;
        Texture2D[] spriteSmashLeft;
        Texture2D[] spriteSmashRight;
        Texture2D[] spriteJump = new Texture2D[2];
        Texture2D[] spriteHurt = new Texture2D[2];
        Texture2D[] luigiTaunt = new Texture2D[12];
        Texture2D LivesIcon;
        Texture2D seriesSymbol;
        Texture2D BlastoiseWaterLeft;
        Texture2D pichuCloud;

        //sound effects
        SoundEffect[] spriteSounds = new SoundEffect[4];
        SoundEffect[] spriteSounds2 = new SoundEffect[3];
        SoundEffect goWeegee;

        Rectangle seriesSymbolRec;
        Rectangle LivesIconRec;
        Rectangle projectileRec;
        protected Rectangle finalDestinationRec = new Rectangle();
        protected Projectiles PlayerProjectiles;

        GamePadState pad1;

        SpriteFont font1;

        protected ContentManager content;

        public Character(Rectangle newRectangle, Texture2D newTexture, string newDirection, int newPlayer, string newCharacter, ContentManager cnt, int disWidth, int disHeight, Rectangle stage, int stageHeight, bool bot)
            : base(newTexture, newRectangle)
        {
            //rectangle = newRectangle;
            direction = newDirection;
            player = newPlayer + 1;
            character = newCharacter;
            content = cnt;
            displayWidth = disWidth;
            displayHeight = disHeight;
            finalDestinationRec = stage;
            stageHeightAdjustment = stageHeight;
            isBot = bot;

            Initialize();
        }

        public void Update(GameTime gameTime)
        {
            isDead = false;
            attackFrame = false;
            counter++;
            deathMethod();
            getInput();
            displaySprite();
            gravity();
            knockbackMethod();
            rectangle.X += dx;
            rectangle.Y += dy;
            dx = 0;
            dy = 0;
            if (PlayerProjectiles != null)
            {
                PlayerProjectiles.Update(gameTime);

                if (character == "Blastoise" && pad1.Buttons.X == ButtonState.Released)
                {
                    destroyProjectileRec();
                }
            }

            if (counter % 3 == 0)
            {
                counterTaunt++;
            }
        }

        public void UnloadContent()
        {
            for (int i = 0; i < spriteSounds.Length; i++)
            {
                spriteSounds[i].Dispose();
            }
        }

        private void Initialize()
        {
            //CAUTION: magic numbers
            if (character == "Mario")
            {
                spriteAttack1Length = 8;
                spriteAttack2Length = 5;
                spriteRunLength = 8;
                counterSpriteModifier = 5;
                counterSpriteModRun = 8;
                counterSpriteAttack = 5;
                widthScaling = 2;
            }

            if (character == "Luigi")
            {
                spriteAttack1Length = 7;
                spriteAttack2Length = 5;
                spriteRunLength = 8;
                counterSpriteModifier = 5;
                counterSpriteModRun = 8;
                counterSpriteAttack = 5;
                widthScaling = 2.2f;
            }

            if (character == "Pichu")
            {
                spriteAttack1Length = 5;
                spriteAttack2Length = 5;
                spriteRunLength = 4;
                counterSpriteModifier = 8;
                counterSpriteModRun = 8;
                counterSpriteAttack = 2;
                widthScaling = 1.5f;
            }

            if (character == "Mewtwo")
            {
                spriteAttack1Length = 4;
                spriteAttack2Length = 4;
                spriteRunLength = 2;
                counterSpriteModifier = 10;
                counterSpriteModRun = 8;
                counterSpriteAttack = 3;
                widthScaling = 1.7f;
            }

            if (character == "Charizard")
            {
                spriteAttack1Length = 4;
                spriteAttack2Length = 4;
                spriteRunLength = 4;
                counterSpriteModifier = 8;
                counterSpriteModRun = 10;
                counterSpriteAttack = 3;
                widthScaling = 2;
            }

            if (character == "Shadow")
            {
                spriteAttack1Length = 9;
                spriteAttack2Length = 17;
                spriteRunLength = 9;
                counterSpriteModifier = 5;
                counterSpriteModRun = 6;
                counterSpriteAttack = 3;
                widthScaling = 2.2f;
            }

            if (character == "Knuckles")
            {
                spriteAttack1Length = 6;
                spriteAttack2Length = 5;
                spriteRunLength = 8;
                counterSpriteModifier = 6;
                counterSpriteModRun = 5;
                counterSpriteAttack = 3;
                widthScaling = 1.5f;
            }

            if (character == "Sonic")
            {
                spriteAttack1Length = 5;
                spriteAttack2Length = 9;
                spriteRunLength = 8;
                counterSpriteModifier = 5;
                counterSpriteModRun = 6;
                counterSpriteAttack = 3;
                widthScaling = 1.7f;
            }

            if (character == "Link")
            {
                spriteAttack1Length = 7;
                spriteAttack2Length = 8;
                spriteRunLength = 6;
                counterSpriteModifier = 4;
                counterSpriteModRun = 6;
                counterSpriteAttack = 4;
                widthScaling = 2.3f;
            }

            if (character == "Shrek")
            {
                spriteAttack1Length = 6;
                spriteAttack2Length = 8;
                spriteRunLength = 8;
                counterSpriteModifier = 6;
                counterSpriteModRun = 6;
                counterSpriteAttack = 3;
                widthScaling = 2;
            }

            if (character == "Blastoise")
            {
                spriteAttack1Length = 4;
                spriteAttack2Length = 3;
                spriteRunLength = 4;
                counterSpriteModifier = 6;
                counterSpriteModRun = 8;
                counterSpriteAttack = 1;
                widthScaling = 1.4f;
            }

            if (character == "Metaknight")
            {
                spriteAttack1Length = 9;
                spriteAttack2Length = 8;
                spriteRunLength = 8;
                counterSpriteModifier = 4;
                counterSpriteModRun = 4;
                counterSpriteAttack = 3;
                widthScaling = 4f;
            }

            if (character == "Kirby")
            {
                spriteAttack1Length = 4;
                spriteAttack2Length = 5;
                spriteRunLength = 10;
                counterSpriteModifier = 6;
                counterSpriteModRun = 5;
                counterSpriteAttack = 3;
                widthScaling = 2.2f;
            }

            if (character == "King")
            {
                spriteAttack1Length = 6;
                spriteAttack2Length = 5;
                spriteRunLength = 4;
                counterSpriteModifier = 7;
                counterSpriteModRun = 8;
                counterSpriteAttack = 5;
                widthScaling = 3;
            }

            if (character == "waddle")
            {
                widthScaling = 0.9f;
                spriteRunLength = 8;
                counterSpriteModifier = 6;
                counterSpriteModRun = 6;
            }

            spriteRunLeft = new Texture2D[spriteRunLength];
            spriteRunRight = new Texture2D[spriteRunLength];
            spriteSmashLeft = new Texture2D[spriteAttack1Length];
            spriteSmashRight = new Texture2D[spriteAttack1Length];
            spriteAttackLeft = new Texture2D[spriteAttack2Length];
            spriteAttackRight = new Texture2D[spriteAttack2Length];

            //load the characters running and attact sprites
            for (int i = 0; i < spriteRunLength; i++)
            {
                spriteRunLeft[i] = content.Load<Texture2D>(character + "/" + character.ToLower() + "RunLeft" + (i + 1));
                spriteRunRight[i] = content.Load<Texture2D>(character + "/" + character.ToLower() + "RunRight" + (i + 1));
            }

            for (int i = 0; i < spriteAttack1Length; i++)
            {
                spriteSmashLeft[i] = content.Load<Texture2D>(character + "/" + character.ToLower() + "SmashLeft" + (i + 1));
                spriteSmashRight[i] = content.Load<Texture2D>(character + "/" + character.ToLower() + "SmashRight" + (i + 1));
            }

            for (int i = 0; i < spriteAttack2Length; i++)
            {
                spriteAttackLeft[i] = content.Load<Texture2D>(character + "/" + character.ToLower() + "AttackLeft" + (i + 1));
                spriteAttackRight[i] = content.Load<Texture2D>(character + "/" + character.ToLower() + "AttackRight" + (i + 1));
            }

            //jumping sprites
            spriteJump[0] = content.Load<Texture2D>(character + "/" + character.ToLower() + "JumpLeft");
            spriteJump[1] = content.Load<Texture2D>(character + "/" + character.ToLower() + "JumpRight");

            //hurt sprites
            spriteHurt[0] = content.Load<Texture2D>(character + "/" + character.ToLower() + "HurtLeft");
            spriteHurt[1] = content.Load<Texture2D>(character + "/" + character.ToLower() + "HurtRight");

            //character icons and symbol 
            if (character != "waddle")
            {
                LivesIcon = content.Load<Texture2D>("LivesIcon/" + character + "Lives");
                LivesIconRec = new Rectangle(200 + (player * 200), ((displayHeight * 4) / 5) - 25, LivesIcon.Width, LivesIcon.Height);

                seriesSymbol = content.Load<Texture2D>("Symbol/" + character + "Symbol");
                seriesSymbolRec = new Rectangle(175 + (player * 200), ((displayHeight * 4) / 5) - 25, (seriesSymbol.Width * 3) / 4, (seriesSymbol.Height * 3) / 4);

                //for (int i = 0; i < spriteSounds.Length; i++)
                //{
                //    spriteSounds[i] = content.Load<SoundEffect>(character + "/" + character.ToLower() + "00" + (i + 1));
                //}

                //for (int i = 0; i < spriteSounds2.Length; i++)
                //{
                //    if (character == "Link" || character == "Pichu")
                //    {
                //        spriteSounds2[i] = content.Load<SoundEffect>(character + "/" + character.ToLower() + "10" + (i + 1));
                //    }
                //}
            }

            BlastoiseWaterLeft = content.Load<Texture2D>("Blastoise/blastoiseWaterLeft1");
            pichuCloud = content.Load<Texture2D>("Pichu/cloud1");


            for (int i = 0; i < luigiTaunt.Length; i++)
            {
                luigiTaunt[i] = content.Load<Texture2D>("Luigi/luigiTaunt" + (i + 1));
            }

            goWeegee = content.Load<SoundEffect>("Luigi/luigi021");

            aspectRatio = (float)spriteRunLeft[0].Height / spriteRunLeft[0].Width;

            font1 = content.Load<SpriteFont>("SpriteFont1");

            deathMethod();
        }

        //display the sprite for each characters animations
        private void displaySprite()
        {
            //scroll through animations
            if (counter % counterSpriteModifier == 0)
            {
                counterSprite++;
            }

            if (counter % counterSpriteModRun == 0)
            {
                counterSprite2++;
            }

            //display the damaged animation when they take tons of damage
            if (knockback > 4)
            {
                texture = spriteHurt[1];
                attack = false;
                attack2 = false;
                jump = false;

                if (PlayerProjectiles != null)
                {
                    destroyProjectileRec();
                }
            }

            else if (knockback < -4)
            {
                texture = spriteHurt[0];
                attack = false;
                attack2 = false;
                jump = false;

                if (PlayerProjectiles != null)
                {
                    destroyProjectileRec();
                }
            }

            //make the character do their special attack
            else if (attack2 == true)
            {
                specialAttack();
                if (character != "Knuckles" && character != "Pichu")
                {
                    dx = 0;
                }

                if (counterSprite < spriteAttack2Length)
                {
                    if (direction == "Left")
                    {
                        texture = spriteAttackLeft[counterSprite];
                    }
                    if (direction == "Right")
                    {
                        texture = spriteAttackRight[counterSprite];
                    }

                    if (counterSprite == 1 && counter % counterSpriteModifier == 1 && (character == "Link" || character == "Pichu"))
                    {
                        spriteSounds2[NumberGenerator.Next(0, 3)].Play();
                    }
                }

                else
                {
                    attack2 = false;
                }
            }

            //Display the attack animation when a player attacks
            else if (attack == true)
            {
                if (counterSprite < spriteAttack1Length)
                {
                    if (direction == "Left")
                    {
                        texture = spriteSmashLeft[counterSprite];
                    }
                    if (direction == "Right")
                    {
                        texture = spriteSmashRight[counterSprite];
                    }
                    if (counterSprite == counterSpriteAttack && counter % counterSpriteModifier == 0)
                    {
                        attackFrame = true;
                    }
                    //if (counterSprite == 2 && counter % counterSpriteModifier == 1 && (character != "Shrek" && character != "King") || (counterSprite == 5 && counter % counterSpriteModifier == 1 && character == "King"))
                    //{
                    //    spriteSounds[NumberGenerator.Next(0, 4)].Play();
                    //}
                }
                else
                {
                    attack = false;
                }
            }

            //display jumping sprite
            else if (jump == true && direction == "Left")
            {
                texture = spriteJump[0];
            }
            else if (jump == true && direction == "Right")
            {
                texture = spriteJump[1];
            }

            else if (taunt == true)
            {
                texture = luigiTaunt[counterTaunt % luigiTaunt.Length];

                //if (counter % 50 == 0)
                //{
                //    goWeegee.Play();
                //}
            }

            //If the player isnt doing anything, display running animation
            else
            {
                if (direction == "Left")
                {
                    texture = spriteRunLeft[counterSprite2 % spriteRunLength];
                }
                if (direction == "Right")
                {
                    texture = spriteRunRight[counterSprite2 % spriteRunLength];
                }
            }
        }

        //getting input
        private void getInput()
        {
            if (player == 1)
            {
                pad1 = GamePad.GetState(PlayerIndex.One);
            }
            if (player == 2)
            {
                pad1 = GamePad.GetState(PlayerIndex.Two);
            }
            if (player == 3)
            {
                pad1 = GamePad.GetState(PlayerIndex.Three);
            }
            if (player == 4)
            {
                pad1 = GamePad.GetState(PlayerIndex.Four);
            }

            if (pad1.ThumbSticks.Left.X < -0.1)
            {
                if ((attack == true || attack2 == true) && direction == "Right")
                {

                }
                else
                {
                    direction = "Left";
                    dx = -3;
                    taunt = false;
                }
            }
            if (pad1.ThumbSticks.Left.X > 0.1)
            {
                if ((attack == true || attack2 == true) && direction == "Left")
                {

                }
                else
                {
                    direction = "Right";
                    dx = 3;
                    taunt = false;
                }
            }

            if ((pad1.Buttons.B == ButtonState.Pressed || pad1.Buttons.Y == ButtonState.Pressed) && jump == false && attack == false && attack2 == false)
            {
                jump = true;
                jumpHeight = 9;
                counterSprite = 0;
                taunt = false;
            }

            if (pad1.Buttons.X == ButtonState.Pressed && attack == false && attack2 == false)
            {
                attack2 = true;
                counterSprite = 0;
                taunt = false;
            }

            if (pad1.Buttons.A == ButtonState.Pressed && attack == false && attack2 == false)
            {
                attack = true;
                counterSprite = 0;
                taunt = false;
            }

            if (pad1.DPad.Up == ButtonState.Pressed && character == "Luigi")
            {
                taunt = true;
            }
        }

        //control bots
        public void getInputBots(int newDX, bool jumping, string newDirection, bool attacking)
        {
            if (dx == 0 && newDX != 0)
            {
                taunt = false;
            }

            dx = newDX;

            if (jump == false && jumping == true)
            {
                jump = true;
                jumpHeight = 9;
                counterSprite = 0;
                taunt = false;
            }

            direction = newDirection;

            if (attack == false && attacking == true)
            {
                attack = true;
                counterSprite = 0;
                taunt = false;
            }
        }

        //the physics engine 
        private void gravity()
        {
            //make characters fall if they are not on a platform
            if ((rectangle.Bottom - picboxHeightModifier >= finalDestinationRec.Top + stageHeightAdjustment && rectangle.Bottom - picboxHeightModifier <= finalDestinationRec.Top + stageHeightAdjustment + 15 && rectangle.Left + (rectangle.Width / 2) >= finalDestinationRec.Left && rectangle.Right - (rectangle.Width / 2) <= finalDestinationRec.Right) == false)
            {
                rectangle.Y += 3; //was 2
            }
            if (jump == true)
            {
                rectangle.Y -= jumpHeight;
            }

            //make the height of the jump decay over time
            if (jump == true && counter % 10 == 0)
            {
                jumpHeight -= 1;
            }
            if (rectangle.Bottom - picboxHeightModifier >= finalDestinationRec.Top + stageHeightAdjustment && rectangle.Bottom - picboxHeightModifier <= finalDestinationRec.Top + stageHeightAdjustment + 15 && rectangle.Left + (rectangle.Width / 2) >= finalDestinationRec.Left && rectangle.Right - (rectangle.Width / 2) <= finalDestinationRec.Right)
            {
                jumpHeight = 0;
                jump = false;
            }
        }

        //the special attack
        private void specialAttack()
        {
            //all attacks damage gets applied in Game1
            //Mewtwo fires a projectile
            if (character == "Mewtwo" && counterSprite == 3 && counter % counterSpriteModifier == 0)
            {
                if (direction == "Left")
                {
                    projectileRec = new Rectangle(rectangle.X - (rectangle.Width / 4), rectangle.Y + (rectangle.Height / 4), 10, 10);
                }
                if (direction == "Right")
                {
                    projectileRec = new Rectangle(rectangle.X + (rectangle.Width / 4), rectangle.Y + (rectangle.Height / 4), 10, 10);
                }
                PlayerProjectiles = new Projectiles(projectileRec, null, direction, player, character, content);
            }

            //Charizard fires a projectile
            if ((character == "Charizard") && counterSprite == 2 && counter % counterSpriteModifier == 0)
            {
                if (direction == "Left")
                {
                    projectileRec = new Rectangle(rectangle.X - (rectangle.Width / 4), rectangle.Y + (rectangle.Height / 4), 10, 10);
                }
                if (direction == "Right")
                {
                    projectileRec = new Rectangle(rectangle.X + (rectangle.Width / 4), rectangle.Y + (rectangle.Height / 4), 10, 10);
                }

                PlayerProjectiles = new Projectiles(projectileRec, null, direction, player, character, content);
            }

            //Mario and Luigi fire projectiles
            if ((character == "Mario" || character == "Luigi") && counterSprite == 4 && counter % counterSpriteModifier == 0)
            {
                if (direction == "Left")
                {
                    projectileRec = new Rectangle(rectangle.X + (rectangle.Width / 4), rectangle.Y + (rectangle.Height / 2), 10, 10);
                }
                if (direction == "Right")
                {
                    projectileRec = new Rectangle(rectangle.Right - (rectangle.Width / 4), rectangle.Y + (rectangle.Height / 2), 10, 10);
                }
                PlayerProjectiles = new Projectiles(projectileRec, null, direction, player, character, content);
            }

            if (character == "Pichu" && counterSprite == 1 && counter % counterSpriteModifier == 0)
            {
                PlayerProjectiles = new Projectiles(rectangle, pichuCloud, direction, player, character, content);
            }

            //Shrek does a special punch
            if ((character == "Shrek" || character == "Seiar" || character == "Metaknight" || character == "Sonic") && counterSprite == 4 && counter % counterSpriteModifier == 0)
            {
                attackFrame = true;
            }

            //knuckles does a fire punch
            if (character == "Knuckles" && counterSprite == 2 && counter % counterSpriteModifier == 0)
            {
                attackFrame = true;
            }

            //kirby does a fire punch
            if ((character == "Kirby" || character == "King") && counterSprite == 3 && counter % counterSpriteModifier == 0)
            {
                attackFrame = true;
            }

            //shadow creates a shield
            if (character == "Shadow" && counterSprite >= 3 && counterSprite <= 10 && counter % counterSpriteModifier == 0)
            {
                attackFrame = true;
            }

            //blastoise fires a projectile
            if (character == "Blastoise" && counterSprite >= 2 && counter % counterSpriteModifier == 0)
            {
                if (direction == "Left")
                {
                    projectileRec = new Rectangle(rectangle.Left + (rectangle.Width / 4) - (BlastoiseWaterLeft.Width * 4), rectangle.Y, BlastoiseWaterLeft.Width * 4, BlastoiseWaterLeft.Height * 4);
                }
                if (direction == "Right")
                {
                    projectileRec = new Rectangle(rectangle.Right - (rectangle.Width / 4), rectangle.Y, BlastoiseWaterLeft.Width * 4, BlastoiseWaterLeft.Height * 4);
                }
                PlayerProjectiles = new Projectiles(projectileRec, BlastoiseWaterLeft, direction, player, character, content);
            }
            if (character == "Blastoise" && counter % 10 == 0)
            {
                attackFrame = true;
            }

            if (character == "Link" && (counterSprite == 3 || counterSprite == 7) && counter % counterSpriteModifier == 0)
            {
                attackFrame = true;
            }
        }

        //send the damage being dealt
        public int attackType()
        {
            if (attack == true && attackFrame == true)
            {
                attackFrame = false;
                return 1;
            }

            else if (attack2 == true && attackFrame == true && character == "Link" && counterSprite == 7)
            {
                attackFrame = false;
                return 4;
            }

            else if (attack2 == true && attackFrame == true)
            {
                attackFrame = false;
                return 2;
            }

            else if (attack2 == true && character == "Shadow")
            {
                return 3;
            }

            else
            {
                return 0;
            }
        }

        //send the players direction
        public string directionPlayer()
        {
            if (direction == "Left")
            {
                return "Left";
            }
            else
            {
                return "Right";
            }
        }

        //send the players rectangle
        public Rectangle setRectangle()
        {
            return rectangle;
        }

        //check if the projectile has been created
        public bool checkProjectile()
        {
            if (PlayerProjectiles == null)
            {
                return false;
            }

            else
            {
                return true;
            }
        }

        //send the projectile rectangle
        public Rectangle setProjectileRec()
        {
            return PlayerProjectiles.getRectangle();
        }

        //destory the projectile rectangle
        public void destroyProjectileRec()
        {
            //destroy the projectiles
            if (character != "Pichu")
            {
                PlayerProjectiles.destroyRec();
            }
        }

        //knockback
        private void knockbackMethod()
        {
            if (knockback != 0)
            {
                //apply the knockback
                rectangle.X += knockback;

                //make the knockback decay over time
                if (counter % 10 == 0 && knockback > 0)
                {
                    knockback -= 1;
                }

                if (counter % 10 == 0 && knockback < 0)
                {
                    knockback += 1;
                }

                if (Math.Abs(dx) > Math.Abs(knockback))
                {
                    knockback = 0;
                }
            }

            //apply the knockup
            if (knockup != 0)
            {
                rectangle.Y -= knockup;

                if (counter % 10 == 0)
                {
                    knockup -= 1;
                }
            }
        }

        //taking damage
        public void getDamage(int damage, int newKnockback)
        {
            //add the damage taken to the players health
            damageTaken += damage;
            knockup = (int)(damageTaken * Math.Abs(newKnockback) / 50);

            //knock players back further the more damage they've taken
            if (newKnockback > 0 && Math.Abs(knockback) < (newKnockback * (damageTaken / 30)))
            {
                knockback = (newKnockback * (damageTaken / 25));
            }

            if (newKnockback < 0 && Math.Abs(knockback) < Math.Abs(newKnockback * (damageTaken / 30)))
            {
                knockback = (newKnockback * (damageTaken / 25));
            }
        }

        //death and respawning
        private void deathMethod()
        {
            if (rectangle.Right < 0 || rectangle.Left > displayWidth || rectangle.Bottom < -100 || rectangle.Top > displayHeight || (rectangle.X == 0 && rectangle.Y == 0))
            {
                //check if they were killed
                if (damageTaken > 0)
                {
                    isDead = true;

                    if (isBot == true)
                    {
                        if (character == "Mario")
                        {
                            character = "Luigi";
                        }
                        else if (character == "Luigi")
                        {
                            character = "Mewtwo";
                        }
                        else if (character == "Mewtwo")
                        {
                            character = "Shadow";
                        }
                        else if (character == "Shadow")
                        {
                            character = "Metaknight";
                        }
                        else if (character == "Metaknight")
                        {
                            character = "Mario";
                        }
                    }
                }

                //make sure the newly spawned character isnt moving
                damageTaken = 0;
                knockback = 0;
                knockup = 0;
                dx = 0;
                dy = 0;
                taunt = false;
                jump = false;
                jumpHeight = 0;

                //small hitbox corrections for characters with space below their feet
                if (character == "Shadow")
                {
                    picboxHeightModifier = (int)(aspectRatio * widthScaling * (displayWidth / 15)) / 10;
                    //rectangle.Y = (displayHeight / 5) - picboxHeightModifier;
                }

                else if (character == "Metaknight" || character == "Kirby")
                {
                    picboxHeightModifier = (int)(aspectRatio * widthScaling * (displayWidth / 15)) / 5;
                    //rectangle.Y = (displayHeight / 5) - picboxHeightModifier;
                }

                else if (character == "Link")
                {
                    picboxHeightModifier = (int)((aspectRatio * widthScaling * (displayWidth / 15)) / 3.5);
                    //rectangle.Y = (displayHeight / 5) - picboxHeightModifier;
                }

                else
                {
                    picboxHeightModifier = 0;
                }

                //spawn the character
                if (isBot == false)
                {
                    //rectangle = new Rectangle(displayWidth * (player) / 5, (displayHeight / 5) - picboxHeightModifier, (int)(displayWidth * widthScaling / 15), (int)(aspectRatio * widthScaling * (displayWidth / 15)));
                    //finalDestinationRec.Top + (finalDestinationRec.Height / 5) - 5
                    rectangle = new Rectangle(displayWidth * (player) / 5, (finalDestinationRec.Top + stageHeightAdjustment - (int)(aspectRatio * widthScaling * (displayWidth / 15)) + picboxHeightModifier), (int)(displayWidth * widthScaling / 15), (int)(aspectRatio * widthScaling * (displayWidth / 15)));
                }
                if (isBot == true)
                {
                    rectangle = new Rectangle((displayWidth * 3) / 5, (displayHeight) / 4 - (rectangle.Height + 20), (int)(displayWidth * widthScaling / 15), (int)(aspectRatio * widthScaling * (displayWidth / 15)));
                }
            }
        }

        //tell the other pichu what attack to use
        public int pichuAttack()
        {
            if (attack == true)
            {
                return 1;
            }
            if (attack2 == true)
            {
                return 2;
            }
            return 0;
        }

        //tell Game1 when the player dies
        public bool setDeath()
        {
            return isDead;
        }

        //draw their health
        public void DrawText(SpriteBatch spriteBatch)
        {
            if (isBot == false)
            {
                spriteBatch.Draw(seriesSymbol, seriesSymbolRec, Color.White);
                spriteBatch.DrawString(font1, damageTaken.ToString() + "%", new Vector2(205 + (player * 200), 5 + (displayHeight * 4) / 5), Color.Black);
                spriteBatch.DrawString(font1, damageTaken.ToString() + "%", new Vector2(200 + (player * 200), (displayHeight * 4) / 5), Color.White);
                spriteBatch.Draw(LivesIcon, LivesIconRec, Color.White);
            }

            else
            {
                spriteBatch.Draw(seriesSymbol, seriesSymbolRec, Color.White);
                spriteBatch.DrawString(font1, damageTaken.ToString() + "%", new Vector2(205 + (2 * 200), 5 + (displayHeight * 4) / 5), Color.Black);
                spriteBatch.DrawString(font1, damageTaken.ToString() + "%", new Vector2(200 + (2 * 200), (displayHeight * 4) / 5), Color.White);
                spriteBatch.Draw(LivesIcon, LivesIconRec, Color.White);
            }

            if (PlayerProjectiles != null)
            {
                PlayerProjectiles.Draw(spriteBatch);

                if (character == "Pichu")
                {
                    PlayerProjectiles.Draw2(spriteBatch);
                }
            }
        }
    }
}
