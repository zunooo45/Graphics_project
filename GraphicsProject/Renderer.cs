using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsProject.Helpers;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Glu = OpenTK.Graphics.Glu;

namespace GraphicsProject
{
    public class Renderer : GameWindow
    {
        float time = 0.0f;

        private Camera cam = new Camera();
        private Vector2 lastMousePos = new Vector2();
        
        private ShaderProgram program;
        private Pyramid pyramid;
        private Stopwatch watch = new Stopwatch();
        private float angle;
        List<Cube> cubes = new List<Cube>(); 

        public Renderer()
            : base(512, 512, new OpenTK.Graphics.GraphicsMode(32, 24, 0, 4))
        {
            
        }

        void initProgram()
        {
            //GL.GenBuffers(1, out ibo_elements);

            //shaders.Add("default", new ShaderProgram("vs.glsl", "fs.glsl", true));


            //objects.Add(new Cube());
            //objects.Add(new Cube());
            //Random rand = new Random();

            //    var c = new ColorCube(new Vector3((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble()));
            //    c.Position = new Vector3(rand.Next(-4, 4), rand.Next(-4, 4), rand.Next(-8, 8));
            //    c.Rotation = new Vector3(rand.Next(0, 6), rand.Next(0, 6), rand.Next(0, 6));
            //    c.Scale = Vector3.One * ((float)rand.NextDouble() + 0.2f);

            //    objects.Add(c);
            //}

            program = new ShaderProgram(File.ReadAllText("vs.glsl"), File.ReadAllText("fs.glsl"));
            program.Use();
            program["projection_matrix"].SetValue(Matrix4.CreatePerspectiveFieldOfView(0.45f, (float)this.Width / this.Height, 0.1f, 1000.0f));
            program["view_matrix"].SetValue(Matrix4.LookAt(new Vector3(0, 0, 10), Vector3.Zero, new Vector3(0, 1, 0)));

            this.pyramid = new Pyramid(program);
            this.pyramid.Camera = cam;
            this.pyramid.OnLoad();
            this.pyramid.SetPosition(new Vector3(-1.5f, 0, 0));

            var rand = new Random();
            for (int i = 0; i < 200; i++)
            {
                var cube = new Cube(program);
                cube.Camera = cam;
                cube.OnLoad();
                cube.SetPosition(new Vector3(rand.Next(-8, 8), rand.Next(-8, 8), rand.Next(-100, -1)));
                cube.SetScale(new Vector3((float)rand.NextDouble()));
                cubes.Add(cube);
            }

            this.watch.Start();
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            initProgram();

            Title = "Hello OpenTK!";
            GL.ClearColor(Color.CornflowerBlue);
            GL.PointSize(5f);
            Cursor = MouseCursor.Empty;
            WindowBorder = WindowBorder.Hidden;
            WindowState = WindowState.Fullscreen;

            GL.LineWidth(10);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (Focused)
            {
                Vector2 delta = lastMousePos - new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);

                cam.AddRotation(delta.X, delta.Y);
                ResetCursor();
            }


        //    foreach (Volume v in objects)
        //    {
        //        v.CalculateModelMatrix();
        //        v.ViewProjectionMatrix = cam.GetViewMatrix() *
        //                                 Matrix4.CreatePerspectiveFieldOfView(1.3f, ClientSize.Width / (float)ClientSize.Height, 1.0f, 40.0f);
        //        v.ModelViewProjectionMatrix = v.ModelMatrix * v.ViewProjectionMatrix;
        //    }


        //    GL.UseProgram(shaders[activeShader].ProgramID);

        //    GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            const float step = 0.5f;
            switch (e.Key)
            {
                case Key.A:
                    this.pyramid.SetPosition(new Vector3(this.pyramid.Position.X - step, this.pyramid.Position.Y, this.pyramid.Position.Z));
                    break;
                case Key.D:
                    this.pyramid.SetPosition(new Vector3(this.pyramid.Position.X + step, this.pyramid.Position.Y, this.pyramid.Position.Z));
                    break;
                case Key.S:
                    this.pyramid.SetPosition(new Vector3(this.pyramid.Position.X, this.pyramid.Position.Y, this.pyramid.Position.Z + step));
                    break;
                case Key.W:
                    this.pyramid.SetPosition(new Vector3(this.pyramid.Position.X, this.pyramid.Position.Y, this.pyramid.Position.Z - step));
                    break;
                case Key.Escape:
                    this.Exit();
                    break;
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            this.watch.Stop();
            var delta = this.watch.ElapsedMilliseconds / 1000f;
            this.watch.Restart();
            this.angle += delta;


            program.Use();


            this.pyramid.SetAngle(angle);
            this.pyramid.OnRenderFrame();

            foreach (var cube in cubes)
            {
                cube.SetAngle(angle);
                cube.OnRenderFrame();
            }

            this.SwapBuffers();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            
            program["projection_matrix"].SetValue(Matrix4.CreatePerspectiveFieldOfView(0.45f, (float)this.Width / this.Height, 0.1f, 1000.0f));
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if (e.KeyChar == 27)
            {
                Exit();
            }

            switch (e.KeyChar)
            {
                case 'w':
                    cam.Move(0f, 0.1f, 0f);
                    break;
                case 'a':
                    cam.Move(-0.1f, 0f, 0f);
                    break;
                case 's':
                    cam.Move(0f, -0.1f, 0f);
                    break;
                case 'd':
                    cam.Move(0.1f, 0f, 0f);
                    break;
                case 'q':
                    cam.Move(0f, 0f, 0.1f);
                    break;
                case 'e':
                    cam.Move(0f, 0f, -0.1f);
                    break;
            }
        }

        void ResetCursor()
        {
            OpenTK.Input.Mouse.SetPosition(Bounds.Left + Bounds.Width / 2, Bounds.Top + Bounds.Height / 2);
            lastMousePos = new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
        }

        protected override void OnFocusedChanged(EventArgs e)
        {
            base.OnFocusedChanged(e);

            if (Focused)
            {
                ResetCursor();
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            this.program.DisposeChildren = true;
            this.program.Dispose();
            this.pyramid.Dispose(); 

            foreach (var cube in cubes)
                cube.Dispose();
        }
    }
}
