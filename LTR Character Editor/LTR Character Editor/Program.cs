using System;

namespace LTR_Character_Editor
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (LTR_CE game = new LTR_CE())
            {
                game.Run();
            }
        }
    }
#endif
}

