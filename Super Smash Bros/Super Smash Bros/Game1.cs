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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GamePadState pad1;
        GamePadState pad2;
        GamePadState pad3;
        GamePadState pad4;

        //variables to make the game work
        string[] character = new string[4]; 
        string[] tempCharacter = new string[4];
        string[] direction = new string[4] {"Right", "Right", "Left", "Left"};
        string[] charNum; //= new string[10];
        int[] attackType = new int[4];
        int numberPlayers = 1;
        int[] knockback = new int[4];
        int[] knockup = new int[4];
        int[] dx = new int[4];
        int[] dy = new int[4];
        int counter = 0;
        int counterAnimation = 0;
        int[] midPointX = new int[4];
        int[] midPointY = new int[4];
        int[] picBoxWidthScaling = new int[4];
        int[] playerLastHit = new int[4];
        int[] score = new int[4];
        int damageDealt;
        int knockbackAbility;
        int stage = 0;
        int stageHeightAdjustment;
        int song;
        int stagetimer = 0;
        Random NumberGenerator = new Random();
        bool player2Bot = false;
        bool[] charizardx = new bool[4] { false, false, false, false };


        //background images
        Texture2D[] finalDestination = new Texture2D[5];        
        Texture2D background;
        Rectangle finalDestinationRec;
        Rectangle backgroundRec;

        //create the character select screen        
        Texture2D[] characterIcons; //= new Texture2D[10];
        Rectangle[] characterIconsRectangle; //= new Rectangle[10];
        Texture2D[] charSelectTokens = new Texture2D[4];
        Rectangle[] charSelectTokensRec = new Rectangle[4];
        Texture2D[] characterSplash; //= new Texture2D[10];
        Rectangle[] characterSplashRec = new Rectangle[4];
        Texture2D[] characterSelect = new Texture2D[4];
        Rectangle[] characterSelectRec = new Rectangle[4];

        //create the stage select screen
        Texture2D[] stageIcons = new Texture2D[6];
        Rectangle[] stageIconsRec = new Rectangle[6];

        //Displaying the characters
        Rectangle[] PlayerPicBox = new Rectangle[4];
        Rectangle[] ProjectPicBox = new Rectangle[4];
        Player[] PlayerClass = new Player[4];

        //create waddledees
        Enemy[] waddle = new Enemy[12];
        Enemy[] pichuBro = new Enemy[4];
        Texture2D waddleSprite;

        //display settings
        int displayWidth;
        int displayHeight;
        
        /*
        //opening trailer
        Video video;
        VideoPlayer mediaPlayer; */

        //background music
        Song[] backgroundMusic = new Song[3];

        //fonts
        SpriteFont font1;

        //write to text files
        System.IO.StreamWriter file;
        System.IO.StreamReader read;
        int plays = 0;
        string address = "";
                
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        //setting screen size
        protected override void Initialize()
        {
            //Set the size of the window
            background = this.Content.Load<Texture2D>("background");
            this.graphics.PreferredBackBufferHeight = 720; // (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / (double)background.Width) * (double)background.Height);
            this.graphics.PreferredBackBufferWidth = 1280; //GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            this.graphics.ApplyChanges();  

            //Display settings
            displayWidth = GraphicsDevice.Viewport.Width;
            displayHeight = GraphicsDevice.Viewport.Height;
            
            //load fonts
            font1 = Content.Load<SpriteFont>("SpriteFont1");

            //take input from controllers
            pad1 = GamePad.GetState(PlayerIndex.One);
            pad2 = GamePad.GetState(PlayerIndex.Two);
            pad3 = GamePad.GetState(PlayerIndex.Three);
            pad4 = GamePad.GetState(PlayerIndex.Four);

            //check how many people are playing
            if (pad2.IsConnected)
            {
                numberPlayers++;
            }

            if (pad3.IsConnected)
            {
                numberPlayers++;
            }

            if (pad4.IsConnected)
            {
                numberPlayers++;
            }

            song = NumberGenerator.Next(0, 3);

            //prepare text writing variables
            address = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            
            if (System.IO.File.Exists(address + "\\smashbros.txt"))
            {
                read = new System.IO.StreamReader(address + "\\smashbros.txt");

                plays = Convert.ToInt32(read.ReadLine());

                read.Close();
            }

            //set variables for character select screen
            charNum = new string[14];
            charNum[0] = "Mario";
            charNum[1] = "Luigi";
            charNum[2] = "Charizard";
            charNum[3] = "Mewtwo";
            charNum[4] = "Blastoise";
            charNum[5] = "Pichu";
            charNum[6] = "Kirby";
            charNum[7] = "Metaknight";
            charNum[8] = "King";
            charNum[9] = "Sonic";
            charNum[10] = "Knuckles";
            charNum[11] = "Shadow";
            charNum[12] = "Link";
            charNum[13] = "Shrek";

            characterIcons = new Texture2D[charNum.Length];
            characterIconsRectangle = new Rectangle[charNum.Length];
            characterSplash = new Texture2D[charNum.Length];

            base.Initialize();
        }

        //loading images
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            for (int i = 0; i < charNum.Length; i++)
            {
                //loading character select icons
                characterIcons[i] = this.Content.Load<Texture2D>("Icon/" + charNum[i].ToLower() + "Icon");

                //loading splash arts
                characterSplash[i] = this.Content.Load<Texture2D>("Splash/" + charNum[i].ToLower() + "Art");
            }

            //display a token for each player to choose his character
            charSelectTokens[0] = this.Content.Load<Texture2D>("Icon/player1Icon");
            charSelectTokens[1] = this.Content.Load<Texture2D>("Icon/player2Icon");
            charSelectTokens[2] = this.Content.Load<Texture2D>("Icon/player3Icon");
            charSelectTokens[3] = this.Content.Load<Texture2D>("Icon/player4Icon");
            
            //rectangles to display the characters artwork
            characterSplashRec[0] = new Rectangle(50, (displayHeight * 3) / 5, characterSplash[0].Width / 2, characterSplash[0].Height / 2);
            characterSplashRec[1] = new Rectangle(((displayWidth) / 5) + 50, (displayHeight * 3) / 5, characterSplash[0].Width / 2, characterSplash[0].Height / 2);
            characterSplashRec[2] = new Rectangle(((displayWidth * 2) / 5) + 50, (displayHeight * 3) / 5, characterSplash[0].Width / 2, characterSplash[0].Height / 2);
            characterSplashRec[3] = new Rectangle(((displayWidth * 3) / 5) + 50, (displayHeight * 3) / 5, characterSplash[0].Width / 2, characterSplash[0].Height / 2);

            //loading character select backgrounds
            for (int i = 0; i < characterSelect.Length; i++) 
            {
                characterSelect[i] = this.Content.Load<Texture2D>("characterselectbox" + (i + 1));
                characterSelectRec[i] = new Rectangle(((displayWidth * i) / 5) + 20, (displayHeight * 3) / 5, characterSelect[i].Width, characterSelect[i].Height);
            }

            //create rectangles for the icons
            characterIconsRectangle[0] = new Rectangle(200, 50, displayWidth / 8, (int)((displayWidth / 8) * ((float)characterIcons[0].Height / characterIcons[0].Width)));
            characterIconsRectangle[1] = new Rectangle(200, characterIconsRectangle[0].Bottom, displayWidth / 8, (int)((displayWidth / 8) * ((float)characterIcons[0].Height / characterIcons[0].Width)));
            characterIconsRectangle[2] = new Rectangle(200, characterIconsRectangle[1].Bottom, displayWidth / 8, (int)((displayWidth / 8) * ((float)characterIcons[0].Height / characterIcons[0].Width)));

            for (int i = 3; i < charNum.Length; i++)
            {
                characterIconsRectangle[i] = new Rectangle(characterIconsRectangle[i - 3].Right, characterIconsRectangle[i % 3].Top, displayWidth / 8, (int)((displayWidth / 8) * ((float)characterIcons[0].Height / characterIcons[0].Width)));
            }

            //create rectangles for the character select tokens
            charSelectTokensRec[0] = new Rectangle(100, 100, charSelectTokens[0].Width * 2, charSelectTokens[0].Height * 2);
            charSelectTokensRec[1] = new Rectangle(100, 150, charSelectTokens[1].Width * 2, charSelectTokens[1].Height * 2);
            charSelectTokensRec[2] = new Rectangle(100, 200, charSelectTokens[2].Width * 2, charSelectTokens[2].Height * 2);
            charSelectTokensRec[3] = new Rectangle(100, 250, charSelectTokens[3].Width * 2, charSelectTokens[3].Height * 2);    
        
            //create icons for the stage select screen
            stageIcons[0] = this.Content.Load<Texture2D>("stageicon1");
            stageIcons[1] = this.Content.Load<Texture2D>("stageicon2");
            stageIcons[2] = this.Content.Load<Texture2D>("stageicon3");
            stageIcons[3] = this.Content.Load<Texture2D>("stageicon4");
            stageIcons[4] = this.Content.Load<Texture2D>("stageicon5");
            stageIcons[5] = this.Content.Load<Texture2D>("stageicon6");

            //create rectangles for the icons
            stageIconsRec[0] = new Rectangle(200, 100, stageIcons[0].Width, stageIcons[0].Height);
            stageIconsRec[1] = new Rectangle(stageIconsRec[0].Right, 100, stageIcons[0].Width, stageIcons[0].Height);
            stageIconsRec[2] = new Rectangle(200, stageIconsRec[0].Bottom, stageIcons[0].Width, stageIcons[0].Height);
            stageIconsRec[3] = new Rectangle(stageIconsRec[0].Right, stageIconsRec[0].Bottom, stageIcons[0].Width, stageIcons[0].Height);
            stageIconsRec[4] = new Rectangle(stageIconsRec[1].Right, stageIconsRec[0].Top, stageIcons[0].Width, stageIcons[0].Height);
            stageIconsRec[5] = new Rectangle(stageIconsRec[3].Right, stageIconsRec[0].Bottom, stageIcons[0].Width, stageIcons[0].Height);

            //create the stage rectangle
            finalDestination[0] = this.Content.Load<Texture2D>("background1");
            finalDestinationRec = new Rectangle(displayWidth / 10, (displayHeight * 2) / 5, displayWidth - (displayWidth / 5), (int)((displayWidth - (displayWidth / 5)) * ((float)finalDestination[0].Height / finalDestination[0].Width)));
            backgroundRec = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            //creating the waddle dee
            waddleSprite = this.Content.Load<Texture2D>("waddle/waddleRunLeft1");

            /*
            //trailer
            video = this.Content.Load<Video>("smashtrailer");
            mediaPlayer = new VideoPlayer();
            mediaPlayer.Play(video); */
        }
        
        //delete things when program closes (DONT TOUCH)
        protected override void UnloadContent()
        {

        }

        //this is where everything is run and methods are called
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //take input from controllers
            pad1 = GamePad.GetState(PlayerIndex.One);
            pad2 = GamePad.GetState(PlayerIndex.Two);
            pad3 = GamePad.GetState(PlayerIndex.Three);
            pad4 = GamePad.GetState(PlayerIndex.Four);

            //show character select screen
            if (character[0] == null || stage == 0)
            {   
                /*
                if (mediaPlayer.State == MediaState.Playing && pad1.Buttons.Start == ButtonState.Pressed)
                {
                    mediaPlayer.Stop();
                }
                if (mediaPlayer.State == MediaState.Stopped)
                {
                    
                } */
                characterSelectMethod();
            }

            //run the game when timer is greater than zero
            if (character[0] != null && stage != 0 && counter < 7200)
            {
                counter++;

                for (int i = 0; i < 4; i++)
                {
                    if (character[i] != null)
                    {
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
                                    waddle[j] = new Enemy(new Rectangle(PlayerPicBox[i].X + (PlayerPicBox[i].Width / 2), PlayerPicBox[i].Y + (PlayerPicBox[i].Height / 2), 100, 100), null, i, "waddle", Content, displayWidth, displayHeight, finalDestinationRec, stageHeightAdjustment);
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
                if (counter == 1)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (character[i] != null && picBoxWidthScaling[i] == 0)
                        {
                            if (character[i] == "Knuckles" || character[i] == "Mewtwo" || character[i] == "King" || character[i] == "Sonic" || character[i] == "Pichu" || character[i] == "Charizard" || character[i] == "CharizardX")
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
            }

            else if (counter >= 7200)
            {
                //write to a text file that another game has been played
                if (counter == 7200)
                {
                    plays += 1;

                    file = new System.IO.StreamWriter(address + "\\smashbros.txt");
                    file.WriteLine(plays);
                    file.Close();

                    counter++;
                }
                //if the game is over, and they press start, restart the game
                if (pad1.Buttons.Start == ButtonState.Pressed)
                {
                    character[0] = null;
                    stage = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        PlayerClass[i] = null;
                        tempCharacter[i] = null;
                        charSelectTokensRec[i].Y = 100;
                        charSelectTokensRec[i].X = 100 + (i * 50);
                        score[i] = 0;
                        //mediaPlayer.Stop();
                    }
                    counter = 0;
                }
            }
            
            if (counter % 5 == 0)
                {
                    counterAnimation++;
                }


            if (MediaPlayer.State != MediaState.Playing && stage != 0)
            {
                song += 1;

                if (song >= backgroundMusic.Length)
                {
                    song = 0;
                }

                MediaPlayer.Play(backgroundMusic[song]);
            }
        
            //control the bot
            if (player2Bot == true)
            {
                controlBot();
            }

            base.Update(gameTime);
        }

        //run the character select screen
        private void characterSelectMethod()
        {
            //get input for player 1
            if (pad1.ThumbSticks.Left.X > 0)
            {
                dx[0] = 4;
            }

            if (pad1.ThumbSticks.Left.X < 0)
            {
                dx[0] = -4;
            }

            if (pad1.ThumbSticks.Left.Y > 0)
            {
                dy[0] = -4;
            }

            if (pad1.ThumbSticks.Left.Y < 0)
            {
                dy[0] = 4;
            }

            if (pad1.Buttons.RightShoulder == ButtonState.Pressed)
            {
                charizardx[0] = true;
            }

            if (pad1.Buttons.LeftShoulder == ButtonState.Pressed)
            {
                charizardx[0] = false;
            }

            //get input for player 2
            if (pad2.ThumbSticks.Left.X > 0)
            {
                dx[1] = 4;
            }

            if (pad2.ThumbSticks.Left.X < 0)
            {
                dx[1] = -4;
            }

            if (pad2.ThumbSticks.Left.Y > 0)
            {
                dy[1] = -4;
            }

            if (pad2.ThumbSticks.Left.Y < 0)
            {
                dy[1] = 4;
            }

            if (pad2.Buttons.RightShoulder == ButtonState.Pressed)
            {
                charizardx[1] = true;
            }

            if (pad2.Buttons.LeftShoulder == ButtonState.Pressed)
            {
                charizardx[1] = false;
            }

            //get input for player 3
            if (pad3.ThumbSticks.Left.X > 0)
            {
                dx[2] = 4;
            }

            if (pad3.ThumbSticks.Left.X < 0)
            {
                dx[2] = -4;
            }

            if (pad3.ThumbSticks.Left.Y > 0)
            {
                dy[2] = -4;
            }

            if (pad3.ThumbSticks.Left.Y < 0)
            {
                dy[2] = 4;
            }

            //get input for player 4
            if (pad4.ThumbSticks.Left.X > 0)
            {
                dx[3] = 4;
            }

            if (pad4.ThumbSticks.Left.X < 0)
            {
                dx[3] = -4;
            }

            if (pad4.ThumbSticks.Left.Y > 0)
            {
                dy[3] = -4;
            }

            if (pad4.ThumbSticks.Left.Y < 0)
            {
                dy[3] = 4;
            }

            //check if the player has selected a character
            for (int i = 0; i < numberPlayers; i++)
            {
                midPointX[i] = charSelectTokensRec[i].Left + (charSelectTokensRec[i].Width / 2);
                midPointY[i] = charSelectTokensRec[i].Top + (charSelectTokensRec[i].Height / 2);

                charSelectTokensRec[i].X += dx[i];
                charSelectTokensRec[i].Y += dy[i];

                dx[i] = 0;
                dy[i] = 0;

                //check if the players token is over a character
                if (character[0] == null)
                {
                    for (int j = 0; j < charNum.Length; j++)
                    {
                        if (charSelectTokensRec[i].Intersects(characterIconsRectangle[j]))
                        {
                            if (midPointX[i] > characterIconsRectangle[j].Left && midPointX[i] < characterIconsRectangle[j].Right && midPointY[i] > characterIconsRectangle[j].Top && midPointY[i] < characterIconsRectangle[j].Bottom)
                            {
                                tempCharacter[i] = charNum[j];
                            }

                            if (tempCharacter[i] == "Charizard" && charizardx[i] == true)
                            {
                                tempCharacter[i] = "CharizardX";
                            }
                        }
                    }

                    if (pad1.Buttons.Start == ButtonState.Pressed)
                    {
                        //lock in their character when they press start
                        for (int k = 0; k < 4; k++)
                        {
                            if (tempCharacter[k] != null)
                            {
                                character[k] = tempCharacter[k];

                            }
                        }
                    }
                }

                else if (stage == 0)
                {
                    stagetimer++;

                    for (int j = 0; j < stageIconsRec.Length; j++)
                    {
                        if (midPointX[0] > stageIconsRec[j].Left && midPointX[0] < stageIconsRec[j].Right && midPointY[0] > stageIconsRec[j].Top && midPointY[0] < stageIconsRec[j].Bottom && pad1.Buttons.Start == ButtonState.Pressed && stagetimer > 60)
                        {
                            stage = j + 1;

                            for (int k = 0; k < finalDestination.Length; k++)
                            {
                                //loading final destination stage
                                if (stage == 1)
                                {
                                    finalDestination[k] = this.Content.Load<Texture2D>("background" + (k + 1));
                                    background = this.Content.Load<Texture2D>("background");
                                }
                                //loading shrek stage
                                if (stage == 2)
                                {
                                    finalDestination[k] = this.Content.Load<Texture2D>("stageShrek");
                                    background = this.Content.Load<Texture2D>("backgroundshrek");
                                }
                                //loading pokemon stadium 1 stage
                                if (stage == 3)
                                {
                                    finalDestination[k] = this.Content.Load<Texture2D>("backgroundpokemon2");
                                    background = this.Content.Load<Texture2D>("backgroundpokemon3");
                                }
                                //loading pokemon stadium 2 stage
                                if (stage == 4)
                                {
                                    finalDestination[k] = this.Content.Load<Texture2D>("backgroundpokemon");
                                    background = this.Content.Load<Texture2D>("backgroundpokemon3");
                                }
                                //loading sonic stage
                                if (stage == 5)
                                {
                                    finalDestination[k] = this.Content.Load<Texture2D>("stageSonic");
                                    background = this.Content.Load<Texture2D>("backgroundsonic");
                                }
                                //loading mario stage
                                if (stage == 6)
                                {
                                    finalDestination[k] = this.Content.Load<Texture2D>("stageMario");
                                    background = this.Content.Load<Texture2D>("backgroundmario");
                                    stageHeightAdjustment = (displayHeight / 5) + 10;
                                }
                            }

                            finalDestinationRec = new Rectangle(displayWidth / 10, ((displayHeight * 2) / 5) + stageHeightAdjustment, displayWidth - (displayWidth / 5), (int)((displayWidth - (displayWidth / 5)) * ((float)finalDestination[0].Height / finalDestination[0].Width)));
                            stageHeightAdjustment = 0;

                            //loading background music
                            if (stage == 1)
                            {
                                backgroundMusic[0] = this.Content.Load<Song>("backgroundMusic1");
                                backgroundMusic[1] = this.Content.Load<Song>("backgroundMusic2");
                                backgroundMusic[2] = this.Content.Load<Song>("backgroundMusic3");
                                stageHeightAdjustment = finalDestinationRec.Height / 5;
                            }
                            if (stage == 2)
                            {
                                backgroundMusic[0] = this.Content.Load<Song>("musicshrek1");
                                backgroundMusic[1] = this.Content.Load<Song>("musicshrek1");
                                backgroundMusic[2] = this.Content.Load<Song>("musicshrek1");
                            }
                            if (stage == 3)
                            {
                                backgroundMusic[0] = this.Content.Load<Song>("musicpokemon1");
                                backgroundMusic[1] = this.Content.Load<Song>("musicpokemon1");
                                backgroundMusic[2] = this.Content.Load<Song>("musicpokemon1");
                                stageHeightAdjustment = finalDestinationRec.Height / 5;
                            }
                            if (stage == 4)
                            {
                                backgroundMusic[0] = this.Content.Load<Song>("musicpokemon4");
                                backgroundMusic[1] = this.Content.Load<Song>("musicpokemon5");
                                backgroundMusic[2] = this.Content.Load<Song>("musicpokemon6");
                                stageHeightAdjustment = finalDestinationRec.Height / 5;
                            }
                            if (stage == 5)
                            {
                                backgroundMusic[0] = this.Content.Load<Song>("musicsonic1");
                                backgroundMusic[1] = this.Content.Load<Song>("musicsonic1");
                                backgroundMusic[2] = this.Content.Load<Song>("musicsonic1");
                            }
                            if (stage == 6)
                            {
                                backgroundMusic[0] = this.Content.Load<Song>("musicmario1");
                                backgroundMusic[1] = this.Content.Load<Song>("musicmario2");
                                backgroundMusic[2] = this.Content.Load<Song>("musicmario3");
                            }

                            //send the information about their character to the class
                            for (int k = 0; k < 4; k++)
                            {
                                if (character[k] != null)
                                {
                                    PlayerClass[k] = new Player(PlayerPicBox[k], null, direction[k], k, character[k], Content, displayWidth, displayHeight, finalDestinationRec, stageHeightAdjustment, false);

                                    if (character[k] == "Pichu")
                                    {
                                        pichuBro[k] = new Enemy(new Rectangle(PlayerPicBox[k].X - 50, PlayerPicBox[k].Y, PlayerPicBox[k].Width, PlayerPicBox[k].Height), null, k, character[k], Content, displayWidth, displayHeight, finalDestinationRec, stageHeightAdjustment);
                                    }
                                }
                            }

                            if (character[0] != null && character[1] == null && character[2] == null && character[3] == null)
                            {
                                player2Bot = true;
                                character[1] = "Mario";
                                PlayerClass[1] = new Player(PlayerPicBox[1], null, direction[1], 1, character[1], Content, displayWidth, displayHeight, finalDestinationRec, stageHeightAdjustment, true);
                            }
                        }
                    }
                }
            }
        }
            
        //massive damage
        public void damage()
        {
            for (int i = 0; i < 4; i++)
            {
                if (PlayerPicBox[i].IsEmpty == false)
                {
                    attackType[i] = PlayerClass[i].attackType();

                    //deal different amounts of damage based on who the character is
                    if (attackType[i] == 1 || (attackType[i] == 2 && (character[i] == "Seiar" || character[i] == "Knuckles" || character[i] == "Kirby" || character[i] == "Link")))
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
                                waddle[i].takeDamage(100, -10);
                            }
                            else if (intersection(PlayerPicBox[j], waddle[i].setRec(), 0, "Right"))//(PlayerPicBox[j].Right < waddle[i].setRec().Right && PlayerPicBox[j].Right > waddle[i].setRec().Left && direction[j] == "Right") 
                            {
                                waddle[i].takeDamage(100, 10);
                            }
                        }

                        //check if anyone has hit a waddle with a projectile
                        else if (j != Math.Floor(i / 3.0) && ProjectPicBox[j].IsEmpty == false && ProjectPicBox[j].Intersects(waddle[i].setRec()))
                        {
                            if (intersection(ProjectPicBox[j], waddle[i].setRec(), 0, "Left")) //(ProjectPicBox[j].Left < waddle[i].setRec().Right && ProjectPicBox[j].Left > waddle[i].setRec().Left)
                            {
                                waddle[i].takeDamage(100, -10);
                                PlayerClass[j].destroyProjectileRec();
                            }
                            else if (intersection(ProjectPicBox[j], waddle[i].setRec(), 0, "Right"))//(ProjectPicBox[j].Right < waddle[i].setRec().Right && ProjectPicBox[j].Right > waddle[i].setRec().Left)
                            {
                                waddle[i].takeDamage(100, 10);
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
                    if (waddle[i].enemyIsDead() == true)
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
                                pichuBro[i].takeDamage(10, -2);
                            }
                            else if (intersection(PlayerPicBox[j], pichuBro[i].setRec(), 0, "Right") && direction[j] == "Right")
                            {
                                pichuBro[i].takeDamage(10, 2);
                            }
                        }

                        //check if anyone has hit a pichuBro with a projectile
                        else if (j != i && ProjectPicBox[j].IsEmpty == false && ProjectPicBox[j].Intersects(pichuBro[i].setRec()))
                        {
                            if (intersection(ProjectPicBox[j], pichuBro[i].setRec(), 0, "Left"))
                            {
                                pichuBro[i].takeDamage(6, 0);
                                PlayerClass[j].destroyProjectileRec();
                            }
                            else if (intersection(ProjectPicBox[j], pichuBro[i].setRec(), 0, "Right"))
                            {
                                pichuBro[i].takeDamage(6,0);
                                PlayerClass[j].destroyProjectileRec();
                            }
                        }

                        //check if pichuBro is hitting anyone
                        else if (pichuBro[i].setRec().Intersects(PlayerPicBox[j]) && j != i)
                        {
                            if (intersection(pichuBro[i].setRec(), PlayerPicBox[j], picBoxWidthScaling[j], "Left"))
                            {
                                if (pichuBro[i].isAttacking())
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
                                if (pichuBro[i].isAttacking())
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
                    if (pichuBro[i].enemyIsDead() == true)
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

                            pichuBro[i] = new Enemy(new Rectangle(PlayerPicBox[i].X - 50, PlayerPicBox[i].Y, PlayerPicBox[i].Width, PlayerPicBox[i].Height), null, i, character[i], Content, displayWidth, displayHeight, finalDestinationRec, stageHeightAdjustment);
                        }
                    }
                }
            }
        }

        //Draw stuff with crayons
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightSlateGray);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            /*
            //character select screen
            if (mediaPlayer.State == MediaState.Playing)
            {                
                background = mediaPlayer.GetTexture();
                spriteBatch.Draw(background, backgroundRec, Color.White);
            }*/
            if (character[0] == null)
            {
                for (int i = 0; i < charNum.Length; i++)
                {
                    spriteBatch.Draw(characterIcons[i], characterIconsRectangle[i], Color.White);
                }                
                
                for (int i = 0; i < numberPlayers; i++)
                {
                    spriteBatch.Draw(characterSelect[i], characterSelectRec[i], Color.White);
                    spriteBatch.Draw(charSelectTokens[i], charSelectTokensRec[i], Color.White);
                }                

                //display their character portrait
                for (int i = 0; i < numberPlayers; i++)
                {
                    for (int j = 0; j < charNum.Length; j++)
                    {
                        if (tempCharacter[i] == charNum[j])
                        {
                            spriteBatch.Draw(characterSplash[j], characterSplashRec[i], Color.White);
                        }
                    }
                }
            }

            else if (stage == 0)
            {
                for (int i = 0; i < stageIconsRec.Length; i++)
                {
                    spriteBatch.Draw(stageIcons[i], stageIconsRec[i], Color.White);
                }

                spriteBatch.Draw(charSelectTokens[0], charSelectTokensRec[0], Color.White);
            }

            else if (counter < 7200)
            {
                //backgrounds
                spriteBatch.Draw(background, backgroundRec, Color.White);
                spriteBatch.Draw(finalDestination[(counterAnimation % 5)], finalDestinationRec, Color.White);

                //health
                for (int i = 0; i < 4; i++)
                {
                    if (character[i] != null)
                    {
                        PlayerClass[i].DrawText(spriteBatch);
                    }
                }

                //display a timer at the top of the screen
                if ((((7200 - counter) / 60) % 60) > 9)
                {
                    spriteBatch.DrawString(font1, (((7200 - counter) / 3600).ToString() + ":" + (((7200 - counter) / 60) % 60)), new Vector2(5 + (displayWidth * 2 / 5), 5 + (displayHeight / 20)), Color.Black);
                    spriteBatch.DrawString(font1, (((7200 - counter) / 3600).ToString() + ":" + (((7200 - counter) / 60) % 60)), new Vector2(displayWidth * 2 / 5, displayHeight / 20), Color.White);
                }
                else
                {
                    spriteBatch.DrawString(font1, (((7200 - counter) / 3600).ToString() + ":0" + (((7200 - counter) / 60) % 60)), new Vector2(5 + (displayWidth * 2 / 5), 5 + (displayHeight / 20)), Color.Black);
                    spriteBatch.DrawString(font1, (((7200 - counter) / 3600).ToString() + ":0" + (((7200 - counter) / 60) % 60)), new Vector2(displayWidth * 2 / 5, displayHeight / 20), Color.White);
                }

                //draw the characters on the screen
                PlayerClass[0].Draw(spriteBatch);

                if (character[1] != null)
                {
                    PlayerClass[1].Draw(spriteBatch);
                }
                if (character[2] != null)
                {
                    PlayerClass[2].Draw(spriteBatch);
                }
                if (character[3] != null)
                {
                    PlayerClass[3].Draw(spriteBatch);
                }

                for (int i = 0; i < waddle.Length; i++)
                {
                    if (waddle[i] != null)
                    {
                        waddle[i].Draw(spriteBatch);
                    }
                }
                for (int i = 0; i < pichuBro.Length; i++)
                {
                    if (pichuBro[i] != null)
                    {
                        pichuBro[i].Draw(spriteBatch);
                        pichuBro[i].Draw2(spriteBatch);
                    }
                }
            }

            //display the characters scores at the end of the game
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    if (PlayerClass[i] != null)
                    {
                        spriteBatch.DrawString(font1, "Player " + (i + 1).ToString() + " scored " + score[i].ToString() + " points!", new Vector2(displayWidth / 5, (displayHeight * (i + 1) / 6)), Color.White);
                        spriteBatch.Draw(this.Content.Load<Texture2D>("Splash/" + character[i]+ "Art"), characterSplashRec[i], Color.White);
                    }
                }

                spriteBatch.DrawString(font1, "Plays " + plays.ToString(), new Vector2(displayWidth * 3 / 5, displayHeight * 4 / 5), Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
