using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    public class GridLayer : Layer
    {
        private TetrominoController m_tetrominoController;
        private Grid m_grid;
        private TetrominoControllerFactory m_controllerFactory;

        public GridLayer()
        {
            m_grid = new Grid(GameData.cellsX, GameData.cellsY, GameData.cellSize);
            m_controllerFactory = new TetrominoControllerFactory(ref m_grid);
        }

        public override void Initialize()
        {
            m_tetrominoController = m_controllerFactory.MakeController();
        }

        bool m_flagW = true;

        public override void Update(GameTime gameTime)
        {

            if (Keyboard.GetState().IsKeyUp(Keys.W) && m_flagW)
            {
                m_flagW = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W) && !m_flagW)
            {
                m_flagW = true;
                m_tetrominoController = m_controllerFactory.MakeController();
            }

            if (m_grid.DetectCollision(m_tetrominoController.Tetromino) || m_tetrominoController.Tetromino.MaxYComponent() == m_grid.Height - 1) //collision or ground hit
            {
                if (m_tetrominoController.Tetromino.Positon.Y <= m_grid.TetrominoSpawnHeight) //game over
                {
                    int a = 0;
                    //todo switch to game over screeen
                    return;
                }
                m_grid.LandTetromino(m_tetrominoController.Tetromino);

                m_tetrominoController = m_controllerFactory.MakeController();
            }

            m_tetrominoController.Update(gameTime);
            m_grid.TestFills();

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            m_tetrominoController.Tetromino.Draw(spriteBatch);
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