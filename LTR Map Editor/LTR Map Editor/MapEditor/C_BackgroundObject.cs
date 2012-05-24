using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LTR_ME
{
    class C_BackgroundObject: Microsoft.Xna.Framework.Game//Container class for background objects
    {
        private Rectangle m_rect;//this will hold current x/y coords and width/height
        private string m_name = "placeholder";//TODO: do we want to reference by string, or int?


        public Rectangle Rect
        {
            set
            {
                m_rect = value;
            }

            get
            {
                return m_rect;
            }
        }
    }

  
}
