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

//new for winforms
using System.Diagnostics;
using System.Windows.Forms;

namespace CharacterEditor
{
    class C_Skeleton : GraphicsDeviceControl
    {
        //private SpriteBatch m_spriteBatch;
        //can assume first bone is the root/head and traverse from there
        List<C_Bone> l_bones = new List<C_Bone>();//todo: might not want to use a list down the road
//        C_Bone root; //pointer to current root bone (kinda like an iterator)
        private const uint MAX_CHILD_COUNT = 100;
        //private Texture2D m_nullTexture;
        private int m_keyFrame = 1;//keyframe 
        public const int MAX_CHAR_BONES = 12;//each keyframe contains 6 bones for now. m_keyFrame*MAX_CHAR_BONES is keyframe entry point

        private VertexPositionColor[] vertices;//holds characters vertices for all keyframes
        VertexPositionColor[] m_drawFrame;//holds current vertices calculated by interpolating keyframes
        BasicEffect basicEffect;
        int[] i_keyFrame;//keyframe index
        float[] i_animationTimer;// index --- load from file

        float time;

        //must use this for timing. cannot rely on xna gameTimer when imbedded into winforms
        Stopwatch timer;
        
        public int selectedBone;//for the editor! if bone is selected it will be stored here

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

        protected override void Initialize()
        {
            //create ortho viewport with standard screen coordinates
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter
                (0, GraphicsDevice.Viewport.Width,     // left, right
                 GraphicsDevice.Viewport.Height, 0,    // bottom, top
                 0, 1);   //near, far

            timer = Stopwatch.StartNew();
            time = 0;

            // Hook the idle event to constantly redraw our animation.
            Application.Idle += delegate { Invalidate(); };
            

            //will load assets here b/c Update() is part of Game class ( not used in winforms)
            Load();
        }

        public void Load()
        {
            LoadKeyFrames();//hard coded vertices. Will eventually load from file....gary

            //Load keyframe and animation indices
            i_keyFrame = new int[vertices.Length / (MAX_CHAR_BONES * 2)];//create index array for each keyframe
            for (int i = 0; i < i_keyFrame.Length; i++)
                i_keyFrame[i] = i * MAX_CHAR_BONES * 2;

            i_animationTimer = new float[vertices.Length / (MAX_CHAR_BONES * 2)];
            //will load from file, testing w/hardcoding now
            i_animationTimer[0] = .1f;//going from key0 to key1 should take 2 seconds  TODO: better variable name?

            m_drawFrame = new VertexPositionColor[MAX_CHAR_BONES * 2];//this is the final positon of the model for the current buffer     
        }

        public new void Update()
        {
            Vector3 moveSpeed = new Vector3(0, 0, 0);
            if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
            {
                time = 0;
                m_keyFrame = 1;
            }
            if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                time = 0;
                m_keyFrame = 0;
            }


            //if keyframe is 0, lerp to 1
            if (m_keyFrame == 0)
            {
                float lerpTime;
                lerpTime = time * i_animationTimer[0];
                if (time * i_animationTimer[0] > 1)
                    lerpTime = 1;
                for (int i = 0; i < MAX_CHAR_BONES * 2; i++)
                {
                    m_drawFrame[i].Position = Vector3.Lerp(vertices[i + i_keyFrame[0]].Position, vertices[i + i_keyFrame[1]].Position, lerpTime);
                    m_drawFrame[i].Position = Vector3.Transform(m_drawFrame[i].Position, Matrix.CreateTranslation(new Vector3(GraphicsDevice.Viewport.Width/2, GraphicsDevice.Viewport.Height/2 - 100, 0)));
                }
            }
            //if keyframe is 1, lerp to 0
            if (m_keyFrame == 1)
            {
                float lerpTime;
                lerpTime = time * i_animationTimer[0];
                if (time * i_animationTimer[0] > 1)
                    lerpTime = 1;
                for (int i = 0; i < MAX_CHAR_BONES * 2; i++)
                {
                    m_drawFrame[i].Position = Vector3.Lerp(vertices[i + i_keyFrame[1]].Position, vertices[i + i_keyFrame[0]].Position, lerpTime);
                    m_drawFrame[i].Position = Vector3.Transform(m_drawFrame[i].Position, Matrix.CreateTranslation(new Vector3(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 100, 0)));
                }
            }

        }


        protected override void Draw()
        {           
            time += (float)timer.Elapsed.Milliseconds /(1000 * 60);

            Update();//Udate function will be called here since winforms cannot use xna.Game.Update                
            
            GraphicsDevice.Clear(Color.CornflowerBlue);
          
            //editor only!! -- when selected
            for(int i = 0; i < m_drawFrame.Length; i++)
                m_drawFrame[i].Color = Color.Black;
            
            m_drawFrame[selectedBone * 2].Color = Color.Yellow;
            m_drawFrame[selectedBone * 2 + 1].Color = Color.Yellow;


            

            //TODO will use vertex buffer later to speed up rendering
            basicEffect.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList,
                m_drawFrame, 0, MAX_CHAR_BONES);
            
        }

        public void LoadKeyFrames()
        {
            //TODO: this is hard coded now, but will eventually read from file
            C_Bone tempBone = new C_Bone();
            tempBone.Name = "head";
            tempBone.Length = 30;
            tempBone.Angle = MathHelper.ToRadians(90);
            //tempBone.Position = new Vector3(GraphicsDevice.Viewport.Width / 2,
            //    GraphicsDevice.Viewport.Height / 2 - 200, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex
            tempBone.Position = Vector3.Zero;

            AddChild(null, tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);


            tempBone.Name = "right arm";
            tempBone.Length = 60;
            tempBone.Angle = MathHelper.ToRadians(20);
            tempBone.Position = new Vector3(GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[0], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);

            tempBone.Name = "right forearm";
            tempBone.Length = 50;
            tempBone.Angle = MathHelper.ToRadians(45);
            tempBone.Position = new Vector3(GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[1], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);

            tempBone.Name = "left arm";
            tempBone.Length = 60;
            tempBone.Angle = MathHelper.ToRadians(160);
            tempBone.Position = new Vector3(GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[0], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);

            tempBone.Name = "left forearm";
            tempBone.Length = 50;
            tempBone.Angle = MathHelper.ToRadians(135);
            tempBone.Position = new Vector3(GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[3], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);

            tempBone.Name = "torso";//bone 6 or [5]
            tempBone.Length = 100;
            tempBone.Angle = MathHelper.ToRadians(90);
            tempBone.Position = new Vector3(GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[0], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);

            tempBone.Name = "left upper leg";
            tempBone.Length = 75;
            tempBone.Angle = MathHelper.ToRadians(120);
            tempBone.Position = new Vector3(GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[5], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);

            tempBone.Name = "left lower leg";//bone 8
            tempBone.Length = 75;
            tempBone.Angle = MathHelper.ToRadians(100);
            tempBone.Position = new Vector3(GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[6], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);

            tempBone.Name = "right upper leg";//bone 9
            tempBone.Length = 75;
            tempBone.Angle = MathHelper.ToRadians(60);
            tempBone.Position = new Vector3(GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[5], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);

            tempBone.Name = "right lower leg";//bone 10
            tempBone.Length = 75;
            tempBone.Angle = MathHelper.ToRadians(80);
            tempBone.Position = new Vector3(GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[8], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);


            tempBone.Name = "left foot";
            tempBone.Length = 35;
            tempBone.Angle = MathHelper.ToRadians(180);
            tempBone.Position = new Vector3(GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[7], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);


            tempBone.Name = "right foot";
            tempBone.Length = 35;
            tempBone.Angle = MathHelper.ToRadians(0);
            tempBone.Position = new Vector3(GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[9], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);


            /////////////////////////////////
            //NEW FRAME
            /////////////////////////////////

            //C_Bone tempBone = new C_Bone();
            tempBone.Name = "head";
            tempBone.Length = 30;
            tempBone.Angle = MathHelper.ToRadians(45);
            //tempBone.Position = new Vector3(GraphicsDevice.Viewport.Width / 2,
            //    GraphicsDevice.Viewport.Height / 2 - 200, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex
            tempBone.Position = Vector3.Zero;

            AddChild(null, tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);


            tempBone.Name = "right arm";
            tempBone.Length = 60;
            tempBone.Angle = MathHelper.ToRadians(-20);
            tempBone.Position = new Vector3(GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[MAX_CHAR_BONES + 0], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);//todo fix algorithm for key index

            tempBone.Name = "right forearm";
            tempBone.Length = 50;
            tempBone.Angle = MathHelper.ToRadians(155);
            tempBone.Position = new Vector3(GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[MAX_CHAR_BONES + 1], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);

            tempBone.Name = "left arm";
            tempBone.Length = 60;
            tempBone.Angle = MathHelper.ToRadians(155);
            tempBone.Position = new Vector3(GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[MAX_CHAR_BONES + 0], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);

            tempBone.Name = "left forearm";
            tempBone.Length = 50;
            tempBone.Angle = MathHelper.ToRadians(130);
            tempBone.Position = new Vector3(GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[MAX_CHAR_BONES + 3], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);

            tempBone.Name = "torso";
            tempBone.Length = 100;
            tempBone.Angle = MathHelper.ToRadians(90);
            tempBone.Position = new Vector3(GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[MAX_CHAR_BONES + 0], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);

            tempBone.Name = "left upper leg";
            tempBone.Length = 75;
            tempBone.Angle = MathHelper.ToRadians(205);
            tempBone.Position = new Vector3(GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[MAX_CHAR_BONES + 5], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);

            tempBone.Name = "left ower leg";//bone 8
            tempBone.Length = 75;
            tempBone.Angle = MathHelper.ToRadians(180);
            tempBone.Position = new Vector3(GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[MAX_CHAR_BONES + 6], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);

            tempBone.Name = "right upper leg";
            tempBone.Length = 75;
            tempBone.Angle = MathHelper.ToRadians(-45);
            tempBone.Position = new Vector3(GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[MAX_CHAR_BONES + 5], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);

            tempBone.Name = "right lower leg";
            tempBone.Length = 75;
            tempBone.Angle = MathHelper.ToRadians(170);
            tempBone.Position = new Vector3(GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[MAX_CHAR_BONES + 8], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);


            tempBone.Name = "left foot";
            tempBone.Length = 35;
            tempBone.Angle = MathHelper.ToRadians(235);
            tempBone.Position = new Vector3(GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[MAX_CHAR_BONES + 7], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);


            tempBone.Name = "right foot";
            tempBone.Length = 35;
            tempBone.Angle = MathHelper.ToRadians(235);
            tempBone.Position = new Vector3(GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2, 0);//!!!!!!! MUST SET LAST to calculate 2nd vertex

            AddChild(l_bones[MAX_CHAR_BONES + 9], tempBone.Position, tempBone.Angle, tempBone.Length, tempBone.Name);
            
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
