//Jake Loftus
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
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
        protected int spriteIdleLength = 1;
        protected int spriteRunLength;
        protected int jumpHeight;
        protected int jumpCounter;
        protected int moveSpeed;
        public int damageTaken;
        protected int picboxHeightModifier;
        public Attack attack;
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
            if (dy != null)
            {
                rectangle.Y += (int)dy;
            }
            
            dx = 0;
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
                this.moveSpeed = 3;
                spriteIdleLength = 4;
                spriteRunLength = 8;
                counterSpriteModifier = 5;
                widthScaling = 2;
            }

            else if (character == "Luigi")
            {
                this.moveSpeed = 3;
                spriteIdleLength = 4;
                spriteRunLength = 8;
                counterSpriteModifier = 5;
                widthScaling = 2.2f;
            }

            else if (character == "Pichu")
            {
                this.moveSpeed = 4;
                spriteIdleLength = 2;
                spriteRunLength = 4;
                counterSpriteModifier = 8;
                widthScaling = 1.5f;
            }

            else if (character == "Mewtwo")
            {
                this.moveSpeed = 3;
                spriteIdleLength = 2;
                spriteRunLength = 2;
                counterSpriteModifier = 10;
                widthScaling = 1.7f;
            }

            else if (character == "Charizard")
            {
                this.moveSpeed = 2;
                spriteIdleLength = 1;
                spriteRunLength = 4;
                counterSpriteModifier = 8;
                widthScaling = 2;
            }

            else if (character == "Shadow")
            {
                this.moveSpeed = 6;
                spriteIdleLength = 6;
                spriteRunLength = 28;
                counterSpriteModifier = 6;
                widthScaling = 2.2f;
            }

            else if (character == "Knuckles")
            {
                this.moveSpeed = 4;
                spriteRunLength = 8;
                counterSpriteModifier = 6;
                widthScaling = 1.5f;
            }

            else if (character == "Sonic")
            {
                this.moveSpeed = 6;
                spriteIdleLength = 6;
                spriteRunLength = 8;
                counterSpriteModifier = 6;
                widthScaling = 1.7f;
            }

            else if (character == "Link")
            {
                this.moveSpeed = 3;
                spriteIdleLength = 1;
                spriteRunLength = 6;
                counterSpriteModifier = 4;
                widthScaling = 2.3f;
            }

            else if (character == "Shrek")
            {
                this.moveSpeed = 2;
                spriteRunLength = 8;
                counterSpriteModifier = 6;
                widthScaling = 2;
            }

            else if (character == "Blastoise")
            {
                this.moveSpeed = 3;
                spriteRunLength = 4;
                counterSpriteModifier = 6;
                widthScaling = 1.4f;
            }

            else if (character == "Metaknight")
            {
                this.moveSpeed = 4;
                spriteRunLength = 8;
                counterSpriteModifier = 4;
                widthScaling = 4f;
            }

            else if (character == "Kirby")
            {
                this.moveSpeed = 4;
                spriteIdleLength = 8;
                spriteRunLength = 10;
                counterSpriteModifier = 6;
                widthScaling = 2.2f;
            }

            else if (character == "King")
            {
                this.moveSpeed = 2;
                spriteIdleLength = 1;
                spriteRunLength = 4;
                counterSpriteModifier = 7;
                widthScaling = 3;
            }

            else if (character == "Sasuke")
            {
                this.moveSpeed = 5;
                spriteIdleLength = 4;
                spriteRunLength = 2;
                counterSpriteModifier = 8;
                widthScaling = 3;
            }

            else if (character == "waddle")
            {
                this.moveSpeed = 2;
                widthScaling = 0.9f;
                spriteRunLength = 8;
                counterSpriteModifier = 6;
            }

            this.jumpCounter = 1;
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
            counterSprite = counter / counterSpriteModifier;

            //display the damaged animation when they take tons of damage
            if (Math.Abs(knockback) > 4)
            {
                texture = spriteHurt[0];
                this.attack = null;
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
            else if (this.dy != null)
            {
                texture = spriteJump[0];
            }

            else if (taunt == true)
            {
                texture = luigiTaunt[counterSprite % luigiTaunt.Length];

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
            return this.attack != null && this.attackFrame && !this.attack.createProjectile;
        }

        //getting input
        public void getInput(GamePadState pad1)
        {
            taunt = false;

            // shield is released based on a counter
            if (this.isShielding && this.counter >= 99)
            {
                this.isShielding = false;
            }

            // add endlag for releasing the shield
            if (pad1.Triggers.Right < 0.5 && this.isShielding && this.counter < 80)
            {
                this.counter = 80;
            }

            // no input processed when attacking or during knockback
            if (this.attack != null || Math.Abs(knockback) > 2)
            {
                this.isShielding = false;
                return;
            }

            else if (pad1.Triggers.Right > 0.5 && this.dy == null)
            {
                if (!this.isShielding)
                {
                    this.isShielding = true;
                    this.counter = 0;
                }
            }
            else if (pad1.Buttons.X == ButtonState.Pressed)
            {
                this.attack = new Attack(this.character, AttackType.Special);
                counter = 0;
            }
            else if (pad1.Buttons.A == ButtonState.Pressed)
            {
                this.attack = new Attack(this.character, AttackType.Jab);
                counter = 0;
            }
            else if ((pad1.Buttons.B == ButtonState.Pressed || pad1.Buttons.Y == ButtonState.Pressed) 
                && this.jumpCounter > 0
                // add a counter delay if they multiple jumps so they aren't all used instantly
                && (this.dy == null || (counter > 30)))
            {
                this.jumpCounter--;
                this.dy = -8;
                counter = 0;
            }
            else if (pad1.ThumbSticks.Left.X < -0.1)
            {
                direction = "Left";
                dx = -1 * this.moveSpeed;
            }
            else if (pad1.ThumbSticks.Left.X > 0.1)
            {
                direction = "Right";
                dx = this.moveSpeed;
            }
            else if (pad1.DPad.Up == ButtonState.Pressed && character == "Luigi")
            {
                taunt = true;
            }
        }

        //control bots
        public void getInputBots(int newDX, bool jumping, string newDirection, bool attacking)
        {
            if (knockback != 0)
            {
                return;
            }
            if (dx == 0 && newDX != 0)
            {
                taunt = false;
            }

            dx = newDX;

            if (this.dy == null && this.jumpCounter > 0 && jumping == true)
            {
                this.dy = -8;
                counter = 0;
                taunt = false;
                this.jumpCounter--;
            }

            direction = newDirection;

            if (this.attack == null && attacking == true)
            {
                this.attack = new Attack(this.character, AttackType.Jab);
                counter = 0;
                taunt = false;
            }
        }

        //the physics engine 
        private void gravity()
        {
            // no action needed when player is jumping upwards
            if (this.dy < 0)
            {

            }

            // dy set to null when they land
            else if (rectangle.Bottom - picboxHeightModifier >= finalDestinationRec.Top + stageHeightAdjustment && rectangle.Bottom - picboxHeightModifier <= finalDestinationRec.Top + stageHeightAdjustment + 15 && rectangle.Left + (rectangle.Width / 2) >= finalDestinationRec.Left && rectangle.Right - (rectangle.Width / 2) <= finalDestinationRec.Right)
            {
                this.jumpCounter = 1;
                this.dy = null;
                if (this.knockup <= 4)
                {
                    this.knockup = 0;
                }
            }
            //make characters fall if they are not on a platform
            else if (this.dy == null)
            {
                this.dy = 4;
            }


            if (this.dy != null && this.dy < 8)
            {
                //make the height of the jump decay over time
                if (counter % 5 == 0)
                {
                    this.dy += 1;
                }
            }
        }

        //send the damage being dealt
        public int attackType()
        {
            if (this.attack == null)
            {
                return 0;
            }
            else if (this.attack.attackType == AttackType.Jab && attackFrame == true)
            {
                return 1;
            }
            else if (this.attack.attackType == AttackType.Special && attackFrame == true && character == "Link" && counterSprite == 7)
            {
                return 4;
            }
            else if (this.attack.attackType == AttackType.Special && attackFrame == true)
            {
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
            knockup = 2 + Math.Abs(newKnockback) * (2 + damageTaken / 50);
            if (newKnockback > 0)
            {
                knockback = 2 + newKnockback * (2 + damageTaken / 25);
                this.direction = "Left";
            }
            else
            {
                knockback = -2 + newKnockback * (2 + damageTaken / 25);
                this.direction = "Right";
            }
            
        }

        //tell Game1 when the player dies
        public bool setDeath()
        {
            return isDead;
        }

        public bool shieldBlock()
        {
            // only the first 8 frames of the shield animation block attacks
            return this.isShielding && this.counter < 80;
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
                spriteBatch.Draw(this.shieldEffect[(counter / 10) % 10], shieldEffectRec, Color.White);
            }
        }
    }
}
