using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    public class TetrominoKeyController
    {
        public TetrominoKeyController(Tetromino tetromino)
        {
            Tetromino = tetromino;
        }

        bool m_flagLeft = true;
        bool m_flagRight = true;

        public Tetromino Tetromino { get; set; }

        public void Update(GameTime gameTime)
        {
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

            Tetromino.CheckOuterBorders();

        }
    }
}