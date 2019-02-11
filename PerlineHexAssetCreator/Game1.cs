using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PerlinControls;
using System.Xaml;
using PerlinGenerator;
using System.Windows;
using System.Threading;
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
        Color[] pixels;
        //we can get rid of these with some refactoring I guesssss
        private int[,] BackingNoiseR;
        private int[,] BackingNoiseG;
        private int[,] BackingNoiseB;

        private MainWindow Mw;
        private PerlinVarsModel PerlinVars;

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
            PerlinVars = new PerlinVarsModel();
            // TODO: Add your initialization logic here
            tracedSize = GraphicsDevice.PresentationParameters.Bounds;
            canvas = new Texture2D(GraphicsDevice, tracedSize.Width, tracedSize.Height, false, SurfaceFormat.Color);
            pixels = new Color[tracedSize.Width * tracedSize.Height];
            this.IsMouseVisible = true;

            //https://stackoverflow.com/questions/2329978/the-calling-thread-must-be-sta-because-many-ui-components-require-this
            //Application.Current.Dispatcher.Invoke((Action) delegate
            //{
            //    wM.Run(new PerlinControls.MainWindow());
            //});
            Thread t = new Thread(() =>
            {
                var app = new App();
                Mw = new MainWindow();
                Mw.SetSwap(GenNewNoise);
                app.Run(Mw);
            });

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
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

        private List<SignalPass.ReInit> GenNewNoise = new List<SignalPass.ReInit>() { SignalPass.ReInit.All };

        private int[,] NoiseColors;
        private double[,,] NoiseArray;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //if(GenNewNoise.Contains(SignalPass.ReInit.None))
            //{
            //    ;
            //}
            if (GenNewNoise.Any())
            {
                if (GenNewNoise.Contains(SignalPass.ReInit.All))
                {
                    NoiseArray = PerlinGenerator.PerlinGenerator.GenerateNoiseDimensions(height: tracedSize.Height,
                        width: tracedSize.Width,
                        depth: 1,
                        octaves: PerlinVars.PerlinOctaves,
                        persistence: PerlinVars.PerlinPersistence,
                        frequency: PerlinVars.PerlinFrequency,
                        amplitude: PerlinVars.PerlinAmplitude
                    );
                }
                if (GenNewNoise.Contains(SignalPass.ReInit.All) ||
                    GenNewNoise.Contains(SignalPass.ReInit.NumGradients))
                {
                    NoiseColors =
                        PerlinGenerator.PerlinGenerator.Map3DNoiseArrayToImage(PerlinVars.NumberOfColorGradients,
                            NoiseArray);
                }
                BackingNoiseR = PerlinGenerator.PerlinGenerator.Multiply2dArray(NoiseColors.Clone() as int[,],
                    PerlinVars.RedValueMultiplier);
                BackingNoiseG = PerlinGenerator.PerlinGenerator.Multiply2dArray(NoiseColors.Clone() as int[,],
                    PerlinVars.GreenValueMultiplier);
                BackingNoiseB = PerlinGenerator.PerlinGenerator.Multiply2dArray(NoiseColors.Clone() as int[,],
                    PerlinVars.BlueValueMultiplier);


                var height = BackingNoiseR.GetLength(0);
                var width = BackingNoiseR.GetLength(1);

                for (int h = 0; h < height; h++)
                {
                    for (int w = 0; w < width; w++)
                    {
                        Color hexVal;
                        //if (j > lastX - 5 && j < lastX + 5 && i < lastY + 5 && i > lastY - 5)
                        //{
                        //    hexVal = Color.Red;
                        //    lastX = currentX;
                        //    lastY = currentY;
                        //}
                        //else
                        hexVal = new Color(BackingNoiseR[h, w], BackingNoiseG[h, w], BackingNoiseB[h, w]);
                        pixels[h*width + w] = hexVal;
                        ;
                    }
                }

                #region 1dlines
                //fetch once
                var pH = PerlinVars.HorizontalLines;
                var pV = PerlinVars.VerticalLines;
                //give this a discrete signal as well
                if (pH || pV)
                {
                    //fetch once
                    var hort = PerlinVars.HorizontalLinesPer;
                    var vert = PerlinVars.VerticalLinesPer;

                    var wiggleMultiplier = 10f;
                    for (int h = 0; h < height; h++) //variabalize 100
                    {
                        for (int w = 0; w < width; w++) //^inverse for other directional line^
                        {
                            //depth can always be 0 for a 1d line
                            //this is between 0 and 1 right now right
                            double noiseVal = NoiseArray[h, w, 0] - 1*-1;
                            //this one d thing is realy easy to reason about...
                            if (h%hort == 0 && pH)
                            {
                                pixels[
                                    (int)
                                    Math.Min(Math.Max((h*width + w) + ((int) (noiseVal*wiggleMultiplier)*width), 0),
                                        height*width - 1)
                                ] = Color.Black;
                            }

                            if (w%vert == 0 && pV)
                            {
                                //this needs to be thought out
                                pixels[
                                    (int)
                                    Math.Min(Math.Max((h * width + w) + (int)(noiseVal * wiggleMultiplier), 0),
                                        height * width - 1)
                                ] = Color.Black;
                            }
                        }
                        ;
                    }
                }

                #endregion

                canvas.SetData<Color>(pixels, 0, tracedSize.Width*tracedSize.Height);
                Stream stream = File.Create(Path.Combine(Environment.CurrentDirectory, "ActualCanvas.png"));
                canvas.SaveAsPng(stream, canvas.Width, canvas.Height);
                stream.Dispose();
                GenNewNoise.Clear();
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
            if (ClickState == ButtonState.Pressed && Mouse.GetState().LeftButton == ButtonState.Released && this.IsActive && System.Windows.Forms.Form.ActiveForm ==
    (System.Windows.Forms.Control.FromHandle(Window.Handle) as System.Windows.Forms.Form) && Keyboard.GetState().IsKeyDown(Keys.A))
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

            Color[] colorArr = new Color[canvas.Width * canvas.Height];
            canvas.GetData(colorArr);

            var bmImage = new Bitmap(HexSize, HexSize);
            //var hexClicked = SelectedHex(x, y);
            var logText = new StringBuilder();
            for (var i = Math.Max(0, x - (HexSize / 2)); i < Math.Min(BackingNoiseR.GetLength(1), x + HexSize / 2); i++)
            {
                for (var j = Math.Max(0, y - (HexSize / 2)); j < Math.Min(BackingNoiseR.GetLength(0), y + HexSize / 2); j++)
                {
                    logText.AppendLine($"Attempting pixel {i} {j}");

                    bmImage.SetPixel(i - Math.Max(0, x - (HexSize / 2)), j - Math.Max(0, y - (HexSize / 2)), System.Drawing.Color.FromArgb(InSameHexAsCenter(x, y, i, j)? 255 : 0, colorArr[j * canvas.Width + i].R, colorArr[j * canvas.Width + i].G, colorArr[j * canvas.Width + i].B));

                    //logText.AppendLine("Setting pixel color" + ((InSameHexAsCenter(x, y, i, j)) ? " color " : " transparent") + BackingNoiseR[i, j].ToString() +  " " + BackingNoiseG[i, j].ToString() + " " + BackingNoiseB[i, j].ToString() + "\r\n");
                    logText.AppendLine("_____");
                    ;

                }
            }
            File.AppendAllText(Path.Combine(Environment.CurrentDirectory, "pixelLog.txt"), logText.ToString());
            bmImage.Save(Path.Combine(Environment.CurrentDirectory, "capturedHex.png"));
            Process.Start(Path.Combine(Environment.CurrentDirectory, "capturedHex.png"));

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

        public void CheckReInitCanvas()
        {
            ;
        }

    }
}
