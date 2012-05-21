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

namespace LCR
{
    public class C_Maps : Microsoft.Xna.Framework.Game
    {
        //class variables here
        private Vector2 m_mousePosition, m_textPosition;
        private SpriteFont m_font1;
        private SpriteBatch m_spriteBatch;
        private String m_mouseText;
        private int m_mouseX, m_mouseY;


        public C_Maps()
        {
            //init stuff here
            //m_textPosition.X = GraphicsDevice.Viewport.Width / 2; //Vector2.Zero;
            //m_textPosition.Y = GraphicsDevice.Viewport.Height / 2; //Vector2.Zero;
            m_mousePosition = Vector2.Zero;
            m_font1 = null;
            m_spriteBatch = null;
        }

        public void Load(GraphicsDeviceManager graphics, SpriteFont font)//load map / editor things here
        {
            m_spriteBatch = new SpriteBatch(graphics.GraphicsDevice);//gfx 
            

            m_textPosition.X = graphics.GraphicsDevice.Viewport.Width / 2; 
            //m_textPosition.Y = ;

            m_font1 = font;// pass array if multiple fonts needed
        }

        public void Update()//update needed while mapeditor active (feeds mouse x/y coords etc)
        {
            //get mouse position and send to string
            MouseState ms = Mouse.GetState();
            m_mouseX = ms.X;
            m_mouseY = ms.Y;

            m_mouseText = "mouse  " + m_mouseX.ToString() + " | " + m_mouseY.ToString();
        }

        public void Draw()
        {
            m_spriteBatch.Begin();
            m_spriteBatch.DrawString(m_font1, m_mouseText, m_textPosition, Color.Red);
            m_spriteBatch.End();
        
        }
    }


}
