using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;

namespace HELP_ME
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random random = new Random();
        char[] answer = "Karan".ToCharArray();
        char[] letters = "jfnhe".ToCharArray();
        bool done = true;     

        Sprite goal;
        Sprite climber;

        Texture2D climbingTexture;
        Texture2D goalTexture;

        uint[] tempList;
        Color[] climbingColors;
        Color[] goalColors;

        public void Mutate (Color[] colors)
        {
            var tempArray = getInts(colors);
            Mutate(tempArray);
            colors = GetColors(tempArray);
        }
        public bool Error (Color[] newA, uint[] oldA, Color[] goal, ref bool gud)
        {
            var tempA = getInts(newA);
            var tempG = getInts(goal);

            return Error(tempA, oldA, tempG, ref gud);
        }

        public uint[] getInts(Color[] colors)
        {
            var tempArray = new uint[colors.Length];
            for (int i = 0; i < colors.Length; i++)
            {
                tempArray[i] = colors[i].PackedValue;
            }
            return tempArray;
        }

        public Color[] GetColors(uint[] tempArray)
        {
            var colors = new Color[tempArray.Length];
            for (int i = 0; i < tempArray.Length; i++)
            {
                colors[i] = new Color((uint)tempArray[i]);
            }
            return colors;
        }

        public void Mutate(uint[] letters)
        {
            var i = random.Next(0, letters.Length);
            letters[i] += (uint)(random.Next(0, 2) == 0 ? 1 : -1);
        }

        public static string BeYarn(char[] thread)
        {
            string quilt = "";
            foreach (var spool in thread)
            {
                quilt += spool;
            }

            return quilt;
        }
        public char[] Shuffle(int length)
        {
            var returner = new char[length];
            for (int i = 0; i < returner.Length; i++)
            {
                returner[i] = (char)random.Next(41, 123);
            }
            return returner;
        }
        public bool Error(uint[] newList, uint[] oldList, uint[] goal, ref bool better)
        {
            uint newDist = 0;
            uint oldDist = 0;
            for (int i = 0; i < newList.Length; i++)
            {
                newDist += (uint)MathHelper.Distance(newList[i], goal[i]);
                oldDist += (uint)MathHelper.Distance(oldList[i], goal[i]);
            }
            better = newDist < oldDist;
            return newDist == 0;
        }



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
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 500;
            graphics.ApplyChanges();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
                        
            int width = 2;
            int height = 2;
            climbingTexture = new Texture2D(GraphicsDevice, width, height);
            goalTexture = new Texture2D(GraphicsDevice, width, height);

            var pixelColor = new Color[width * height];
            for (int i = 0; i < pixelColor.Length; i++)
            {
                pixelColor[i] = Color.White;                
            }
            climbingTexture.SetData(pixelColor);

            var fuckOff = new Color[width * height];
            for (int i = 0; i < fuckOff.Length; i++)
            {
                fuckOff[i] = Color.Blue;
            }

            goalTexture.SetData(fuckOff);

            climber = new Sprite(climbingTexture, new Vector2(0, 0));
            climber.Scale = 500 / width;
            goal = new Sprite(goalTexture, new Vector2(width * climber.Scale, 0));
            goal.Scale = climber.Scale;

            climbingColors = pixelColor;
            goalColors = new Color[pixelColor.Length];
            goalTexture.GetData(goalColors);
            tempList = getInts(climbingColors);
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
        protected override void Update(GameTime gameTime)
        {
            //if (!done)
            //{
            //    var tempList = (char[])letters.Clone();
            //    Mutate(ref letters);
            //    bool gud = false;

            //    if (Error(letters, tempList, answer, ref gud))
            //    {
            //        Console.WriteLine($"Reached goal of {BeYarn(answer)}");
            //        done = true;
            //    }
            //    if (gud)
            //    {
            //        Console.WriteLine($"Positive mutation, {BeYarn(letters)}");
            //    }
            //    else
            //    {
            //        Console.WriteLine($"Negative mutation, {BeYarn(letters)} reverted back to {BeYarn(tempList)}");
            //        letters = tempList;
            //    }

            //}
            //else
            //{
            //    Console.WriteLine("Enter word(s):");
            //    string words = Console.ReadLine();
            //    answer = words.ToCharArray();
            //    letters = Shuffle(answer.Length);
            //    done = false;
            //}

            bool gud = false;

            if (Error(climbingColors, tempList, goalColors, ref gud))
            {
                done = true;
            }
            if (!gud)
            {
                climbingColors = GetColors(tempList);
            }

            tempList = getInts(climbingColors);
            Mutate(climbingColors);

            climber.Image.SetData(climbingColors);
            goal.Image.SetData(goalColors);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            goal.Draw(spriteBatch);
            climber.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
