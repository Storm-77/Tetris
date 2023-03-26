using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Tetris
{

    public partial class Tetromino
    {
        private int m_shapeDataIndex;
        private Color m_color;
        private int m_cellSize;
        public Vector2 Positon;

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

            Positon = new(5f, 2f);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle cell = new Rectangle(0, 0, m_cellSize, m_cellSize);

            for (int i = 0; i < 4; i++)
            {
                cell.X = (int)(Positon.X * m_cellSize + m_cellSize * s_figureData[m_shapeDataIndex, i, 0]);
                cell.Y = (int)(Positon.Y * m_cellSize + m_cellSize * s_figureData[m_shapeDataIndex, i, 1]);

                spriteBatch.DrawRectangleColor(cell, m_color);

            }
        }

        public int MinXComponent()
        {
            int min = int.MaxValue;

            for (int i = 0; i < 4; i++)
                min = (int)Math.Min(s_figureData[m_shapeDataIndex, i, 0] + Positon.X, min);

            return min;
        }

        public int MaxXComponent()
        {
            int max = int.MinValue;

            for (int i = 0; i < 4; i++)
                max = (int)Math.Max(s_figureData[m_shapeDataIndex, i, 0] + Positon.X, max);


            return max;
        }

        public int MaxYComponent() // lowest piece of the tetromino
        {
            int max = int.MinValue;

            for (int i = 0; i < 4; i++)
                max = (int)Math.Max(s_figureData[m_shapeDataIndex, i, 1] + Positon.Y, max);

            return max;
        }

    }
}