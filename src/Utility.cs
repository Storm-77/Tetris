using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris;

public static class Utility
{
    private static Texture2D s_pixelTexture;
    public static void Init(GraphicsDevice gd)
    {
        s_pixelTexture = new Texture2D(gd, 1, 1);

        Color[] data = new Color[1];
        data[0] = Color.White;
        s_pixelTexture.SetData(data);
    }

    public static void Shutdown()
    {
        s_pixelTexture.Dispose();
    }

    public static void DrawRectangleOutline(this SpriteBatch spriteBatch, Rectangle rectangle, Color color, int thickness = 1)
    {
        // Draw the top line
        spriteBatch.Draw(s_pixelTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, thickness), color);

        // Draw the bottom line
        spriteBatch.Draw(s_pixelTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - thickness, rectangle.Width, thickness), color);

        // Draw the left line
        spriteBatch.Draw(s_pixelTexture, new Rectangle(rectangle.X, rectangle.Y, thickness, rectangle.Height), color);

        // Draw the right line
        spriteBatch.Draw(s_pixelTexture, new Rectangle(rectangle.X + rectangle.Width - thickness, rectangle.Y, thickness, rectangle.Height), color);

    }

    public static void DrawRectangleColor(this SpriteBatch spriteBatch, Rectangle rectangle, Color color)
    {
        spriteBatch.Draw(s_pixelTexture, rectangle, color);
    }

    public static void DrawRectangleFull(this SpriteBatch spriteBatch, Rectangle rectangle, Color fillColor, Color outlineColor, int thickness = 1)
    {
        spriteBatch.Draw(s_pixelTexture, rectangle, fillColor);
        spriteBatch.DrawRectangleOutline(rectangle, outlineColor, thickness);
    }



}
