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
    public class C_MapEditor : Microsoft.Xna.Framework.Game
    {
        //class variables here
        private Vector2 m_mousePosition, m_textPosition;
        private SpriteFont m_font1;
        private SpriteBatch m_spriteBatch;

        private String m_mouseText;
        private int m_mouseX, m_mouseY;
        private Boolean m_leftMouseDown = false, m_rightMouseDown = false;//lol, you can do this in C#?
        
        private Texture2D m_background;//TODO: textures needed for this will be listed in a file. So eventually just an array of string will be needed?

        private Texture2D m_nullTexture;
        private Color m_bgColor;

        //container for bg objects
        private List<C_BackgroundObject> l_bgObjects = new List<C_BackgroundObject>();
        
#region Init
        public C_MapEditor()
        {
            //init stuff here
            m_mousePosition = Vector2.Zero;
            m_font1 = null;
            m_spriteBatch = null;

            //temp data for now TODO: load from file later
            C_BackgroundObject tempBG = new C_BackgroundObject();
            tempBG.Rect = new Rectangle(0, 0, 500, 500);
            l_bgObjects.Add(tempBG);
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
            MouseState ms = Mouse.GetState();//get mouse state
            //get mouse button states  //TODO put this in a function (or mouse class <- sounds better)
            if (ms.LeftButton == ButtonState.Pressed)
                m_leftMouseDown = true;
            else
                m_leftMouseDown = false;

            if (ms.RightButton == ButtonState.Pressed)
                m_rightMouseDown = true;
            else
                m_rightMouseDown = false;

            //get mouse position and send to string            
            m_mouseX = ms.X;
            m_mouseY = ms.Y;

            m_mouseText = "mouse  " + m_mouseX.ToString() + " | " + m_mouseY.ToString();//debug output

            //write code so that when an object is clicked + held. You can drag that object
            //have a bg object container
            //collision check if mouse is over object inside above container
            //when user left clicks
                //get object mouse is over
                //while left click held change objects x/y coords (use change of distance from last update, so users can drag from anywhere)
                //do feint highlight of selected object<-- add later
                    //if leftclick is down then rightclick pressed - reset to old position
                    //if leftclick released - stop dragging and save objects new x/y coordinates
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

            //Go through objects in the list and if it's on the screen draw it// TODO: check that it's on the screen
            m_spriteBatch.Begin();
            foreach (C_BackgroundObject bgObject in l_bgObjects)
                m_spriteBatch.Draw(m_nullTexture, bgObject.Rect, Color.Red);
            m_spriteBatch.End();
        }
#endregion


    }//end class


}//end namespace
