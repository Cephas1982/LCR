using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LTR//will change to LTR at some point 
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    
    public class LTR : Microsoft.Xna.Framework.Game
    {
        static Game instance;//singleton so we can access LTR from subclasses

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //temp for testing
        Texture2D m_spriteTexture;//holds texture from content
        Vector2 m_position = Vector2.Zero;
        
        //end temp
        C_Maps MapSystem;

        //FYI: borrowed code below
        // By preloading any assets used by UI rendering, we avoid framerate glitches
        // when they suddenly need to be loaded in the middle of a menu transition.
        static readonly string[] preloadAssets =
        {
            "gradient", "Courier New", "Sprites/buildings",
        };

        public LTR()
        {
            instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //show the mouse
            IsMouseVisible = true;
 
        }

        //singleton class to access LTR variables throughout menus
        public static Game Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {           
            
            // TODO: Add your initialization logic here
            //Set to  720p
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            //init our classes
            MapSystem = new C_Maps();
            Mouse.WindowHandle = this.Window.Handle;//let mouse know window size/position
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use singleton to access LTR content pipeline
            m_spriteTexture = this.Content.Load<Texture2D>("sprites/buildings");//todo: function to load from file


            MapSystem.Load();
            //end test
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit  TODO: function to handle inputs
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here
            
            MapSystem.Update();
            //end our logic

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();//TODO: remove these 3 lines
            spriteBatch.Draw(m_spriteTexture, m_position, Color.White);
            spriteBatch.End();

            MapSystem.Draw();
            //end test code

            base.Draw(gameTime);
        }
    }
}
