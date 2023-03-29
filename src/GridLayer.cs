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

        public static uint InitialTickInterval = 400;
        public static uint MinimalTickInterval = 50;

        public GridLayer()
        {
            m_grid = new Grid(GameData.cellsX, GameData.cellsY, GameData.cellSize);
        }

        public override void Initialize()
        {
            m_fallingTetromino = m_grid.SpawnTetromino();
            m_fallingTetrominoController = new(m_fallingTetromino);
        }

        private System.TimeSpan prev;
        bool m_flagW = true;

        uint delay = InitialTickInterval;
        public override void Update(GameTime gameTime)
        {
            uint interTickDelay = Keyboard.GetState().IsKeyDown(Keys.Down) ? 30 : delay;

            if ((gameTime.TotalGameTime - prev).Milliseconds >= interTickDelay)
            {
                if (delay > MinimalTickInterval)
                    delay -= 6; //delay step

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



            if (m_grid.DetectCollision(m_fallingTetromino) || m_fallingTetromino.MaxYComponent() == m_grid.Height - 1) //collision or ground hit
            {
                if (m_fallingTetromino.Positon.Y <= m_grid.TetrominoSpawnHeight) //game over
                {
                    int a = 0;
                    //todo switch to game over screeen
                    return;
                }
                m_grid.LandTetromino(m_fallingTetromino);

                m_fallingTetromino = m_grid.SpawnTetromino();
                m_fallingTetrominoController.Tetromino = m_fallingTetromino;
                delay = InitialTickInterval;
            }

            m_fallingTetrominoController.Update(gameTime);
            m_grid.TestFills();

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            m_fallingTetromino.Draw(spriteBatch);
            m_grid.DrawLines(spriteBatch); // grid lines drawn over tetrominos
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