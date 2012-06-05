using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace CharacterEditor

{  
    class C_Bone
    {

        private string m_name, m_animationName;// bone name, and the animation bone belongs to
        private Vector3 m_positionStart, m_positionEnd;//bone start positon
        private float m_length, m_angle;//bone length and angle (radians)
        private uint m_childCount;//how many child bones does this bone have?
        public const int MAX_CHILD_BONES = 8;//maximum children
        private int m_parentNumber;
        private int m_keyFrame;//which key this bone belongs to
        

        C_Bone m_parent = null;//reference to parent
        C_Bone[] m_children = null;//reference to children

        public C_Bone()
        {
            m_name = "";
            m_positionStart = Vector3.Zero;
            m_length = 0;
            m_angle = 0;
            m_childCount = 0;
            m_parentNumber = -1;
            m_keyFrame = -1;

            m_parent = null;
            m_children = new C_Bone[MAX_CHILD_BONES];
        }

        public string Name//name of bone
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

        public string AnimationName//name of animation bone belongs to
        {
            set
            {
                m_animationName = value;
            }
            get
            {
                return m_animationName;
            }
        }

        public int KeyFrame
        {
            set
            {
                m_keyFrame = value;
            }
            get
            {
                return m_keyFrame;
            }
        }

        public int ParentNumber
        {
            set
            {
                m_parentNumber = value;
            }
            get
            {
                return m_parentNumber;
            }
        }

        public Vector3 Position
        {
            set
            {
                m_positionStart = value;
                m_positionEnd.X = (float)Math.Cos(m_angle) * m_length;//get end point X value
                m_positionEnd.Y = (float)Math.Sin(m_angle) * m_length;//get Y

                m_positionEnd += m_positionStart;
            }
            get
            {
                return m_positionStart;
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
                m_positionEnd += m_positionStart;

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
