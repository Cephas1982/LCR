using System;

namespace LTR_ME
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (LTR_ME game = new LTR_ME())
            {
                game.Run();
            }
        }
    }
#endif
}

