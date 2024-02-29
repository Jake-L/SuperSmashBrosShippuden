using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace SmashBrosShippuden
{
    internal class Attack
    {
        private readonly string character;
        public readonly AttackType attackType;
        public readonly string direction;
        public int spriteLength = 0;
        public int[] attackFrame;
        public int hitboxWidth;
        public int hitboxHeight;
        public int[] hitboxXOffset;
        public int[] hitboxYOffset;
        public int damage;
        public int knockback;
        public bool createProjectile = false;
        protected int graphicsScaling = 4;

        public Attack(string character, AttackType attackType, string direction)
        {
            this.character = character;
            this.attackType = attackType;
            this.direction = direction;
            this.Initialize();
        }

        private void Initialize()
        {
            if (character == "Mario")
            {
                if (this.attackType == AttackType.Jab)
                {
                    this.damage = 10;
                    this.knockback = 2;
                    this.spriteLength = 8;
                    this.attackFrame = new int[] { 5 };
                }
                else if (this.attackType == AttackType.Special)
                {
                    this.spriteLength = 5;
                    this.createProjectile = true; 
                    this.attackFrame = new int[] { 4 };
                }
            }
            if (character == "Luigi")
            {
                if (this.attackType == AttackType.Jab)
                {
                    this.damage = 10;
                    this.knockback = 2;
                    this.spriteLength = 7;
                    this.attackFrame = new int[] { 5 };
                }
                else if (this.attackType == AttackType.Special)
                {
                    this.spriteLength = 5;
                    this.createProjectile = true;
                    this.attackFrame = new int[] { 4 };
                }
            }
            if (character == "Mewtwo")
            {
                if (this.attackType == AttackType.Jab)
                {
                    this.damage = 13;
                    this.knockback = 2;
                    this.spriteLength = 4;
                    this.attackFrame = new int[] { 3 };
                }
                else if (this.attackType == AttackType.Special)
                {
                    this.spriteLength = 4;
                    this.createProjectile = true;
                    this.attackFrame = new int[] { 3 };
                }
            }
            if (character == "Sonic")
            {
                if (this.attackType == AttackType.Jab)
                {
                    this.damage = 7;
                    this.knockback = 1;
                    this.spriteLength = 5;
                    this.attackFrame = new int[] { 3 };
                    this.hitboxHeight = 8;
                    this.hitboxWidth = 10;
                    this.hitboxYOffset = new int[] { 17 };
                    this.hitboxXOffset = new int[] { 18 };
                }
                else if (this.attackType == AttackType.Special)
                {
                    this.damage = 14;
                    this.knockback = -2;
                    this.spriteLength = 9;
                    this.attackFrame = new int[] { 4 };
                    this.hitboxHeight = 8;
                    this.hitboxWidth = 36;
                    this.hitboxYOffset = new int[] { 33 };
                    this.hitboxXOffset = new int[] { 0 };
                }
            }
            if (character == "Shadow")
            {
                if (this.attackType == AttackType.Jab)
                {
                    this.damage = 13;
                    this.knockback = 2;
                    this.spriteLength = 9;
                    this.attackFrame = new int[] { 3 };
                    this.hitboxHeight = 12;
                    this.hitboxWidth = 10;
                    this.hitboxYOffset = new int[] { 25 };
                    this.hitboxXOffset = new int[] { 16 };
                }
                else if (this.attackType == AttackType.Special)
                {
                    this.damage = 2;
                    this.knockback = 1;
                    this.spriteLength = 17;
                    this.attackFrame = new int[] { 5, 7, 9 };
                    this.hitboxHeight = 44;
                    this.hitboxWidth = 44;
                    this.hitboxYOffset = new int[] { 21, 21, 21 };
                    this.hitboxXOffset = new int[] { 0, 0, 0 };
                }
            }
            if (character == "Knuckles")
            {
                if (this.attackType == AttackType.Jab)
                {
                    this.damage = 9;
                    this.knockback = 2;
                    this.spriteLength = 6;
                    this.attackFrame = new int[] { 3 };
                }
                else if (this.attackType == AttackType.Special)
                {
                    this.damage = 13;
                    this.knockback = 1;
                    this.spriteLength = 5;
                    this.attackFrame = new int[] { 2 };
                }
            }
            if (character == "Blastoise")
            {
                if (this.attackType == AttackType.Jab)
                {
                    this.damage = 8;
                    this.knockback = 2;
                    this.spriteLength = 4;
                    this.attackFrame = new int[] { 1 };
                }
                else if (this.attackType == AttackType.Special)
                {
                    this.spriteLength = 3;
                    this.createProjectile = true;
                    this.attackFrame = new int[] { 0, 1, 2, 3 };
                }
            }
            if (character == "Pichu")
            {
                if (this.attackType == AttackType.Jab)
                {
                    this.damage = 8;
                    this.knockback = 1;
                    this.spriteLength = 5;
                    this.attackFrame = new int[] { 2 };
                }
                else if (this.attackType == AttackType.Special)
                {
                    this.spriteLength = 5;
                    this.createProjectile = true;
                    this.attackFrame = new int[] { 1 };
                }
            }
            if (character == "Charizard")
            {
                if (this.attackType == AttackType.Jab)
                {
                    this.damage = 10;
                    this.knockback = 2;
                    this.spriteLength = 4;
                    this.attackFrame = new int[] { 3 };
                    this.hitboxHeight = 16;
                    this.hitboxWidth = 8;
                    this.hitboxYOffset = new int[] { 13 };
                    this.hitboxXOffset = new int[] { 16 };
                }
                else if (this.attackType == AttackType.Special)
                {
                    this.spriteLength = 4;
                    this.createProjectile = true;
                    this.attackFrame = new int[] { 2 };
                }
            }
            if (character == "Kirby")
            {
                if (this.attackType == AttackType.Jab)
                {
                    this.damage = 7;
                    this.knockback = 1;
                    this.spriteLength = 4;
                    this.attackFrame = new int[] { 3 };
                    this.hitboxHeight = 8;
                    this.hitboxWidth = 8;
                    this.hitboxYOffset = new int[] { 24 };
                    this.hitboxXOffset = new int[] { 15 };
                }
                else if (this.attackType == AttackType.Special)
                {
                    this.damage = 9;
                    this.knockback = 2;
                    this.spriteLength = 5;
                    this.attackFrame = new int[] { 3 };
                    this.hitboxHeight = 40;
                    this.hitboxWidth = 20;
                    this.hitboxYOffset = new int[] { 20 };
                    this.hitboxXOffset = new int[] { 16 };
                }
            }
            if (character == "Metaknight")
            {
                if (this.attackType == AttackType.Jab)
                {
                    this.damage = 9;
                    this.knockback = 1;
                    this.spriteLength = 9;
                    this.attackFrame = new int[] { 3 };
                }
                else if (this.attackType == AttackType.Special)
                {
                    this.damage = 13;
                    this.knockback = 2;
                    this.spriteLength = 8;
                    this.attackFrame = new int[] { 4 };
                }
            }
            if (character == "King")
            {
                if (this.attackType == AttackType.Jab)
                {
                    this.damage = 15;
                    this.knockback = 3;
                    this.spriteLength = 6;
                    this.attackFrame = new int[] { 5 };
                    this.hitboxHeight = 50;
                    this.hitboxWidth = 22;
                    this.hitboxYOffset = new int[] { 32 };
                    this.hitboxXOffset = new int[] { 42 };
                }
                else if (this.attackType == AttackType.Special)
                {
                    this.spriteLength = 5;
                    this.attackFrame = new int[] { 3 };
                }
            }
            if (character == "Link")
            {
                if (this.attackType == AttackType.Jab)
                {
                    this.damage = 8;
                    this.knockback = 1;
                    this.spriteLength = 7;
                    this.attackFrame = new int[] { 4 };
                }
                else if (this.attackType == AttackType.Special)
                {
                    this.damage = 9;
                    this.knockback = 2;
                    this.spriteLength = 8;
                    this.attackFrame = new int[] { 3, 7 };
                }
            }

            if (character == "Shrek")
            {
                if (this.attackType == AttackType.Jab)
                {
                    this.damage = 10;
                    this.knockback = 1;
                    this.spriteLength = 6;
                    this.attackFrame = new int[] { 3 };
                }
                else if (this.attackType == AttackType.Special)
                {
                    this.damage = 15;
                    this.knockback = 2;
                    this.spriteLength = 8;
                    this.attackFrame = new int[] { 4 };
                }
            }

            if (character == "Sasuke")
            {
                if (this.attackType == AttackType.Jab)
                {
                    this.damage = 6;
                    this.knockback = 1;
                    this.spriteLength = 4;
                    this.attackFrame = new int[] { 2 };
                    this.hitboxHeight = 6;
                    this.hitboxWidth = 10;
                    this.hitboxYOffset = new int[] { 23 };
                    this.hitboxXOffset = new int[] { 13 };
                }
                else if (this.attackType == AttackType.SideSmash)
                {
                    this.damage = 10;
                    this.knockback = 2;
                    this.spriteLength = 4;
                    this.attackFrame = new int[] { 2 };
                    this.hitboxHeight = 8;
                    this.hitboxWidth = 12;
                    this.hitboxYOffset = new int[] { 23 };
                    this.hitboxXOffset = new int[] { 12 };
                }
                else if (this.attackType == AttackType.Special)
                {
                    this.spriteLength = 9;
                    this.attackFrame = new int[] { 4 };
                    this.createProjectile = true;
                }
            }
        }

        public Rectangle getAttackHitbox(int currentFrame, int playerX, int playerY)
        {
            int frameIndex = Array.IndexOf(this.attackFrame, currentFrame);
            if (frameIndex == -1)
            {
                throw new Exception("getAttackHitbox called outside of attack frame!");
            }
            else
            {
                int xAdjustment;
                if (this.direction == "Right")
                {
                    xAdjustment = (this.hitboxXOffset[frameIndex] - this.hitboxWidth / 2) * graphicsScaling;
                }
                else
                {
                    xAdjustment = (-1 * this.hitboxXOffset[frameIndex] - this.hitboxWidth / 2) * graphicsScaling;
                }

                return new Rectangle(
                    playerX + xAdjustment,
                    playerY - (this.hitboxYOffset[frameIndex] + this.hitboxHeight / 2) * graphicsScaling,
                    this.hitboxWidth * graphicsScaling,
                    this.hitboxHeight * graphicsScaling
                );
            }
        }

        public void update()
        {

        }
    }
}
