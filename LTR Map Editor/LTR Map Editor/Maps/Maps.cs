using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
//TODO: not all these are needed, get rid of some later. Copy/pasting for now

namespace LTR_ME
{
    public class C_Maps : Microsoft.Xna.Framework.Game
    {
        //class variables here
        private Vector2 m_mousePosition, m_textPosition;
        private SpriteFont m_font1;
        private SpriteBatch m_spriteBatch;

        private String m_mouseText;
        private int m_mouseX, m_mouseY;
        private Boolean m_leftMouseDown, m_rightMouseDown;
        
        private Texture2D m_background;//TODO: textures needed for this will be listed in a file. So eventually just an array of string will be needed?

        private Texture2D m_nullTexture;
        private Color m_bgColor;
        
#region Init
        public C_Maps()
        {
            //init stuff here
            m_mousePosition = Vector2.Zero;
            m_font1 = null;
            m_spriteBatch = null;
        }

        public void Load()//load map / editor things here
        {
            m_spriteBatch = new SpriteBatch(LTR_ME.Instance.GraphicsDevice);//gfx
            m_bgColor = new Color(0.0f, 0.0f, 0.0f, 0.5f);

            m_textPosition.X = LTR_ME.Instance.GraphicsDevice.Viewport.Width / 2;//set x pos of text
            //m_textPosition.Y = ;

            m_nullTexture = LTR_ME.Instance.Content.Load<Texture2D>("sprites/nullsurface");
            m_background = LTR_ME.Instance.Content.Load<Texture2D>("sprites/buildings");
            m_font1 = LTR_ME.Instance.Content.Load<SpriteFont>("Courier New");// font;// pass array if multiple fonts need
        }
#endregion

#region Update
        public void Update()//update needed while mapeditor active (feeds mouse x/y coords etc)
        {
            //get mouse position and send to string
            MouseState ms = Mouse.GetState();
            m_mouseX = ms.X;
            m_mouseY = ms.Y;

            m_mouseText = "mouse  " + m_mouseX.ToString() + " | " + m_mouseY.ToString();
        }
#endregion 

#region Draw
        public void Draw()
        {
            Rectangle editor_bg;
            editor_bg.X = LTR_ME.Instance.GraphicsDevice.Viewport.Width / 2;
            editor_bg.Y = 0;// LTR.Instance.GraphicsDevice.Viewport.Width / 2;
            editor_bg.Width = editor_bg.X; // LTR.Instance.GraphicsDevice.Viewport.Width / 2;
            editor_bg.Height = LTR_ME.Instance.GraphicsDevice.Viewport.Height;

            Vector2 bg_pos = Vector2.Zero;

            m_spriteBatch.Begin();
            m_spriteBatch.Draw(m_background, bg_pos, Color.White);
            m_spriteBatch.Draw(m_nullTexture, editor_bg, m_bgColor);
            m_spriteBatch.DrawString(m_font1, m_mouseText, m_textPosition, Color.Red);
            m_spriteBatch.End();

        }
#endregion


    }//end class


}//end namespace
