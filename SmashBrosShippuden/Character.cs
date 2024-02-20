﻿//Jake Loftus
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
        public string character;
        public int player;
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
        public int damageTaken;
        protected int picboxHeightModifier;
        protected AttackType attack;
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
        Texture2D[] spriteRun;
        Texture2D[] spriteAttack;
        Texture2D[] spriteSmash;
        Texture2D[] spriteJump = new Texture2D[1];
        Texture2D[] spriteHurt = new Texture2D[1];
        Texture2D[] luigiTaunt = new Texture2D[12];
        public Texture2D LivesIcon;
        public Texture2D seriesSymbol;

        //sound effects
        SoundEffect[] spriteSounds = new SoundEffect[4];
        SoundEffect[] spriteSounds2 = new SoundEffect[3];
        SoundEffect goWeegee;

        public Rectangle seriesSymbolRec;
        public Rectangle LivesIconRec;
        protected Rectangle finalDestinationRec = new Rectangle();

        GamePadState pad1;

        public Character(Rectangle newRectangle, Texture2D newTexture, string newDirection, int newPlayer, string newCharacter, int disWidth, int disHeight, Rectangle stage, int stageHeight, bool bot)
            : base(newTexture, newRectangle)
        {
            //rectangle = newRectangle;
            direction = newDirection;
            player = newPlayer;
            character = newCharacter;
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
            displaySprite();
            gravity();
            knockbackMethod();
            rectangle.X += dx;
            rectangle.Y += dy;
            dx = 0;
            dy = 0;

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

            else if (character == "Luigi")
            {
                spriteAttack1Length = 7;
                spriteAttack2Length = 5;
                spriteRunLength = 8;
                counterSpriteModifier = 5;
                counterSpriteModRun = 8;
                counterSpriteAttack = 5;
                widthScaling = 2.2f;
            }

            else if (character == "Pichu")
            {
                spriteAttack1Length = 5;
                spriteAttack2Length = 5;
                spriteRunLength = 4;
                counterSpriteModifier = 8;
                counterSpriteModRun = 8;
                counterSpriteAttack = 2;
                widthScaling = 1.5f;
            }

            else if (character == "Mewtwo")
            {
                spriteAttack1Length = 4;
                spriteAttack2Length = 4;
                spriteRunLength = 2;
                counterSpriteModifier = 10;
                counterSpriteModRun = 8;
                counterSpriteAttack = 3;
                widthScaling = 1.7f;
            }

            else if (character == "Charizard")
            {
                spriteAttack1Length = 4;
                spriteAttack2Length = 4;
                spriteRunLength = 4;
                counterSpriteModifier = 8;
                counterSpriteModRun = 10;
                counterSpriteAttack = 3;
                widthScaling = 2;
            }

            else if (character == "Shadow")
            {
                spriteAttack1Length = 9;
                spriteAttack2Length = 17;
                spriteRunLength = 9;
                counterSpriteModifier = 5;
                counterSpriteModRun = 6;
                counterSpriteAttack = 3;
                widthScaling = 2.2f;
            }

            else if (character == "Knuckles")
            {
                spriteAttack1Length = 6;
                spriteAttack2Length = 5;
                spriteRunLength = 8;
                counterSpriteModifier = 6;
                counterSpriteModRun = 5;
                counterSpriteAttack = 3;
                widthScaling = 1.5f;
            }

            else if (character == "Sonic")
            {
                spriteAttack1Length = 5;
                spriteAttack2Length = 9;
                spriteRunLength = 8;
                counterSpriteModifier = 5;
                counterSpriteModRun = 6;
                counterSpriteAttack = 3;
                widthScaling = 1.7f;
            }

            else if (character == "Link")
            {
                spriteAttack1Length = 7;
                spriteAttack2Length = 8;
                spriteRunLength = 6;
                counterSpriteModifier = 4;
                counterSpriteModRun = 6;
                counterSpriteAttack = 4;
                widthScaling = 2.3f;
            }

            else if (character == "Shrek")
            {
                spriteAttack1Length = 6;
                spriteAttack2Length = 8;
                spriteRunLength = 8;
                counterSpriteModifier = 6;
                counterSpriteModRun = 6;
                counterSpriteAttack = 3;
                widthScaling = 2;
            }

            else if (character == "Blastoise")
            {
                spriteAttack1Length = 4;
                spriteAttack2Length = 3;
                spriteRunLength = 4;
                counterSpriteModifier = 6;
                counterSpriteModRun = 8;
                counterSpriteAttack = 1;
                widthScaling = 1.4f;
            }

            else if (character == "Metaknight")
            {
                spriteAttack1Length = 9;
                spriteAttack2Length = 8;
                spriteRunLength = 8;
                counterSpriteModifier = 4;
                counterSpriteModRun = 4;
                counterSpriteAttack = 3;
                widthScaling = 4f;
            }

            else if (character == "Kirby")
            {
                spriteAttack1Length = 4;
                spriteAttack2Length = 5;
                spriteRunLength = 10;
                counterSpriteModifier = 6;
                counterSpriteModRun = 5;
                counterSpriteAttack = 3;
                widthScaling = 2.2f;
            }

            else if (character == "King")
            {
                spriteAttack1Length = 6;
                spriteAttack2Length = 5;
                spriteRunLength = 4;
                counterSpriteModifier = 7;
                counterSpriteModRun = 8;
                counterSpriteAttack = 5;
                widthScaling = 3;
            }

            else if (character == "waddle")
            {
                widthScaling = 0.9f;
                spriteRunLength = 8;
                counterSpriteModifier = 6;
                counterSpriteModRun = 6;
            }
        }

        public override void LoadContent(ContentManager content)
        { 
            spriteRun = new Texture2D[spriteRunLength];
            spriteSmash = new Texture2D[spriteAttack1Length];
            spriteAttack = new Texture2D[spriteAttack2Length];

            //load the characters running and attact sprites
            for (int i = 0; i < spriteRunLength; i++)
            {
                spriteRun[i] = content.Load<Texture2D>(character + "/" + character.ToLower() + "Run" + (i + 1));
            }

            for (int i = 0; i < spriteAttack1Length; i++)
            {
                spriteSmash[i] = content.Load<Texture2D>(character + "/" + character.ToLower() + "Smash" + (i + 1));
            }

            for (int i = 0; i < spriteAttack2Length; i++)
            {
                spriteAttack[i] = content.Load<Texture2D>(character + "/" + character.ToLower() + "Attack" + (i + 1));
            }

            //jumping sprites
            spriteJump[0] = content.Load<Texture2D>(character + "/" + character.ToLower() + "Jump");

            //hurt sprites
            spriteHurt[0] = content.Load<Texture2D>(character + "/" + character.ToLower() + "Hurt");

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

            for (int i = 0; i < luigiTaunt.Length; i++)
            {
                luigiTaunt[i] = content.Load<Texture2D>("Luigi/luigiTaunt" + (i + 1));
            }

            goWeegee = content.Load<SoundEffect>("Luigi/luigi021");

            aspectRatio = (float)spriteRun[0].Height / spriteRun[0].Width;

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

            this.rectangle = new Rectangle(displayWidth * (player) / 5, (finalDestinationRec.Top + stageHeightAdjustment - (int)(aspectRatio * widthScaling * (displayWidth / 15)) + picboxHeightModifier), (int)(displayWidth * widthScaling / 15), (int)(aspectRatio * widthScaling * (displayWidth / 15)));
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
            if (Math.Abs(knockback) > 4)
            {
                texture = spriteHurt[0];
                this.attack = AttackType.None;
                jump = false;
            }

            //make the character do their special attack
            else if (this.attack == AttackType.Special)
            {
                specialAttack();
                if (character != "Knuckles" && character != "Pichu")
                {
                    dx = 0;
                }

                if (counterSprite < spriteAttack2Length)
                {
                    texture = spriteAttack[counterSprite];

                    if (counterSprite == 1 && counter % counterSpriteModifier == 1 && (character == "Link" || character == "Pichu"))
                    {
                        spriteSounds2[NumberGenerator.Next(0, 3)].Play();
                    }
                }

                else
                {
                    this.attack = AttackType.None;
                }
            }

            //Display the attack animation when a player attacks
            else if (this.attack == AttackType.Jab)
            {
                if (counterSprite < spriteAttack1Length)
                {
                    texture = spriteSmash[counterSprite];

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
                    this.attack = AttackType.None;
                }
            }

            //display jumping sprite
            else if (jump == true)
            {
                texture = spriteJump[0];
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
                texture = spriteRun[counterSprite2 % spriteRunLength];
            }
        }

        //getting input
        public void getInput(GamePadState pad1)
        {
            if (pad1.ThumbSticks.Left.X < -0.1)
            {
                if (this.attack != AttackType.None && direction == "Right")
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
                if (this.attack != AttackType.None && direction == "Left")
                {

                }
                else
                {
                    direction = "Right";
                    dx = 3;
                    taunt = false;
                }
            }

            if ((pad1.Buttons.B == ButtonState.Pressed || pad1.Buttons.Y == ButtonState.Pressed) && jump == false && this.attack == AttackType.None)
            {
                jump = true;
                jumpHeight = 9;
                counterSprite = 0;
                taunt = false;
            }

            if (pad1.Buttons.X == ButtonState.Pressed && this.attack == AttackType.None)
            {
                this.attack = AttackType.Special;
                counterSprite = 0;
                taunt = false;
            }

            if (pad1.Buttons.A == ButtonState.Pressed && this.attack == AttackType.None)
            {
                this.attack = AttackType.Jab;
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

            if (this.attack == AttackType.None && attacking == true)
            {
                this.attack = AttackType.Jab;
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
                rectangle.Y += 4; //was 2
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
            //Shrek does a special punch
            if ((character == "Shrek" || character == "Metaknight" || character == "Sonic") && counterSprite == 4 && counter % counterSpriteModifier == 0)
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

            if (character == "Blastoise" && counter % 10 == 0)
            {
                attackFrame = true;
            }

            if (character == "Link" && (counterSprite == 3 || counterSprite == 7) && counter % counterSpriteModifier == 0)
            {
                attackFrame = true;
            }
        }

        public bool createProjectile()
        {
            if (counter % counterSpriteModifier != 0 || this.attack != AttackType.Special)
            {
                return false;
            }

            //Mewtwo fires a projectile
            else if (character == "Mewtwo" && counterSprite == 3)
            {
                return true;
            }

            //Charizard fires a projectile
            else if ((character == "Charizard") && counterSprite == 2)
            {
                return true;
            }

            //Mario and Luigi fire projectiles
            else if ((character == "Mario" || character == "Luigi") && counterSprite == 4)
            {
                return true;
            }

            else if (character == "Pichu" && counterSprite == 1)
            {
                return true;
            }

            //blastoise fires a projectile
            else if (character == "Blastoise" && counterSprite >= 2)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        //send the damage being dealt
        public int attackType()
        {
            if (this.attack == AttackType.Jab && attackFrame == true)
            {
                attackFrame = false;
                return 1;
            }

            else if (this.attack == AttackType.Special && attackFrame == true && character == "Link" && counterSprite == 7)
            {
                attackFrame = false;
                return 4;
            }

            else if (this.attack == AttackType.Special && attackFrame == true)
            {
                attackFrame = false;
                return 2;
            }

            else if (this.attack == AttackType.Special && character == "Shadow")
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
            return direction;
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

        //tell Game1 when the player dies
        public bool setDeath()
        {
            return isDead;
        }
    }
}
