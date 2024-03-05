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
        protected int spriteJumpLength;
        protected int jumpHeight;
        protected int jumpCounter;
        protected int moveSpeed;
        protected int hitboxWidth;
        protected int hitboxHeight;

        public int damageTaken;
        public Attack attack;
        protected bool taunt;
        protected bool isShielding;
        protected bool attackFrame;
        protected bool isBot;
        protected bool isDead;
        protected int displayWidth;
        protected int displayHeight;
        protected int stageHeightAdjustment;
        protected Random NumberGenerator = new Random();
        protected readonly int shieldFrameLength = 6;

        //all the sprites
        Texture2D[] spriteRun;
        Texture2D[][] spriteAttack = new Texture2D[4][];
        Texture2D[] spriteIdle;
        Texture2D[] spriteJump;
        Texture2D[] spriteJumpUp;
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

        public Character(int x, int y, string newDirection, int newPlayer, string newCharacter, int disWidth, int disHeight, Rectangle stage, int stageHeight, bool bot)
            : base(x, y)
        {
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

            if (this.attack != null)
            {
                this.x += this.attack.getDx(this.counterSprite);
            }
            else 
            {
                this.x += dx;
            }
            
            if (this.attack != null)
            {
                this.y += this.attack.getDy(this.counterSprite);
            }
            else if (dy != null)
            {
                this.y += (int)dy;
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
            this.spriteJumpLength = 1;

            if (character == "Mario")
            {
                this.moveSpeed = 4;
                spriteIdleLength = 4;
                spriteRunLength = 8;
                counterSpriteModifier = 5;
                this.hitboxWidth = 20;
                this.hitboxHeight = 33;
                this.hitboxYOffset = 1;
            }

            else if (character == "Luigi")
            {
                this.moveSpeed = 4;
                spriteIdleLength = 4;
                spriteRunLength = 8;
                counterSpriteModifier = 5;
                this.hitboxWidth = 16;
                this.hitboxHeight = 38;
            }

            else if (character == "Pichu")
            {
                this.moveSpeed = 5;
                spriteIdleLength = 2;
                spriteRunLength = 4;
                counterSpriteModifier = 8;
            }

            else if (character == "Mewtwo")
            {
                this.moveSpeed = 4;
                spriteIdleLength = 2;
                spriteRunLength = 2;
                counterSpriteModifier = 10;
            }

            else if (character == "Charizard")
            {
                this.moveSpeed = 3;
                spriteIdleLength = 1;
                spriteRunLength = 4;
                counterSpriteModifier = 8;
                this.hitboxWidth = 22;
                this.hitboxHeight = 26;
            }

            else if (character == "Shadow")
            {
                this.moveSpeed = 8;
                spriteIdleLength = 6;
                spriteRunLength = 28;
                this.spriteJumpLength = 2;
                counterSpriteModifier = 6;
                this.hitboxWidth = 26;
                this.hitboxHeight = 35;
                this.hitboxYOffset = 6;
            }

            else if (character == "Knuckles")
            {
                this.moveSpeed = 5;
                spriteIdleLength = 8;
                spriteRunLength = 8;
                this.spriteJumpLength = 2;
                counterSpriteModifier = 6;
                this.hitboxWidth = 24;
                this.hitboxHeight = 34;
                this.hitboxYOffset = 6;
            }

            else if (character == "Sonic")
            {
                this.moveSpeed = 8;
                spriteIdleLength = 6;
                spriteRunLength = 8;
                counterSpriteModifier = 6;
                this.hitboxWidth = 22;
                this.hitboxHeight = 34;
                this.hitboxYOffset = 1;
            }

            else if (character == "Link")
            {
                this.moveSpeed = 4;
                spriteIdleLength = 1;
                spriteRunLength = 6;
                counterSpriteModifier = 4;
                this.hitboxWidth = 12;
                this.hitboxHeight = 23;
                this.hitboxYOffset = 14;
            }

            else if (character == "Shrek")
            {
                this.moveSpeed = 2;
                spriteRunLength = 8;
                counterSpriteModifier = 6;
            }

            else if (character == "Blastoise")
            {
                this.moveSpeed = 3;
                spriteRunLength = 4;
                counterSpriteModifier = 6;
            }

            else if (character == "Metaknight")
            {
                this.moveSpeed = 5;
                spriteRunLength = 8;
                counterSpriteModifier = 4;
                this.hitboxWidth = 26;
                this.hitboxHeight = 23;
                this.hitboxYOffset = 13;
            }

            else if (character == "Kirby")
            {
                this.moveSpeed = 4;
                spriteIdleLength = 8;
                spriteRunLength = 10;
                counterSpriteModifier = 6;
                this.hitboxWidth = 24;
                this.hitboxHeight = 22;
                this.hitboxYOffset = 13;
            }

            else if (character == "King")
            {
                this.moveSpeed = 2;
                spriteIdleLength = 1;
                spriteRunLength = 4;
                counterSpriteModifier = 7;
                this.hitboxWidth = 44;
                this.hitboxHeight = 50;
            }

            else if (character == "Sasuke")
            {
                this.moveSpeed = 5;
                spriteIdleLength = 4;
                spriteRunLength = 2;
                counterSpriteModifier = 8;
                this.hitboxWidth = 14;
                this.hitboxHeight = 34;
            }

            else if (character == "waddle")
            {
                this.moveSpeed = 3;
                spriteIdleLength = 1;
                spriteRunLength = 8;
                counterSpriteModifier = 6;
                this.hitboxWidth = 26;
                this.hitboxHeight = 30;
            }

            this.jumpCounter = 1;
        }

        public override void LoadContent(ContentManager content)
        { 
            spriteRun = new Texture2D[spriteRunLength];
            spriteIdle = new Texture2D[spriteIdleLength];
            this.spriteJump = new Texture2D[spriteJumpLength];
            this.spriteJumpUp = new Texture2D[spriteJumpLength];
            AttackType[] attackTypes = new AttackType[4] { AttackType.Jab, AttackType.SideSmash, AttackType.Special,  AttackType.SideSpecial };
            string[] attackLabels = new string[4] { "Smash", "SideSmash", "Attack", "SideSpecial" };

            // load the characters attack sprites
            foreach (AttackType a in attackTypes)
            {
                Attack tempAttack = new Attack(this.character, a, this.direction);
                if (tempAttack.spriteLength > 0)
                {
                    spriteAttack[(int)a] = new Texture2D[tempAttack.spriteLength];

                    for (int i = 0; i < tempAttack.spriteLength; i++)
                    {
                        spriteAttack[(int)a][i] = content.Load<Texture2D>(character + "/" + character.ToLower() + attackLabels[(int)a] + (i + 1));
                    }
                }
            }

            //load the characters running sprites
            for (int i = 0; i < spriteRunLength; i++)
            {
                spriteRun[i] = content.Load<Texture2D>(character + "/" + character.ToLower() + "Run" + (i + 1));
            }

            for (int i = 0; i < spriteIdleLength; i++)
            {
                spriteIdle[i] = content.Load<Texture2D>(character + "/" + character.ToLower() + (i + 1));
            }

            //jumping sprites
            for (int i = 0; i < this.spriteJump.Length; i++)
            {
                spriteJump[i] = content.Load<Texture2D>(character + "/" + character.ToLower() + "Jump" + (i+1));
                spriteJumpUp[i] = content.Load<Texture2D>(character + "/" + character.ToLower() + "JumpUp" + (i + 1));
            }

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

            //Display the attack animation when a player attacks
            else if (this.attack != null)
            {
                if (counterSprite < spriteAttack[(int)this.attack.attackType].Length)
                {
                    texture = spriteAttack[(int)this.attack.attackType][counterSprite];

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
                if (this.dy >= 0)
                {
                    texture = spriteJump[this.counterSprite % this.spriteJumpLength];
                }
                else
                {
                    texture = spriteJumpUp[this.counterSprite % this.spriteJumpLength];
                }
                
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

        public bool createCompanion()
        {
            return this.attack != null && this.attackFrame && this.attack.createCompanion;
        }

        public bool isAttackFrame()
        {
            return this.attack != null && this.attackFrame && !this.attack.createProjectile && !this.attack.createCompanion;
        }

        //getting input
        public void getInput(GamePadState pad1)
        {
            taunt = false;

            // shield is released based on a counter
            if (this.isShielding && this.counter >= this.shieldEffect.Length * this.shieldFrameLength - 1)
            {
                this.isShielding = false;
            }

            // add endlag for releasing the shield
            if (pad1.Triggers.Right < 0.5 && this.isShielding && this.counter < (this.shieldEffect.Length - 2) * this.shieldFrameLength)
            {
                this.counter = (this.shieldEffect.Length - 2) * this.shieldFrameLength;
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
            else if (pad1.Buttons.X == ButtonState.Pressed && Math.Abs(pad1.ThumbSticks.Left.X) > 0.4 && this.spriteAttack[(int)AttackType.SideSpecial] != null)
            {
                this.attack = new Attack(this.character, AttackType.SideSpecial, this.direction);
                counter = 0;
            }
            else if (pad1.Buttons.X == ButtonState.Pressed)
            {
                this.attack = new Attack(this.character, AttackType.Special, this.direction);
                counter = 0;
            }
            else if (pad1.Buttons.A == ButtonState.Pressed && Math.Abs(pad1.ThumbSticks.Left.X) > 0.4 && this.spriteAttack[(int)AttackType.SideSmash] != null)
            {
                this.attack = new Attack(this.character, AttackType.SideSmash, this.direction);
                counter = 0;
            }
            else if (pad1.Buttons.A == ButtonState.Pressed)
            {
                this.attack = new Attack(this.character, AttackType.Jab, this.direction);
                counter = 0;
            }
            else if ((pad1.Buttons.B == ButtonState.Pressed || pad1.Buttons.Y == ButtonState.Pressed) 
                && this.jumpCounter > 0
                // add a counter delay if they multiple jumps so they aren't all used instantly
                && (this.dy == null || (counter > 30)))
            {
                this.jumpCounter--;
                this.dy = -10;
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

            // move half as fast when jumping
            if (dy != null)
            {
                this.dx -= this.dx / 3;
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
                this.attack = new Attack(this.character, AttackType.Jab, this.direction);
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
            else if (this.y >= finalDestinationRec.Top + stageHeightAdjustment && this.y <= finalDestinationRec.Top + stageHeightAdjustment + 15 && this.x >= finalDestinationRec.Left && this.x <= finalDestinationRec.Right)
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
                if (counter % 3 == 0)
                {
                    this.dy += 1;
                }
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
                this.x += knockback;

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
                this.y -= knockup;

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
            return this.isShielding && this.counter < (this.shieldEffect.Length - 2) * this.shieldFrameLength;
        }

        // custom logic for defining the hitbox rectangle
        public override Rectangle getRectangle()
        {
            return new Rectangle(
                this.x - this.hitboxWidth * graphicsScaling / 2,
                this.y - this.hitboxHeight * graphicsScaling,
                this.hitboxWidth * graphicsScaling,
                this.hitboxHeight * 4
            );
        }

        public Rectangle getAttackHitboxRectangle()
        {
            return this.attack.getAttackHitbox(counterSprite, this.x, this.y);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (this.isShielding)
            {
                Rectangle shieldEffectRec = new Rectangle(
                    this.x - this.shieldEffect[0].Width * graphicsScaling / 2,
                    this.y + (-1 * this.hitboxHeight / 2 - this.shieldEffect[0].Height / 2) * graphicsScaling,
                    this.shieldEffect[0].Width * graphicsScaling,
                    this.shieldEffect[0].Height * 4
                );
                spriteBatch.Draw(this.shieldEffect[(counter / this.shieldFrameLength) % this.shieldEffect.Length], shieldEffectRec, Color.White);
            }
            //if (this.attack != null && Array.IndexOf(this.attack.attackFrame, counterSprite) > -1)
            //{
            //    spriteBatch.Draw(texture, this.getAttackHitboxRectangle(), Color.Green);
            //}
        }
    }
}
