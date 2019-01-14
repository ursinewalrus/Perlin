using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PerlinGenerator;

namespace PerlineHexAssetCreator
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D canvas;
        Rectangle tracedSize;
        UInt32[] pixels;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            tracedSize = GraphicsDevice.PresentationParameters.Bounds;
            canvas = new Texture2D(GraphicsDevice, tracedSize.Width, tracedSize.Height, false, SurfaceFormat.Color);
            pixels = new UInt32[tracedSize.Width * tracedSize.Height];
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
        private bool GenNewNoise = true;

        private double[,,] NoiseArray;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (GenNewNoise)
            {
                NoiseArray = PerlinGenerator.PerlinGenerator.GenerateNoiseDimensions(height: 500,
                    width: 500,
                    depth: 1,
                    octaves: 25,
                    persistence: .25,
                    frequency: 16,
                    amplitude: 32
                );
                var noiseColors = PerlinGenerator.PerlinGenerator.Map3DNoiseArrayToImage(8, NoiseArray);
                for (int i = 0; i < NoiseArray.GetLength(0);i++)
                {
                    for (int j = 0; j < NoiseArray.GetLength(1); j++)
                    {
                        Int32 noiseColor = noiseColors[i, j];
                        var hexNoiseVal = noiseColor.ToString("X") + noiseColor.ToString("X") + noiseColor.ToString("X") + noiseColor.ToString("X");
                        var hexVal = (uint)int.Parse(hexNoiseVal, System.Globalization.NumberStyles.HexNumber);
                        pixels[j*NoiseArray.GetLength(1) + i] = hexVal; //0xFF000000;
                    }
                }
                canvas.SetData<UInt32>(pixels, 0, tracedSize.Width * tracedSize.Height);

                GenNewNoise = false;
            }
            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);


            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(canvas, new Rectangle(0, 0, 500, 500), Color.Gray);
            spriteBatch.End();
            base.Draw(gameTime);

        }
    }
}
