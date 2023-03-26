using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    public class GridLayer : Layer
    {
        Tetromino m_fallingTetromino;
        TetrominoKeyController m_fallingTetrominoController;
        Grid m_grid;
        List<Tetromino> m_floor;

        public GridLayer()
        {
            m_grid = new Grid(GameData.cellsX, GameData.cellsY, GameData.cellSize);
            m_floor = new();
        }

        public override void Initialize()
        {
            m_fallingTetromino = m_grid.SpawnTetromino();
            m_fallingTetrominoController = new(m_fallingTetromino);
        }

        private System.TimeSpan prev;
        bool m_flagW = true;
        public override void Update(GameTime gameTime)
        {
            int interTickDelay = Keyboard.GetState().IsKeyDown(Keys.Down) ? 30 : 400;

            if ((gameTime.TotalGameTime - prev).Milliseconds >= interTickDelay)
            {
                m_fallingTetromino.Positon.Y++; // make it fall

                prev = gameTime.TotalGameTime;
            }


            if (Keyboard.GetState().IsKeyUp(Keys.W) && m_flagW)
            {
                m_flagW = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W) && !m_flagW)
            {
                m_flagW = true;
                m_fallingTetromino = m_grid.SpawnTetromino();
                m_fallingTetrominoController.Tetromino = m_fallingTetromino;
            }

            m_fallingTetrominoController.Update(gameTime);

            if (m_grid.DetectCollision(m_fallingTetromino) || m_fallingTetromino.MaxYComponent() == m_grid.Height - 1) //collision or ground hit
            {
                if (m_fallingTetromino.Positon.Y <= m_grid.TetrominoSpawnHeight)//game over
                {
                    //todo switch to game over screeen
                    return;
                }
                m_grid.LandTetromino(m_fallingTetromino);

                m_floor.Add(m_fallingTetromino);

                m_fallingTetromino = m_grid.SpawnTetromino();
                m_fallingTetrominoController.Tetromino = m_fallingTetromino;
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            foreach (var item in m_floor)
            {
                item.Draw(spriteBatch);
            }

            m_fallingTetromino.Draw(spriteBatch);

            m_grid.Draw(spriteBatch); // grid lines drawn over tetrominos
        }

        protected override void UnloadContent()
        {
        }

        public Vector2 GetGridSizePx()
        {
            Vector2 size;

            size.X = m_grid.Width * m_grid.CellSize;
            size.Y = m_grid.Height * m_grid.CellSize;

            return size;
        }


    }
}