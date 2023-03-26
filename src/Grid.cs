using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Tetris
{
    public class Grid
    {
        private uint m_width, m_height, m_cellSize;
        private uint[,] m_board;

        public Color linesColor { get; set; }
        public int linesThickness { get; set; }
        public Grid(uint X, uint Y, uint cellSize)
        {
            m_width = X;
            m_height = Y;
            m_cellSize = cellSize;

            linesColor = Color.Black;
            linesThickness = 2;

            m_board = new uint[X, Y];

            EachCell((ref uint value) =>
            {
                value = 0; // explicitly set all cells to 0
            });

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle rect = new Rectangle(0, 0, (int)m_cellSize, (int)m_cellSize);

            for (int i = 0; i < m_width; i++)
            {
                for (int j = 0; j < m_height; j++)
                {
                    rect.X = (int)(i * m_cellSize);
                    rect.Y = (int)(j * m_cellSize);

                    spriteBatch.DrawRectangleOutline(rect, linesColor, linesThickness);
                }
            }

        }

        //helper functions
        private delegate void CellAction(ref uint cellValue);

        private void EachCell(CellAction action)
        {
            for (int i = 0; i < m_width; i++)
            {
                for (int j = 0; j < m_height; j++)
                {
                    action(ref m_board[i, j]);
                }
            }
        }

    }
}