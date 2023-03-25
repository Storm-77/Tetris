using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    public class Grid : Layer
    {
        private Dictionary<string, Texture2D> m_tileCombinations;
        private ContentManager m_content;
        public Texture2D frame;
        Vector2 position;

        private readonly float tileSize = (float)GameData.bufferW / (float)12;

        //gamestate 12x20 2d array of chars representing the board

        public Grid(ContentManager contentManager)
        {
            m_content = contentManager;
        }

        public override void Initialize()
        {
            m_tileCombinations = new Dictionary<string, Texture2D>();
            position = Vector2.Zero;


            frame = m_content.Load<Texture2D>("Board-Clear");

            m_tileCombinations["Z"] = m_content.Load<Texture2D>("TILE_Z");
            m_tileCombinations["T"] = m_content.Load<Texture2D>("TILE_T");
            m_tileCombinations["S"] = m_content.Load<Texture2D>("TILE_S");
            m_tileCombinations["O"] = m_content.Load<Texture2D>("TILE_O");
            m_tileCombinations["L"] = m_content.Load<Texture2D>("TILE_L");
            m_tileCombinations["J"] = m_content.Load<Texture2D>("TILE_J");
            m_tileCombinations["I"] = m_content.Load<Texture2D>("TILE_I");
        }
        System.TimeSpan prev;
        int counter = 1;

        public override void Update(GameTime gameTime)
        {
            if ((gameTime.TotalGameTime - prev).Seconds >= 1)
            {

                counter++;
                position.Y += tileSize;

                prev = gameTime.TotalGameTime;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(frame, new Vector2(0, 0), Color.White);

            spriteBatch.Draw(m_tileCombinations["J"], position, Color.Red);

        }

        protected override void UnloadContent()
        {
            m_content.Unload();
        }

    }
}