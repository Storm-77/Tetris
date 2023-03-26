using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    public class GridLayer : Layer
    {
        Tetromino m_fallingTetromino;
        Grid m_visualGrid;

        List<Tetromino> m_floor;

        public GridLayer()
        {
            m_visualGrid = new Grid(GameData.cellsX, GameData.cellsY, GameData.cellSize);
            m_floor = new();
        }

        public override void Initialize()
        {
            m_fallingTetromino = m_visualGrid.SpawnTetromino();
        }

        private System.TimeSpan prev;
        bool m_flagW = true;
        bool m_flagLeft = true;
        bool m_flagRight = true;

        public override void Update(GameTime gameTime)
        {
            if ((gameTime.TotalGameTime - prev).Milliseconds >= 600)
            {
                m_fallingTetromino.Positon.Y++; // make it fall

                prev = gameTime.TotalGameTime;
            }


            if (Keyboard.GetState().IsKeyUp(Keys.W) && m_flagW)
                m_flagW = false;

            if (Keyboard.GetState().IsKeyDown(Keys.W) && !m_flagW)
            {
                m_flagW = true;
                m_fallingTetromino = m_visualGrid.SpawnTetromino();
            }

            //move to the left
            if (Keyboard.GetState().IsKeyUp(Keys.Left) && m_flagLeft)
                m_flagLeft = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Left) && !m_flagLeft)
            {
                m_flagLeft = true;

                if (m_fallingTetromino.MinXComponent() > 0)
                    m_fallingTetromino.Positon.X--;

            }

            //move to the right
            if (Keyboard.GetState().IsKeyUp(Keys.Right) && m_flagRight)
                m_flagRight = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Right) && !m_flagRight)
            {
                m_flagRight = true;

                if (m_fallingTetromino.MaxXComponent() < m_visualGrid.Width - 1)
                    m_fallingTetromino.Positon.X++;

            }


            //todo make tetromino fall fast when down arrow is pressed

            //todo make it stop on lowest block

            // if (m_fallingTetromino.MaxYComponent() == m_visualGrid.Height - 1)
            // {
            //     m_floor.Add(m_fallingTetromino);
            //     m_fallingTetromino = m_visualGrid.SpawnTetromino();
            // }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            foreach (var item in m_floor)
            {
                item.Draw(spriteBatch);
            }

            m_fallingTetromino.Draw(spriteBatch);

            //draw all tetrominos




            m_visualGrid.Draw(spriteBatch); // grid lines drawn over tetrominos
        }

        protected override void UnloadContent()
        {
        }

    }
}