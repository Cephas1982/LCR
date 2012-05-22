using System;

namespace LTR
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (LTR game = new LTR())
            {
                game.Run();
            }
        }
    }
#endif
}

