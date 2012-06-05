using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CharacterEditor
{
    class C_Keyframe
    {
        private C_Bone[] m_boneArray;


        public C_Keyframe()
        {
            m_boneArray = new C_Bone[12];// 12 is current maximum bone count!!
        }
    }
}
