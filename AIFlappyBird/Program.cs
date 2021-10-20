using System;

namespace AIFlappyBird
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new AIFlappyBird())
                game.Run();
        }
    }
}
