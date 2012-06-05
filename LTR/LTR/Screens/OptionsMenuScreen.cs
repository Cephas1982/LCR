#region File Description
//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
#endregion

namespace LTR
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        MenuEntry option1;
        MenuEntry option2;
        MenuEntry option3;
        MenuEntry option4;

        enum Ungulate
        {
            White,
            Asian,
            Gary
        }

        static Ungulate currentUngulate = Ungulate.White;
        static string[] languages = { "Pig Latin", "Klingon", "Meskin" };
        static int currentLanguage = 0;
        static bool frobnicate = false;
        static int time = 15;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base("OPTIONS")
        {
            // Create our menu entries.
            option1 = new MenuEntry(string.Empty);
            option2 = new MenuEntry(string.Empty);
            option3 = new MenuEntry(string.Empty);
            option4 = new MenuEntry(string.Empty);

            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            option1.Selected += UngulateMenuEntrySelected;
            option2.Selected += LanguageMenuEntrySelected;
            option3.Selected += FrobnicateMenuEntrySelected;
            option4.Selected += ElfMenuEntrySelected;
            back.Selected += OnCancel;
            
            // Add entries to the menu.
            MenuEntries.Add(option1);
            MenuEntries.Add(option2);
            MenuEntries.Add(option3);
            MenuEntries.Add(option4);
            MenuEntries.Add(back);
        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            option1.Text = "Preferred prostitue: " + currentUngulate;
            option2.Text = "Language: " + languages[currentLanguage];
            option3.Text = "A2M: " + (frobnicate ? "hell yeh" : "no");
            option4.Text = "Time: " + time + " minits";
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Ungulate menu entry is selected.
        /// </summary>
        void UngulateMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentUngulate++;

            if (currentUngulate > Ungulate.Gary)
                currentUngulate = 0;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Language menu entry is selected.
        /// </summary>
        void LanguageMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentLanguage = (currentLanguage + 1) % languages.Length;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Frobnicate menu entry is selected.
        /// </summary>
        void FrobnicateMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            frobnicate = !frobnicate;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Elf menu entry is selected.
        /// </summary>
        void ElfMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            time += 15;

            if (time > 60)
                time = 0;
            SetMenuEntryText();
        }


        #endregion
    }
}
