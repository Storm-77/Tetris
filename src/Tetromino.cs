using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Tetris
{

    public partial class Tetromino
    {
        private int m_shapeDataIndex;
        private Color m_color;
        public Vector2 Positon;
        private Grid m_myGrid;

        public Tetromino(Grid belongsTo, TetrominoShape shape = TetrominoShape.None)
        {
            m_myGrid = belongsTo;

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
            Rectangle cell = new Rectangle(0, 0, (int)m_myGrid.CellSize, (int)m_myGrid.CellSize);

            for (int i = 0; i < 4; i++)
            {
                cell.X = (int)(Positon.X * m_myGrid.CellSize + m_myGrid.CellSize * s_figureData[m_shapeDataIndex, i, 0]);
                cell.Y = (int)(Positon.Y * m_myGrid.CellSize + m_myGrid.CellSize * s_figureData[m_shapeDataIndex, i, 1]);

                spriteBatch.DrawRectangleColor(cell, m_color);

            }
        }

        public bool Collides(ref bool[,] grid)
        {
            for (int i = 0; i < 4; i++)
            {

                int X = (int)(Positon.X + s_figureData[m_shapeDataIndex, i, 0]);
                int Y = (int)(Positon.Y + s_figureData[m_shapeDataIndex, i, 1]);

                if (grid[X, Y])
                {
                    Positon.Y--;
                    return true;
                }
            }

            return false;
        }

        public void Land(ref bool[,] grid)
        {
            for (int i = 0; i < 4; i++)
            {
                int X = (int)(Positon.X + s_figureData[m_shapeDataIndex, i, 0]);
                int Y = (int)(Positon.Y + s_figureData[m_shapeDataIndex, i, 1]);

                grid[X, Y] = true;

            }
        }

        public void CheckOuterBorders()
        {
            int max = int.MinValue;

            for (int i = 0; i < 4; i++)
                max = (int)Math.Max(s_figureData[m_shapeDataIndex, i, 0] + Positon.X, max);


            if (max > m_myGrid.Width - 1)
                Positon.X--;

            int min = int.MaxValue;

            for (int i = 0; i < 4; i++)
                min = (int)Math.Min(s_figureData[m_shapeDataIndex, i, 0] + Positon.X, min);

            if (min < 0)
                Positon.X++;
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