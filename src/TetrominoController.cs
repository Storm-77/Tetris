using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    internal class TetrominoControllerFactory
    {
        private Grid m_grid;
        public TetrominoControllerFactory(ref Grid grid)
        {
            m_grid = grid;
        }

        public TetrominoController MakeController()
        {
            return new TetrominoController(m_grid.SpawnTetromino());
        }

        public TetrominoController MakeController(Tetromino tetromino)
        {
            return new TetrominoController(tetromino);
        }

    }

    public class TetrominoController
    {
        public static uint InitialTickInterval = 400;
        public static uint MinimalTickInterval = 50;
        public Tetromino Tetromino { get; set; }
        public TetrominoController(Tetromino tetromino)
        {
            Tetromino = tetromino;
            m_tickDelay = InitialTickInterval;
        }

        private bool m_flagLeft = true;
        private bool m_flagRight = true;
        private bool m_flagUp = true;
        private uint m_tickDelay;

        private System.TimeSpan m_prev;
        public void Update(GameTime gameTime)
        {
            uint dl = Keyboard.GetState().IsKeyDown(Keys.Down) ? 30 : m_tickDelay;

            if ((gameTime.TotalGameTime - m_prev).Milliseconds >= dl)
            {
                if (m_tickDelay > MinimalTickInterval)
                    m_tickDelay -= 6; //delay step

                Tetromino.Positon.Y++; // make it fall
                m_prev = gameTime.TotalGameTime;

            }

            //move to the left
            if (Keyboard.GetState().IsKeyUp(Keys.Left) && m_flagLeft)
                m_flagLeft = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Left) && !m_flagLeft)
            {
                m_flagLeft = true;
                Tetromino.Positon.X--;
            }

            //move to the right
            if (Keyboard.GetState().IsKeyUp(Keys.Right) && m_flagRight)
                m_flagRight = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Right) && !m_flagRight)
            {
                m_flagRight = true;
                Tetromino.Positon.X++;
            }

            //rotarion
            if (Keyboard.GetState().IsKeyUp(Keys.Up) && m_flagUp)
                m_flagUp = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Up) && !m_flagUp)
            {
                m_flagUp = true;
                Tetromino.Rotate();
            }

            Tetromino.CheckOuterBorders();

        }
    }
}