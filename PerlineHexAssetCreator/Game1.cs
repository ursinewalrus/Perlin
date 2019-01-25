using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PerlinGenerator;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

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
        private int[,] BackingNoise;



        private int HexSize = 100;
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
            this.IsMouseVisible = true;

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
                NoiseArray = PerlinGenerator.PerlinGenerator.GenerateNoiseDimensions(height: tracedSize.Height,
                    width: tracedSize.Width,
                    depth: 1,
                    octaves: 25,
                    persistence: .25,
                    frequency: 16,
                    amplitude: 32
                );
                var noiseColors = PerlinGenerator.PerlinGenerator.Map3DNoiseArrayToImage(8, NoiseArray);
                BackingNoise = noiseColors;
                for (int i = 0; i < NoiseArray.GetLength(0);i++)
                {
                    for (int j = 0; j < NoiseArray.GetLength(1); j++)
                    {
                        Int32 noiseColor = noiseColors[i, j];
                        var hexNoiseVal = noiseColor.ToString("X") + noiseColor.ToString("X") + noiseColor.ToString("X") + noiseColor.ToString("X");
                        var hexVal = (uint)int.Parse(hexNoiseVal, System.Globalization.NumberStyles.HexNumber);
                        pixels[i*NoiseArray.GetLength(1) + j] = hexVal; //0xFF000000;
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
        private ButtonState ClickState;
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            var a = ClickState;
            if (ClickState == ButtonState.Pressed && Mouse.GetState().LeftButton == ButtonState.Released)
            {
                var xpos = Mouse.GetState().X;
                var ypos = Mouse.GetState().Y;
                BuildImage(xpos ,ypos);
            }
            ClickState = Mouse.GetState().LeftButton;



            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(canvas, new Rectangle(0, 0, 500, 500), Color.Gray);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        /*
         * The horizontal distance between adjacent hexagon centers is w * 3/4. The vertical distance between adjacent hexagon centers is h.
         */
        public void BuildImage(int x, int y)
        {
            //tracedsize does not match the canvas size at all
            var bmImage = new Bitmap(HexSize,HexSize);
            var hexClicked = SelectedHex(x, y);
            var logText = new StringBuilder();
            for (var i = Math.Max(0,x-(HexSize/2)); i < Math.Min(tracedSize.Width, x+HexSize/2); i++)
            {
                for (var j = Math.Max(0, y - (HexSize / 2)); j < Math.Min(tracedSize.Height, y + HexSize / 2); j++)
                {
                    
                    ;
                    var hexAt = SelectedHex(i, j);
                    if (hexAt["X"] == hexClicked["X"] && hexAt["Y"] == hexClicked["Y"])
                        bmImage.SetPixel(i - Math.Max(0, x - (HexSize / 2)), j - Math.Max(0, y - (HexSize / 2)), System.Drawing.Color.FromArgb(255, BackingNoise[i, j], BackingNoise[i, j], BackingNoise[i, j]));
                    else
                        bmImage.SetPixel(i - Math.Max(0, x - (HexSize / 2)), j - Math.Max(0, y - (HexSize / 2)), System.Drawing.Color.FromArgb(0, BackingNoise[i, j], BackingNoise[i, j], BackingNoise[i, j]));
                    logText.Append( "Setting pixel "  + (i - Math.Max(0, x - (HexSize / 2))).ToString() + " " + (j - Math.Max(0, y - (HexSize / 2))).ToString() + ((hexAt["X"] == hexClicked["X"] && hexAt["Y"] == hexClicked["Y"]) ? " color " : " transparent") + BackingNoise[i,j].ToString() + "\r\n");
                    var t = bmImage.GetPixel(i - Math.Max(0, x - (HexSize/2)), j - Math.Max(0, y - (HexSize/2)));
                    ;
                    //bmImage.SetPixel(i - Math.Max(0, x - (HexSize / 2)), j - Math.Max(0, y - (HexSize / 2)), System.Drawing.Color.Red);

                }
            }
            File.AppendAllText(@"C:\Users\jkerxhalli\Desktop\golf\PixelLog.txt", logText.ToString());
            bmImage.Save(@"C:\Users\jkerxhalli\Desktop\golf\selected.png");

        }

        public Dictionary<string, int> SelectedHex(int x, int y)
        {
            var R = (int) Math.Round((2.0f/3.0f*y)/HexSize);
            var Q = (int) Math.Round(((Math.Sqrt(3)/3*x) - (1.0f/3.0f*y))/HexSize);
            return new Dictionary<string, int>() {
                { "X", R},
                { "Y", Q}};
        }
    }
}
