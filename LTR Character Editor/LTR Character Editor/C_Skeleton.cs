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

namespace LTR_Character_Editor
{
    class C_Skeleton : Microsoft.Xna.Framework.Game
    {
        //private SpriteBatch m_spriteBatch;
        //can assume first bone is the root/head and traverse from there
        List<C_Bone> l_bones = new List<C_Bone>();//todo: might not want to use a list down the road
//        C_Bone root; //pointer to current root bone (kinda like an iterator)
        private const uint MAX_CHILD_COUNT = 100;
        private Texture2D m_nullTexture;
        private int m_keyFrame = 0;//keyframe 
        public const int MAX_CHAR_BONES = 6;//each keyframe contains 6 bones for now. m_keyFrame*MAX_CHAR_BONES is keyframe entry point

        private VertexPositionColor[] vertices;//holds characters vertices for all keyframes
        VertexPositionColor[] m_drawFrame;//holds current vertices calculated by interpolating keyframes
        BasicEffect basicEffect;
        int[] i_keyFrame;//keyframe index
        float[] i_animationTimer;// index --- load from file

        float time, aTime;

        public C_Skeleton()
        {  
      
        }

        //Skeleton functions
        public void AddChild(C_Bone targetBone, Vector3 position, float angle, float length, string name)
        {
            C_Bone childBone = new C_Bone();//create new bone and update its values before adding to bone list
            //add child to the target bone (parameter)
            //child bone holds pointers to the to children and it's single parent (if applicable)
            if (targetBone == null)
            {//if no parent
                childBone.Parent = null;//this is the parent, so parent pointer is null
                childBone.Position = position; //sets where to start drawing Head
            }
            else if (targetBone.ChildCount < MAX_CHILD_COUNT) //if space for child
            {
                childBone.Parent = targetBone;//set parent
                targetBone.ChildCount += 1;//increment parents child count
                childBone.Position = childBone.Parent.PositionEnd;//set x/y and this is relative to the end of parent bone
            }
            // TODO: ERROR CHECKING CODE HERE
                

            //set data
            childBone.Angle = angle;
            childBone.Length = length;
            //childBone.Name = name;
            childBone.ChildCount = 0; //not really needed b/c of C_Bone constructor

            if (name.Length > 0)
                childBone.Name = name;
            else
                childBone.Name = "bone " + l_bones.Count().ToString();

            l_bones.Add(childBone);


        }

        public void DumpTree(C_Bone bone)
        {
            //prints bones hierarchy from given bone
            //recursively go through nodes
        }

        public void Init()
        {
            //create ortho viewport with standard screen coordinates
            basicEffect = new BasicEffect(LTR_CE.Instance.GraphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter
                (0, LTR_CE.Instance.GraphicsDevice.Viewport.Width,     // left, right
                LTR_CE.Instance.GraphicsDevice.Viewport.Height, 0,    // bottom, top
                0, 1);   

            
            base.Initialize();
        }

        public void Load()
        {
            SetFrame1();

            //Load keyframe and animation indices
            i_keyFrame = new int[vertices.Length / (MAX_CHAR_BONES * 2)];//create index array for each keyframe
            for (int i = 0; i < i_keyFrame.Length; i++)
                i_keyFrame[i] = i * MAX_CHAR_BONES * 2;

            i_animationTimer = new float[vertices.Length / (MAX_CHAR_BONES * 2)];
            //will load from file, testing w/hardcoding now
            i_animationTimer[0] = .5f;//going from key0 to key1 should take 2 seconds  TODO: better variable name?
        }

        public void Update(GameTime gameTime)
        {
            Vector3 moveSpeed = new Vector3(0, 0, 0);
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                //moveSpeed.X -= 3;
                time = 0;
                m_keyFrame = 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                //moveSpeed.X += 3;
                time = 0;
                m_keyFrame = 0;
            }
            

            for(int i = 0; i < vertices.Count(); i++)
                vertices[i].Position += moveSpeed;

            //offset of draw function is the current keyframe * max bone count * 2(cause 2 vertices per bone)
            int offset = m_keyFrame * MAX_CHAR_BONES * 2;

            m_drawFrame = new VertexPositionColor[MAX_CHAR_BONES * 2];

            time += (float)gameTime.ElapsedGameTime.Milliseconds/100;
            
            //if keyframe is 0, lerp to 1
            if (m_keyFrame == 0)
            {
                float lerpTime;
                lerpTime = time * i_animationTimer[0];
                if (time * i_animationTimer[0] > 1)
                    lerpTime = 1;
                for (int i = 0; i < 12; i++)
                {
                    m_drawFrame[i].Position = Vector3.Lerp(vertices[i + i_keyFrame[0]].Position, vertices[i + i_keyFrame[1]].Position, lerpTime);
                }
            }
            //if keyframe is 1, lerp to 0
            if (m_keyFrame == 1)
            {
                float lerpTime;
                lerpTime = time * i_animationTimer[0];
                if (time * i_animationTimer[0] > 1)
                    lerpTime = 1;
                for (int i = 0; i < 12; i++)
                    m_drawFrame[i].Position = Vector3.Lerp(vertices[i + i_keyFrame[1]].Position, vertices[i + i_keyFrame[0]].Position, lerpTime);
            }
        }

        public void Draw()
        {

            //offset of draw function is the current keyframe * max bone count * 2(cause 2 vertices per bone)
           // int offset = m_keyFrame * MAX_CHAR_BONES * 2;
            

            //TODO will use vertex buffer later to speed up rendering
            basicEffect.CurrentTechnique.Passes[0].Apply();
            LTR_CE.Instance.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList,
                m_drawFrame, 0, MAX_CHAR_BONES);
            
        }

        public void SetFrame1()
        {
            //TODO: this is hard coded now, but will eventually read from file
            C_Bone tempBone = new C_Bone();
            tempBone.Name = "head";
            tempBone.Length = 30;
            tempBone.Angle = MathHelper.ToRadians(90);
            tempBone.Position = new Vector3(LTR_CE.Instance.GraphicsDevice.Viewport.Width / 2,
                LTR_CE.Instance.GraphicsDevice.Viewport.Height / 2 - 200, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(null, tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);


            tempBone.Name = "right arm";
            tempBone.Length = 60;
            tempBone.Angle = MathHelper.ToRadians(0);
            tempBone.Position = new Vector3(LTR_CE.Instance.GraphicsDevice.Viewport.Width / 2,
                LTR_CE.Instance.GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[0], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);

            tempBone.Name = "right forearm";
            tempBone.Length = 50;
            tempBone.Angle = MathHelper.ToRadians(45);
            tempBone.Position = new Vector3(LTR_CE.Instance.GraphicsDevice.Viewport.Width / 2,
                LTR_CE.Instance.GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[1], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);

            tempBone.Name = "left arm";
            tempBone.Length = 60;
            tempBone.Angle = MathHelper.ToRadians(180);
            tempBone.Position = new Vector3(LTR_CE.Instance.GraphicsDevice.Viewport.Width / 2,
                LTR_CE.Instance.GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[0], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);

            tempBone.Name = "left forearm";
            tempBone.Length = 50;
            tempBone.Angle = MathHelper.ToRadians(135);
            tempBone.Position = new Vector3(LTR_CE.Instance.GraphicsDevice.Viewport.Width / 2,
                LTR_CE.Instance.GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[3], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);

            tempBone.Name = "torso";
            tempBone.Length = 100;
            tempBone.Angle = MathHelper.ToRadians(90);
            tempBone.Position = new Vector3(LTR_CE.Instance.GraphicsDevice.Viewport.Width / 2,
                LTR_CE.Instance.GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[0], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);
            /////////////////////////////////
            //NEW FRAME
            /////////////////////////////////

            //C_Bone tempBone = new C_Bone();
            tempBone.Name = "head";
            tempBone.Length = 30;
            tempBone.Angle = MathHelper.ToRadians(45);
            tempBone.Position = new Vector3(LTR_CE.Instance.GraphicsDevice.Viewport.Width / 2,
                LTR_CE.Instance.GraphicsDevice.Viewport.Height / 2 - 200, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(null, tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);


            tempBone.Name = "right arm";
            tempBone.Length = 60;
            tempBone.Angle = MathHelper.ToRadians(-45);
            tempBone.Position = new Vector3(LTR_CE.Instance.GraphicsDevice.Viewport.Width / 2,
                LTR_CE.Instance.GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[MAX_CHAR_BONES + 0], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);//todo fix algorithm for key index

            tempBone.Name = "right forearm";
            tempBone.Length = 50;
            tempBone.Angle = MathHelper.ToRadians(45);
            tempBone.Position = new Vector3(LTR_CE.Instance.GraphicsDevice.Viewport.Width / 2,
                LTR_CE.Instance.GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[MAX_CHAR_BONES + 1], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);

            tempBone.Name = "left arm";
            tempBone.Length = 60;
            tempBone.Angle = MathHelper.ToRadians(180);
            tempBone.Position = new Vector3(LTR_CE.Instance.GraphicsDevice.Viewport.Width / 2,
                LTR_CE.Instance.GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[MAX_CHAR_BONES + 0], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);

            tempBone.Name = "left forearm";
            tempBone.Length = 50;
            tempBone.Angle = MathHelper.ToRadians(225);
            tempBone.Position = new Vector3(LTR_CE.Instance.GraphicsDevice.Viewport.Width / 2,
                LTR_CE.Instance.GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[MAX_CHAR_BONES + 3], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);

            tempBone.Name = "torso";
            tempBone.Length = 100;
            tempBone.Angle = MathHelper.ToRadians(90);
            tempBone.Position = new Vector3(LTR_CE.Instance.GraphicsDevice.Viewport.Width / 2,
                LTR_CE.Instance.GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[MAX_CHAR_BONES + 0], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);
            
            
            
            vertices = new VertexPositionColor[l_bones.Count() * 2];//2 vertices per bone
            //after reading bone data load vertices
            for (int i = 0; i < l_bones.Count(); i++)
            {
                vertices[i * 2].Position = l_bones[i].Position;
                vertices[i * 2].Color = Color.Black;
                vertices[i * 2 + 1].Position = l_bones[i].PositionEnd;
                vertices[i * 2 + 1].Color = Color.Black;
            }
        }
    }
}
