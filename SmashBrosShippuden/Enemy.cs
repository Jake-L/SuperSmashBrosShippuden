//Jake Loftus
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SmashBrosShippuden
{
    class Enemy : Character
    {
        // variables specific to AI characters
        int[] playerDistance = new int[4];
        int closestPlayer;
        Rectangle[] playerPicBox = new Rectangle[4];

        public Enemy(int x, int y, string newDirection, int newPlayer, string newCharacter, int disWidth, int disHeight, StageObject[] stageObjects, bool bot)
            : base(x, y, newDirection, newPlayer, newCharacter, disWidth, disHeight, stageObjects, bot)
        {
            // all logic handled in base class
        }

        public void Update(GameTime gameTime)
        {
            if (knockback == 0)
            {
                Tracking();
            }

            base.Update(gameTime);
        }

        // tell the character where to go
        // this is where the AI makes decisions
        private void Tracking()
        {
            // the waddle dee casually walks towards other players
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
            // Pichu chases it's ally
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

                if (rectangle.Top > stageObjects[0].y)
                {
                    dy = -5;
                }
            }
        }

        // get all players rectangles
        // used so the AI can "see" other players
        public void getRec(Rectangle[] playerPicBox)
        {
            this.playerPicBox = playerPicBox;
        }

        //send the characters rectangle
        public Rectangle setRec()
        {
            return rectangle;
        }
    }
}
