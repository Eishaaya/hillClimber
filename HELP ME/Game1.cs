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

        Vector4[] tempList;
        Color[] climbingColors;
        Color[] goalColors;

        public void Mutate(ref Color[] colors)
        {
            var tempArray = getInts(colors);
            SubirUneMutation(ref tempArray);
            colors = GetColors(tempArray);
        }
        public bool Error(Color[] newA, Vector4[] oldA, Color[] goal, ref bool gud)
        {
            var tempA = GetVector4s(newA);
            var tempG = GetVector4s(goal);

            return TestError(tempA, oldA, tempG, ref gud);
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

        public Vector4[] GetVector4s(Color[] colors)
        {
            var tempArray = new Vector4[colors.Length];
            for (int i = 0; i < colors.Length; i++)
            {
                tempArray[i] = colors[i].ToVector4();
            }
            return tempArray;
        }

        public Color[] ReturnColors(Vector4[] tempArray)
        {
            var colors = new Color[tempArray.Length];
            for (int i = 0; i < tempArray.Length; i++)
            {
                colors[i] = new Color(tempArray[i]);
            }
            return colors;
        }

        public Color[] GetColors(uint[] tempArray)
        {
            var colors = new Color[tempArray.Length];
            for (int i = 0; i < tempArray.Length; i++)
            {
                colors[i] = new Color(tempArray[i]);
            }
            return colors;
        }

        public void SubirUneMutation(ref uint[] lettres)
        {
            //this is in french
            var indice = random.Next(0, lettres.Length);
            var couleurActuelles = lettres[indice];
            lettres[indice] = 0;

            var aléatoire = random.Next(0, 4);

            byte[] octetsDeCouleur = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                octetsDeCouleur[i] = (byte)(couleurActuelles >> i * 8);
            }

            var octetActuel = octetsDeCouleur[aléatoire];
            var monnaie = random.Next(0, 2) == 1 ? 1 : -1;
            octetActuel = (byte)(octetActuel + monnaie);

            if (Math.Abs(octetsDeCouleur[aléatoire] - octetActuel) != 1)
            {
                octetActuel = (byte)(octetActuel - monnaie);
            }

            octetsDeCouleur[aléatoire] = octetActuel;
            for (int i = 0; i < 4; i++)
            {
                lettres[indice] += (uint)octetsDeCouleur[i] << i * 8;
            }
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
                newDist += (uint)Math.Abs(newList[i] - goal[i]);
                oldDist += (uint)Math.Abs(oldList[i] - goal[i]);
            }
            better = newDist < oldDist;
            return newDist == 0;
        }

        public bool TestError(Vector4[] newList, Vector4[] oldList, Vector4[] goal, ref bool better)
        {
            float newDist = 0;
            float oldDist = 0;
            for (int i = 0; i < newList.Length; i++)
            {
                newDist += Vector4.Distance(newList[i], goal[i]);
                oldDist += Vector4.Distance(oldList[i], goal[i]);
            }
            better = newDist < oldDist;
            return newDist == 0;
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 500;
            graphics.ApplyChanges();
            base.Initialize();
        }

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
                pixelColor[i] = Color.LimeGreen;
            }
            climbingTexture.SetData(pixelColor);

            var fuckOff = new Color[width * height];
            for (int i = 0; i < fuckOff.Length; i++)
            {
                fuckOff[i] = Color.Red;
            }

            goalTexture.SetData(fuckOff);

            climber = new Sprite(climbingTexture, new Vector2(0, 0));
            climber.Scale = 500 / width;
            goal = new Sprite(goalTexture, new Vector2(width * climber.Scale, 0));
            goal.Scale = climber.Scale;

            climbingColors = pixelColor;
            goalColors = new Color[pixelColor.Length];
            goalTexture.GetData(goalColors);
            tempList = GetVector4s(climbingColors);
        }

        protected override void Update(GameTime gameTime)
        {
            #region oldCOde

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

            #endregion

            bool gud = false;

            if (Error(climbingColors, tempList, goalColors, ref gud))
            {
                done = true;
            }
            if (!gud)
            {
                climbingColors = ReturnColors(tempList);
            }

            tempList = GetVector4s(climbingColors);
            Mutate(ref climbingColors);

            climber.Image.SetData(climbingColors);
            goal.Image.SetData(goalColors);

            base.Update(gameTime);
        }

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
