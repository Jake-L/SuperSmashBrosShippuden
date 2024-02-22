using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace SmashBrosShippuden
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private CharacterSelect characterSelect;
        private StageSelect stageSelect;
        private Battle battle;

        GamePadState[] gamePad = new GamePadState[4];
        int counter = 0;

        //display settings
        int displayWidth;
        int displayHeight;

        /*
        //opening trailer
        Video video;
        VideoPlayer mediaPlayer; */

        //background music


        BaseModel currentModel;
        

        //write to text files
        System.IO.StreamWriter file;
        System.IO.StreamReader read;
        int plays = 0;
        string address = "";

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            //Set the size of the window
            this._graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            this._graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            this._graphics.ApplyChanges();

            //Display settings
            displayWidth = GraphicsDevice.Viewport.Width;
            displayHeight = GraphicsDevice.Viewport.Height;

            //song = this.NumberGenerator.Next(0, 3);

            //prepare text writing variables
            address = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            if (System.IO.File.Exists(address + "\\smashbros.txt"))
            {
                read = new System.IO.StreamReader(address + "\\smashbros.txt");

                plays = Convert.ToInt32(read.ReadLine());

                read.Close();
            }

            this.characterSelect = new CharacterSelect(displayWidth, displayHeight);
            this.currentModel = this.characterSelect;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            this.characterSelect.LoadContent(this.Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            //take input from controllers
            for (int i = 0; i < gamePad.Length; i++)
            {
                gamePad[i] = GamePad.GetState(i);
            }

            this.currentModel.Update(gamePad, gameTime);

            if (this.currentModel.isComplete())
            {
                if (this.stageSelect == null)
                {
                    this.stageSelect = new StageSelect();
                    this.currentModel = this.stageSelect;
                }
                else if (this.battle == null)
                {
                    this.battle = new Battle(this.stageSelect.stageIndex, this.characterSelect.character, this.displayWidth, this.displayHeight);
                    this.currentModel = this.battle;
                }
                else
                {
                    this.battle = null;
                    this.stageSelect = null;
                    this.characterSelect = new CharacterSelect(this.displayWidth, this.displayHeight);
                    this.currentModel = this.characterSelect;
                }
                this.currentModel.LoadContent(this.Content);
            }



            //else if (counter >= 7200)
            //{
            //    //write to a text file that another game has been played
            //    if (counter == 7200)
            //    {
            //        plays += 1;

            //        file = new System.IO.StreamWriter(address + "\\smashbros.txt");
            //        file.WriteLine(plays);
            //        file.Close();

            //        counter++;
            //    }
            //    //if the game is over, and they press start, restart the game
            //    if (gamePad[0].Buttons.Start == ButtonState.Pressed)
            //    {
            //        character[0] = null;
            //        stage = 0;
            //        for (int i = 0; i < 4; i++)
            //        {
            //            PlayerClass[i] = null;
            //            tempCharacter[i] = null;
            //            charSelectTokensRec[i].Y = 100;
            //            charSelectTokensRec[i].X = 100 + (i * 50);
            //            score[i] = 0;
            //            //mediaPlayer.Stop();
            //        }
            //        counter = 0;
            //    }
            //}

            //if (MediaPlayer.State != MediaState.Playing && stage != 0)
            //{
            //    song += 1;

            //    if (song >= backgroundMusic.Length)
            //    {
            //        song = 0;
            //    }

            //    MediaPlayer.Play(backgroundMusic[song]);
            //}


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightSlateGray);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            currentModel.Draw(_spriteBatch);

            ////display the characters scores at the end of the game
            //else
            //{
            //    for (int i = 0; i < 4; i++)
            //    {
            //        if (PlayerClass[i] != null)
            //        {
            //            _spriteBatch.DrawString(font1, "Player " + (i + 1).ToString() + " scored " + score[i].ToString() + " points!", new Vector2(displayWidth / 5, (displayHeight * (i + 1) / 6)), Color.White);
            //            _spriteBatch.Draw(this.Content.Load<Texture2D>("Splash/" + character[i] + "Art"), characterSplashRec[i], Color.White);
            //        }
            //    }

            //    _spriteBatch.DrawString(font1, "Plays " + plays.ToString(), new Vector2(displayWidth * 3 / 5, displayHeight * 4 / 5), Color.White);
            //}

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
