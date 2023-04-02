

public static class EntryPoint
{
    static void Main(string[] args)
    {
        using var game = new Tetris.MyGame();
        game.Run();
    }
}