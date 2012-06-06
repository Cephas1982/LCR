using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CharacterEditor
{
    class C_Animation
    {
        //each animation class will have a list of keyframes, 
        //and each keyframe contains 1 array of bones
        private List<C_Keyframe> l_keyfame { get { return l_keyfame; } set { l_keyfame = value; } }
        private String m_name;


        public C_Animation()
        {
            l_keyfame = new List<C_Keyframe>();
        }

        public void Load(string animationName)
        {
            //this will load all keyframes that belong to the requested animation
        }        


    }


}
