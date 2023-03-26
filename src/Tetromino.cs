using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Tetris
{

    public partial class Tetromino
    {
        private int m_shapeDataIndex;
        private Color m_color;
        private int m_cellSize;

        public Tetromino(uint cellSize, TetrominoShape shape = TetrominoShape.None)
        {
            m_cellSize = (int)cellSize;

            if (shape == TetrominoShape.None)
                shape = PickRandomShape();

            s_figureDataIndexMap.TryGetValue(shape, out m_shapeDataIndex);

            // pick random color from pool
            Color[] colors = { Color.DeepPink, Color.Orange, Color.PaleGreen, Color.MediumBlue, Color.MediumVioletRed };

            System.Random random = new System.Random();

            m_color = colors[random.Next(colors.Length)];

            //? maybe add outline color variable
        }

        public void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            Rectangle cell = new Rectangle(x, y, m_cellSize, m_cellSize);

            for (int i = 0; i < 4; i++)
            {
                cell.X = x * m_cellSize + m_cellSize * s_figureData[m_shapeDataIndex, i, 0];
                cell.Y = y * m_cellSize + m_cellSize * s_figureData[m_shapeDataIndex, i, 1];

                spriteBatch.DrawRectangleColor(cell, m_color);

            }
        }
    }
}