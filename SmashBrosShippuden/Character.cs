//Jake Loftus
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Transactions;

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
        protected int counterSpriteModifier;
        protected int counterTaunt;
        protected int spriteIdleLength = 1;
        protected int spriteRunLength;
        protected int jumpHeight;
        public int damageTaken;
        protected int picboxHeightModifier;
        public Attack attack;
        protected bool jump;
        protected bool taunt;
        protected bool isShielding;
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
        Texture2D[] spriteIdle;
        Texture2D[] spriteJump = new Texture2D[1];
        Texture2D[] spriteHurt = new Texture2D[1];
        Texture2D[] shieldEffect = new Texture2D[10];
        Texture2D[] luigiTaunt = new Texture2D[12];
        public Texture2D LivesIcon;
        public Texture2D seriesSymbol;

        //sound effects
        SoundEffect[] spriteSounds = new SoundEffect[4];
        SoundEffect[] spriteSounds2 = new SoundEffect[3];

        public Rectangle seriesSymbolRec;
        public Rectangle LivesIconRec;
        protected Rectangle finalDestinationRec = new Rectangle();

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
                spriteRunLength = 8;
                counterSpriteModifier = 5;
                widthScaling = 2;
            }

            else if (character == "Luigi")
            {
                spriteRunLength = 8;
                counterSpriteModifier = 5;
                widthScaling = 2.2f;
            }

            else if (character == "Pichu")
            {
                spriteRunLength = 4;
                counterSpriteModifier = 8;
                widthScaling = 1.5f;
            }

            else if (character == "Mewtwo")
            {
                spriteRunLength = 2;
                counterSpriteModifier = 10;
                widthScaling = 1.7f;
            }

            else if (character == "Charizard")
            {
                spriteRunLength = 4;
                counterSpriteModifier = 8;
                widthScaling = 2;
            }

            else if (character == "Shadow")
            {
                spriteIdleLength = 6;
                spriteRunLength = 9;
                counterSpriteModifier = 5;
                widthScaling = 2.2f;
            }

            else if (character == "Knuckles")
            {
                spriteRunLength = 8;
                counterSpriteModifier = 6;
                widthScaling = 1.5f;
            }

            else if (character == "Sonic")
            {
                spriteIdleLength = 6;
                spriteRunLength = 8;
                counterSpriteModifier = 5;
                widthScaling = 1.7f;
            }

            else if (character == "Link")
            {
                spriteRunLength = 6;
                counterSpriteModifier = 4;
                widthScaling = 2.3f;
            }

            else if (character == "Shrek")
            {
                spriteRunLength = 8;
                counterSpriteModifier = 6;
                widthScaling = 2;
            }

            else if (character == "Blastoise")
            {
                spriteRunLength = 4;
                counterSpriteModifier = 6;
                widthScaling = 1.4f;
            }

            else if (character == "Metaknight")
            {
                spriteRunLength = 8;
                counterSpriteModifier = 4;
                widthScaling = 4f;
            }

            else if (character == "Kirby")
            {
                spriteRunLength = 10;
                counterSpriteModifier = 6;
                widthScaling = 2.2f;
            }

            else if (character == "King")
            {
                spriteRunLength = 4;
                counterSpriteModifier = 7;
                widthScaling = 3;
            }

            else if (character == "Sasuke")
            {
                spriteIdleLength = 4;
                spriteRunLength = 2;
                counterSpriteModifier = 8;
                widthScaling = 3;
            }

            else if (character == "waddle")
            {
                widthScaling = 0.9f;
                spriteRunLength = 8;
                counterSpriteModifier = 6;
            }
        }

        public override void LoadContent(ContentManager content)
        { 
            spriteRun = new Texture2D[spriteRunLength];
            spriteIdle = new Texture2D[spriteIdleLength];
            Attack tempAttack = new Attack(this.character, AttackType.Jab);
            spriteSmash = new Texture2D[tempAttack.spriteLength];
            tempAttack = new Attack(this.character, AttackType.Special);
            spriteAttack = new Texture2D[tempAttack.spriteLength];

            //load the characters running and attact sprites
            for (int i = 0; i < spriteRunLength; i++)
            {
                spriteRun[i] = content.Load<Texture2D>(character + "/" + character.ToLower() + "Run" + (i + 1));
            }

            for (int i = 0; i < spriteIdleLength; i++)
            {
                spriteIdle[i] = content.Load<Texture2D>(character + "/" + character.ToLower() + (i + 1));
            }

            for (int i = 0; i < spriteSmash.Length; i++)
            {
                spriteSmash[i] = content.Load<Texture2D>(character + "/" + character.ToLower() + "Smash" + (i + 1));
            }

            for (int i = 0; i < spriteAttack.Length; i++)
            {
                spriteAttack[i] = content.Load<Texture2D>(character + "/" + character.ToLower() + "Attack" + (i + 1));
            }

            //jumping sprites
            spriteJump[0] = content.Load<Texture2D>(character + "/" + character.ToLower() + "Jump");

            //hurt sprites
            spriteHurt[0] = content.Load<Texture2D>(character + "/" + character.ToLower() + "Hurt");

            // shield sprites
            for (int i = 0; i < this.shieldEffect.Length; i++)
            {
                this.shieldEffect[i] = content.Load<Texture2D>("Effects/guard" + i);
            }

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

            this.rectangle = new Rectangle(displayWidth * (player) / 5, (finalDestinationRec.Top + stageHeightAdjustment - (int)(aspectRatio * widthScaling * (displayWidth / 15)) + picboxHeightModifier), spriteRun[0].Width * 4, spriteRun[0].Height * 4);
        }

        //display the sprite for each characters animations
        private void displaySprite()
        {
            this.attackFrame = false;

            //scroll through animations
            if (counter % counterSpriteModifier == 0)
            {
                counterSprite++;
            }

            //display the damaged animation when they take tons of damage
            if (Math.Abs(knockback) > 4)
            {
                texture = spriteHurt[0];
                this.attack = null;
                jump = false;
            }

            //make the character do their special attack
            else if (this.attack != null && this.attack.attackType == AttackType.Special)
            {
                if (character != "Knuckles" && character != "Pichu")
                {
                    dx = 0;
                }

                if (counterSprite < spriteAttack.Length)
                {
                    if (Array.IndexOf(this.attack.attackFrame, counterSprite) > -1 && counter % counterSpriteModifier == 0)
                    {
                        this.attackFrame = true;
                    }

                    texture = spriteAttack[counterSprite];

                    if (counterSprite == 1 && counter % counterSpriteModifier == 1 && (character == "Link" || character == "Pichu"))
                    {
                        spriteSounds2[NumberGenerator.Next(0, 3)].Play();
                    }
                }

                else
                {
                    this.attack = null;
                }
            }

            //Display the attack animation when a player attacks
            else if (this.attack != null && this.attack.attackType == AttackType.Jab)
            {
                if (counterSprite < spriteSmash.Length)
                {
                    texture = spriteSmash[counterSprite];

                    if (Array.IndexOf(this.attack.attackFrame, counterSprite) > -1 && counter % counterSpriteModifier == 0)
                    {
                        this.attackFrame = true;
                    }
                    //if (counterSprite == 2 && counter % counterSpriteModifier == 1 && (character != "Shrek" && character != "King") || (counterSprite == 5 && counter % counterSpriteModifier == 1 && character == "King"))
                    //{
                    //    spriteSounds[NumberGenerator.Next(0, 4)].Play();
                    //}
                }
                else
                {
                    this.attack = null;
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

            else if (this.dx != 0)
            {
                texture = spriteRun[counterSprite % spriteRunLength];
            }

            //If the player isnt doing anything, display idle animation
            else
            {
                texture = spriteIdle[counterSprite % spriteIdleLength];
            }
        }

        // return whether a projectile should be created at the current frame
        public bool createProjectile()
        {
            return this.attack != null && this.attackFrame && this.attack.createProjectile;
        }

        public bool isAttackFrame()
        {
            return this.attack != null && this.attackFrame;
        }

        //getting input
        public void getInput(GamePadState pad1)
        {
            taunt = false;

            // clear released input
            if (pad1.Triggers.Right < 0.5 && this.isShielding)
            {
                this.isShielding = false;
            }

            // no input processed when attacking or during knockback
            if (this.attack != null || Math.Abs(knockback) > 2)
            {
                this.isShielding = false;
                return;
            }

            else if (pad1.Triggers.Right > 0.5 && !this.jump)
            {
                if (!this.isShielding)
                {
                    this.isShielding = true;
                }
            }
            else if (pad1.Buttons.X == ButtonState.Pressed)
            {
                this.attack = new Attack(this.character, AttackType.Special);
                counterSprite = 0;
            }
            else if (pad1.Buttons.A == ButtonState.Pressed)
            {
                this.attack = new Attack(this.character, AttackType.Jab);
                counterSprite = 0;
            }
            else if ((pad1.Buttons.B == ButtonState.Pressed || pad1.Buttons.Y == ButtonState.Pressed) && jump == false)
            {
                jump = true;
                jumpHeight = 9;
                counterSprite = 0;
            }
            else if (pad1.ThumbSticks.Left.X < -0.1)
            {
                direction = "Left";
                dx = -3;
            }
            else if (pad1.ThumbSticks.Left.X > 0.1)
            {
                direction = "Right";
                dx = 3;
            }
            else if (pad1.DPad.Up == ButtonState.Pressed && character == "Luigi")
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

            if (this.attack == null && attacking == true)
            {
                this.attack = new Attack(this.character, AttackType.Jab);
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

        //send the damage being dealt
        public int attackType()
        {
            if (this.attack.attackType == AttackType.Jab && attackFrame == true)
            {
                attackFrame = false;
                return 1;
            }

            else if (this.attack.attackType == AttackType.Special && attackFrame == true && character == "Link" && counterSprite == 7)
            {
                attackFrame = false;
                return 4;
            }

            else if (this.attack.attackType == AttackType.Special && attackFrame == true)
            {
                attackFrame = false;
                return 2;
            }

            else if (this.attack.attackType == AttackType.Special && character == "Shadow")
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
            knockup = 3 + (Math.Abs(newKnockback) * damageTaken / 50);
            if (newKnockback > 0)
            {
                knockback = 3 + newKnockback * (5 + damageTaken / 25);
            }
            else
            {
                knockback = -3 + newKnockback * (5 + damageTaken / 25);
            }
            
        }

        //tell Game1 when the player dies
        public bool setDeath()
        {
            return isDead;
        }

        public bool shieldBlock()
        {
            return this.isShielding;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (this.isShielding)
            {
                Rectangle shieldEffectRec = new Rectangle(
                    this.rectangle.X + this.rectangle.Width / 2 - this.shieldEffect[0].Width * 2,
                    this.rectangle.Y + this.rectangle.Height / 2 - this.shieldEffect[0].Height * 2,
                    this.shieldEffect[0].Width * 4,
                    this.shieldEffect[0].Height * 4
                );
                spriteBatch.Draw(this.shieldEffect[counterSprite % this.shieldEffect.Length], shieldEffectRec, Color.White);
            }
        }
    }
}
