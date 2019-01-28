using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private int[,] BackingNoiseR;
        private int[,] BackingNoiseG;
        private int[,] BackingNoiseB;




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

        private double[,,] NoiseArrayR;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (GenNewNoise)
            {
                NoiseArrayR = PerlinGenerator.PerlinGenerator.GenerateNoiseDimensions(height: tracedSize.Height,
                    width: tracedSize.Width,
                    depth: 1,
                    octaves: 25,
                    persistence: .25,
                    frequency: 4,
                    amplitude: 32
                );
                var noiseColorsR = PerlinGenerator.PerlinGenerator.Map3DNoiseArrayToImage(8, NoiseArrayR);
                noiseColorsR = PerlinGenerator.PerlinGenerator.Multiply2dArray(noiseColorsR, .5);
                BackingNoiseR = noiseColorsR;
                
                var noiseArrayG = PerlinGenerator.PerlinGenerator.GenerateNoiseDimensions(height: tracedSize.Height,
                    width: tracedSize.Width,
                    depth: 1,
                    octaves: 25,
                    persistence: .25,
                    frequency: 8,
                    amplitude: 16
                );
                var noiseColorsG = PerlinGenerator.PerlinGenerator.Map3DNoiseArrayToImage(8, noiseArrayG);
                noiseColorsG = PerlinGenerator.PerlinGenerator.Multiply2dArray(noiseColorsG, 1.5);

                BackingNoiseG = noiseColorsG;

                var noiseArrayB = PerlinGenerator.PerlinGenerator.GenerateNoiseDimensions(height: tracedSize.Height,
                    width: tracedSize.Width,
                    depth: 1,
                    octaves: 25,
                    persistence: .25,
                    frequency: 8,
                    amplitude: 16
                );
                var noiseColorsB = PerlinGenerator.PerlinGenerator.Map3DNoiseArrayToImage(8, noiseArrayB);
                noiseColorsB = PerlinGenerator.PerlinGenerator.Multiply2dArray(noiseColorsB, .5);
                BackingNoiseB = noiseColorsB;

                for (int i = 0; i < NoiseArrayR.GetLength(0);i++)
                {
                    for (int j = 0; j < NoiseArrayR.GetLength(1); j++)
                    {
                        Int32 noiseColorR = noiseColorsR[i, j];
                        Int32 noiseColorG = noiseColorsG[i, j];
                        Int32 noiseColorB = noiseColorsB[i, j];
                        //no A on this atm, toggleable make
                        var hexNoiseVal = "FF" +  noiseColorR.ToString("X") + noiseColorG.ToString("X") + noiseColorB.ToString("X");
                        var hexVal = (uint)int.Parse(hexNoiseVal, System.Globalization.NumberStyles.HexNumber);
                        pixels[i*NoiseArrayR.GetLength(1) + j] = hexVal; //0xFF000000;
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
            if (ClickState == ButtonState.Pressed && Mouse.GetState().LeftButton == ButtonState.Released && this.IsActive)
            {
                var xpos = Mouse.GetState().X;
                var ypos = Mouse.GetState().Y;
                BuildImage(xpos ,ypos);
            }
            else
            {

            }
            ClickState = Mouse.GetState().LeftButton;



            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(canvas, new Rectangle(0, 0, tracedSize.Width, tracedSize.Height), Color.Gray);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        /*
         * The horizontal distance between adjacent hexagon centers is w * 3/4. The vertical distance between adjacent hexagon centers is h.
         */
        public void BuildImage(int x, int y)
        {
            var bmImage = new Bitmap(HexSize, HexSize);
            //var hexClicked = SelectedHex(x, y);
            var logText = new StringBuilder();
            for (var i = Math.Max(0, x - (HexSize / 2)); i < Math.Min(tracedSize.Width, x + HexSize / 2); i++)
            {
                for (var j = Math.Max(0, y - (HexSize / 2)); j < Math.Min(tracedSize.Height, y + HexSize / 2); j++)
                {
                    
                    ;
                    //if(i > x - 5 && i < x + 5  && j > y - 5 && j < y + 5)
                    //    bmImage.SetPixel(i,
                    //        j,
                    //        System.Drawing.Color.Red);
                    if(InSameHexAsCenter(x,y,i,j))
                        bmImage.SetPixel(i - Math.Max(0, x - (HexSize / 2)), j - Math.Max(0, y - (HexSize / 2)), System.Drawing.Color.FromArgb(255, BackingNoiseR[i, j], BackingNoiseG[i, j], BackingNoiseB[i, j]));
                    else
                        bmImage.SetPixel(i - Math.Max(0, x - (HexSize / 2)), j - Math.Max(0, y - (HexSize / 2)), System.Drawing.Color.FromArgb(0, BackingNoiseR[i, j], BackingNoiseG[i, j], BackingNoiseB[i, j]));

                    //logText.Append( "Setting pixel "  + (i - Math.Max(0, x - (HexSize / 2))).ToString() + " " + (j - Math.Max(0, y - (HexSize / 2))).ToString() + ((hexAt["X"] == hexClicked["X"] && hexAt["Y"] == hexClicked["Y"]) ? " color " : " transparent") + BackingNoise[i,j].ToString() + "\r\n");
                    ;
                    //bmImage.SetPixel(i - Math.Max(0, x - (HexSize / 2)), j - Math.Max(0, y - (HexSize / 2)), System.Drawing.Color.Red);

                }
            }
            File.AppendAllText(Path.Combine(Environment.CurrentDirectory, "pixelLog.txt"), logText.ToString());
            bmImage.Save(Path.Combine(@"C:\Users\kk\Desktop\Hexes\Hexes\Modules\MainModule\Content\Backgrounds", "capturedHex.png"));
            Process.Start(Path.Combine(@"C:\Users\kk\Desktop\Hexes\Hexes\Modules\MainModule\Content\Backgrounds", "capturedHex.png"));

        }

        public bool InSameHexAsCenter(int centerX, int centerY, int posX, int posY)
        {
            //http://www.playchilla.com/how-to-check-if-a-point-is-inside-a-hexagon
            var distX = Math.Abs(posX - centerX);
            var distY = Math.Abs(posY - centerY);
            //wtheck is this magic BS
            var horiDist = HexSize * .44;
            var vertDist = HexSize * .5;
            if (distX > horiDist || distY > vertDist)
                return false;
            return (horiDist * vertDist) - ((vertDist/2) * distX) - (horiDist * distY) >= 0; 
        }

        public Dictionary<string, int> SelectedHex(int x, int y)
        {
            var R = (int) Math.Round((2.0f/3.0f*y)/HexSize);
            var Q = (int) Math.Round(((Math.Sqrt(3)/3*x) - (1.0f/3.0f*y))/HexSize);
            return new Dictionary<string, int>() {
                { "U", R},
                { "V", Q}
            };
        }

    }
}
