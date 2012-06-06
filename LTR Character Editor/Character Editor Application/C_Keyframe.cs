using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CharacterEditor
{
    class C_Keyframe
    {
        private C_Bone[] m_boneArray { get { return m_boneArray; } set { m_boneArray = value; } }
        private float m_animationSpeed { get { return m_animationSpeed; } set { m_animationSpeed = value; } }
        
        public const int MAX_BONE_COUNT = 12;// 12 is current maximum bone count!!
        

        public C_Keyframe()
        {
            m_boneArray = new C_Bone[MAX_BONE_COUNT];
        }

        public void CopyKeyFrame(C_Keyframe priorKeyframe)//Copies keyframe data from given keyframe
        {
            for (int i = 0; i < MAX_BONE_COUNT; i++)
            {
                C_Bone tempBone = new C_Bone();
                //C# copies by reference. There's a clone function, but for now I'll do this so I don't overwrite data ='(
                //animationName = priorKeyframe.AnimationName;//***** no longer hold animation name   |  AnimationClass -> KeyframeClass -> BoneClass
                string name = priorKeyframe.m_boneArray[i].Name;
                Vector3 position = priorKeyframe.m_boneArray[i].Position;
                Vector3 positionEnd = priorKeyframe.m_boneArray[i].PositionEnd;
                float length = priorKeyframe.m_boneArray[i].Length;
                float angle = priorKeyframe.m_boneArray[i].Angle;
                uint childCount = priorKeyframe.m_boneArray[i].ChildCount;
                int parentNumber = priorKeyframe.m_boneArray[i].ParentNumber;

                tempBone.Name = name;
                tempBone.Position = position;
                tempBone.PositionEnd = positionEnd;
                tempBone.Length = length;
                tempBone.Angle = angle;
                tempBone.ChildCount = childCount;
                if (parentNumber != -1)
                    tempBone.ParentNumber = parentNumber + MAX_BONE_COUNT;//also very important. Tells which bone it is attached to;


                if (tempBone.Parent != null && tempBone.Parent.ChildCount < MAX_BONE_COUNT) //if space for child
                {
                    tempBone.Parent.ChildCount += 1;//increment parents child count
                    tempBone.Position = tempBone.Parent.PositionEnd;//set x/y and this is relative to the end of parent bone
                }

                //all done, add to array
                m_boneArray[i] = tempBone;
            }//end forloop
        }//end funciton     

        public void Update(C_Bone updatedBone, int index)//update one bone inside a keyframe
        {
            m_boneArray[index].Name = updatedBone.Name;
            m_boneArray[index].Position = updatedBone.Position;
            m_boneArray[index].PositionEnd = updatedBone.PositionEnd;
            m_boneArray[index].Length = updatedBone.Length;
            m_boneArray[index].Angle = updatedBone.Angle;
            m_boneArray[index].ChildCount = updatedBone.ChildCount;
            m_boneArray[index].ParentNumber = updatedBone.ParentNumber;

        }//end funciton     
    }
}
