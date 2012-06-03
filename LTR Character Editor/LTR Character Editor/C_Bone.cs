using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace LTR_Character_Editor
{  
    class C_Bone
    {

        private string m_name;// = new char[30];
        private Vector3 m_position, m_positionEnd;//bone start positon
        private float m_length, m_angle;//bone length and angle (radians)
        private uint m_childCount;//how many child bones does this bone have?
        public const int MAX_CHILD_BONES = 8;//maximum children

        C_Bone m_parent;//reference to parent
        C_Bone[] m_children;//reference to children

        public C_Bone()
        {
            m_name = "";
            m_position = Vector3.Zero;
            m_length = 0;
            m_angle = 0;
            m_childCount = 0;

            m_parent = null;
            m_children = new C_Bone[MAX_CHILD_BONES];
        }

        public string Name
        {
            set
            {
                m_name = value;
            }
            get
            {
                return m_name;
            }
        }

        public Vector3 Position
        {
            set
            {
                m_position = value;
                m_positionEnd.X = (float)Math.Cos(m_angle) * m_length;//get end point X value
                m_positionEnd.Y = (float)Math.Sin(m_angle) * m_length;//get Y

                m_positionEnd += m_position;
            }
            get
            {
                return m_position;
            }
        }

        public Vector3 PositionEnd
        {
            set
            {
                m_positionEnd = value;
            }
            get
            {
                m_positionEnd.X = (float)Math.Cos(m_angle) * m_length;//get end point X value
                m_positionEnd.Y = (float)Math.Sin(m_angle) * m_length;//get Y
                m_positionEnd += m_position;

                return m_positionEnd;
            }
        }

        public float Length
        {
            set
            {
                m_length = value;
            }
            get
            {
                return m_length;
            }
        }

        public float Angle
        {
            set
            {
                m_angle = value;
            }
            get
            {
                return m_angle;
            }
        }
        
        public uint ChildCount
        {
            set
            {
                m_childCount = value;
            }
            get
            {
                return m_childCount;
            }
        }

        public C_Bone Parent
        {
            set
            {
                m_parent = value;
            }
            get
            {
                return m_parent;
            }
        }



   

    }
}
