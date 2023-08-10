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
    class Enemy : Sprite
    {
        //variables that control the character
        int player = 0;
        int counterSprite = 0;
        
        int[] playerDistance = new int[4];
        int closestPlayer;
        int dx;
        int dy;
        int knockback;
        int knockup;
        int counterSpriteMod;
        int counter;
        int damage;
        int jumpHeight;
        string direction = "Left";
        string character = "waddle";
        bool hurt;
        bool jump;
        bool attack;
        bool attack2;
        Rectangle[] playerPicBox = new Rectangle[4];

        //sprites
        int spriteRunLength;
        int spriteAttack1Length;
        int spriteAttack2Length;
        float widthScaling;
        Texture2D[] spriteRunLeft;
        Texture2D[] spriteRunRight;
        Texture2D[] spriteAttackLeft;
        Texture2D[] spriteAttackRight;
        Texture2D[] spriteSmashLeft;
        Texture2D[] spriteSmashRight;
        Texture2D spriteHurtLeft;
        Texture2D spriteHurtRight;

        //variables to control the stage
        int displayWidth;
        int displayHeight;
        Rectangle stage;
        int stageHeightAdjustment;

        Projectiles pichuLightning;

        ContentManager content;

        public Enemy(Rectangle newRectangle, Texture2D newTexture, int newPlayer, string newCharacter, ContentManager cnt, int disWidth, int disHeight, Rectangle stagerec, int stageHeight)
            : base(newTexture, newRectangle)
        {
            rectangle = newRectangle;
            player = newPlayer;
            content = cnt;
            displayHeight = disHeight;
            displayWidth = disWidth;
            stage = stagerec;
            stageHeightAdjustment = stageHeight;
            character = newCharacter;

            if (character == "waddle")
            {
                widthScaling = 0.9f;
                spriteRunLength = 8;
                counterSpriteMod = 6;
            }

            if (character == "Pichu")
            {
                widthScaling = 1.4f;
                spriteRunLength = 4;
                spriteAttack1Length = 5;
                spriteAttack2Length = 6;
                counterSpriteMod = 8;
            }

            spriteRunLeft = new Texture2D[spriteRunLength];
            spriteRunRight = new Texture2D[spriteRunLength];

            spriteHurtLeft = content.Load<Texture2D>(character + "/" + character.ToLower() + "HurtLeft");
            spriteHurtRight = content.Load<Texture2D>(character + "/" + character.ToLower() + "HurtRight");

            for (int i = 0; i < spriteRunLength; i++)
            {
                spriteRunLeft[i] = content.Load<Texture2D>(character + "/" + character.ToLower() + "RunLeft" + (i + 1));
                spriteRunRight[i] = content.Load<Texture2D>(character + "/" + character.ToLower() + "RunRight" + (i + 1));
            }

            if (spriteAttack1Length != 0)
            {
                spriteSmashLeft = new Texture2D[spriteAttack1Length];
                spriteSmashRight = new Texture2D[spriteAttack1Length];

                for (int i = 0; i < spriteAttack1Length; i++)
                {
                    spriteSmashRight[i] = content.Load<Texture2D>(character + "/" + character.ToLower() + "SmashRight" + (i + 1));
                    spriteSmashLeft[i] = content.Load<Texture2D>(character + "/" + character.ToLower() + "SmashLeft" + (i + 1));
                }
            }
            if (spriteAttack2Length != 0)
            {
                spriteAttackLeft = new Texture2D[spriteAttack2Length];
                spriteAttackRight = new Texture2D[spriteAttack2Length];

                for (int i = 0; i < spriteAttack2Length; i++)
                {
                    spriteAttackLeft[i] = content.Load<Texture2D>(character + "/" + character.ToLower() + "AttackLeft" + (i + 1));
                    spriteAttackRight[i] = content.Load<Texture2D>(character + "/" + character.ToLower() + "AttackRight" + (i + 1));
                }
            }

            rectangle.Width = (int)(displayWidth * widthScaling / 15);
            rectangle.Height = (int)((spriteRunLeft[0].Width / spriteRunLeft[0].Height) * widthScaling * (displayWidth / 15));
        }

        public void Update(GameTime gameTime)
        {
            counter++;
            if (counter % counterSpriteMod == 0)
            {
                counterSprite++;
            }

            DisplaySprite();
            gravity();

            if (hurt == false)
            {
                Tracking();
            }

            if (pichuLightning != null)
            {
                pichuLightning.Update(gameTime);
            }

            dx += knockback;
            rectangle.X += dx;
            rectangle.Y += dy;
            dy = 0;
            dx = 0;
        }

        //animate the character
        private void DisplaySprite()
        {
            if (hurt == true && direction == "Left")
            {
                texture = spriteHurtLeft;
            }
            else if (hurt == true && direction == "Right")
            {
                texture = spriteHurtRight;
            }
            else if (attack2 == true)
            {
                if (counterSprite < spriteAttack2Length)
                {
                    if (direction == "Left")
                    {
                        texture = spriteAttackLeft[counterSprite];
                    }
                    else if (direction == "Right")
                    {
                        texture = spriteAttackRight[counterSprite];
                    }
                }
                else
                {
                    attack2 = false;
                }
            }
            else if (attack == true)
            {
                if (counterSprite < spriteAttack1Length)
                {
                    if (direction == "Left")
                    {
                        texture = spriteSmashLeft[counterSprite];
                    }
                    else if (direction == "Right")
                    {
                        texture = spriteSmashRight[counterSprite];
                    }
                }
                else
                {
                    attack = false;
                }
            }
            else if (direction == "Left")
            {
                texture = spriteRunLeft[counterSprite % spriteRunLength];
            }
            else if (direction == "Right")
            {
                texture = spriteRunRight[counterSprite % spriteRunLength];
            }
        }

        //make the character fall when in the air
        private void gravity()
        {
            if ((rectangle.Bottom >= stage.Top + stageHeightAdjustment && rectangle.Bottom <= stage.Top + stageHeightAdjustment + 15 && rectangle.Left + (rectangle.Width / 2) >= stage.Left && rectangle.Right - (rectangle.Width / 2) <= stage.Right) == false)
            {
                dy += 3;
            }

            if (hurt == true && character == "waddle")
            {
                dy -= 6;
            }

            if (knockback > 0 && counter % 15 == 0)
            {
                knockback--;
            }

            if (knockback < 0 && counter % 15 == 0)
            {
                knockback++;
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

            if (rectangle.Bottom >= stage.Top + stageHeightAdjustment && rectangle.Bottom <= stage.Top + stageHeightAdjustment + 15 && rectangle.Left + (rectangle.Width / 2) >= stage.Left && rectangle.Right - (rectangle.Width / 2) <= stage.Right)
            {
                jumpHeight = 0;
                jump = false;
            }
        }

        //tell the character where to go
        private void Tracking()
        {
            if (character == "waddle")
            {
                for (int i = 0; i < playerPicBox.Length; i++)
                {
                    if (playerPicBox[i].IsEmpty == false)
                    {
                        playerDistance[i] = (rectangle.X + (rectangle.Width / 2)) - (playerPicBox[i].X + (playerPicBox[i].Width / 2));
                    }

                    if (Math.Abs(playerDistance[i]) > closestPlayer && i != player)
                    {
                        closestPlayer = playerDistance[i];
                    }

                    if (Math.Abs(dx) < 3)
                    {
                        if (closestPlayer < 60)
                        {
                            dx = 2;
                            direction = "Right";
                        }

                        else if (closestPlayer > -60)
                        {
                            dx = -2;
                            direction = "Left";
                        }
                    }
                }
            }
            if (character == "Pichu")
            {
                if (playerPicBox[player].Right < rectangle.Left)
                {
                    direction = "Left";
                    dx = -2;
                }
                if (playerPicBox[player].Left > rectangle.Right)
                {
                    direction = "Right";
                    dx = 2;
                }

                if (rectangle.Top > stage.Top - stageHeightAdjustment)
                {
                    jump = true;
                    jumpHeight = 9;
                }
            }
        }

        //get all players rectangles
        public void getRec(Rectangle Rec1, Rectangle Rec2, Rectangle Rec3, Rectangle Rec4)
        {
            playerPicBox[0] = Rec1;
            playerPicBox[1] = Rec2;
            playerPicBox[2] = Rec3;
            playerPicBox[3] = Rec4;
        }

        //send the characters rectangle
        public Rectangle setRec()
        {
            return rectangle;
        }

        //determine if character is dead
        public bool enemyIsDead()
        {
            if (rectangle.Y > displayHeight || rectangle.Right < 0 || rectangle.X > displayWidth || rectangle.Bottom < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //receive damage from other players
        public void takeDamage(int newDamage, int newknockback)
        {
            if (character == "waddle")
            {
                knockback = newknockback;
                hurt = true;
            }

            if (character == "Pichu")
            {
                damage += newDamage;
                knockback = newknockback * damage / 20;
                knockup = damage / 50;
            }
        }

        //check if the projectile has been created
        public bool checkProjectile()
        {
            if (pichuLightning == null)
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
            return pichuLightning.getRectangle();
        } 

        //send if pichu is using an attack
        public bool isAttacking()
        {
            if (attack == true && counter % counterSpriteMod == 0 && counterSprite == 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //check if pichu is using attack 1
        public void getAttack1()
        {
            if (attack == false && attack2 == false)
            {
                attack = true;
                counterSprite = 0;
            }
        }

        //Check if pichu is using attack 2
        public void getAttack2()
        {
            if (attack == false && attack2 == false)
            {
                attack2 = true;
                counterSprite = 0;
                pichuLightning = new Projectiles(rectangle, texture, "left", player, "Pichu", content);
            }
        }

        //Display any projectiles on the screen
        public void Draw2(SpriteBatch spriteBatch)
        {
            if (pichuLightning != null)
            {
                pichuLightning.Draw(spriteBatch);
                pichuLightning.Draw2(spriteBatch);
            }
        }
    }
}
