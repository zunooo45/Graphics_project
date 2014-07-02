using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using GraphicsProject.Helpers;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace GraphicsProject
{
    public class Renderer : GameWindow
    {
        private readonly Camera camera = new Camera();
        private readonly IList<Cube> cubes = new List<Cube>();
        private readonly IList<Line> lines = new List<Line>();
        private readonly Stopwatch watch = new Stopwatch();
        private float angle;
        private Vector2 lastMousePos;

        private ShaderProgram program;
        private Pyramid pyramid;
        private float time = 0.0f;
        
        public Renderer()
            : base(512, 512, new OpenTK.Graphics.GraphicsMode(32, 24, 0, 4))
        {
        }

        public override void Dispose()
        {
            base.Dispose();

            this.program.DisposeChildren = true;
            this.program.Dispose();
            this.pyramid.Dispose();

            foreach (Cube cube in this.cubes)
                cube.Dispose();

            foreach (Line line in this.lines)
                line.Dispose();
        }

        protected override void OnFocusedChanged(EventArgs e)
        {
            base.OnFocusedChanged(e);

            if (this.Focused)
                this.ResetCursor();
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            const float shapeStep = 0.5f;
            const float cameraStep = 0.3f;
            switch (e.Key)
            {
                case Key.Up:
                    this.pyramid.SetPosition(new Vector3(this.pyramid.Position.X, this.pyramid.Position.Y, this.pyramid.Position.Z - shapeStep));
                    break;
                case Key.Down:
                    this.pyramid.SetPosition(new Vector3(this.pyramid.Position.X, this.pyramid.Position.Y, this.pyramid.Position.Z + shapeStep));
                    break;
                case Key.Left:
                    this.pyramid.SetPosition(new Vector3(this.pyramid.Position.X - shapeStep, this.pyramid.Position.Y, this.pyramid.Position.Z));
                    break;
                case Key.Right:
                    this.pyramid.SetPosition(new Vector3(this.pyramid.Position.X + shapeStep, this.pyramid.Position.Y, this.pyramid.Position.Z));
                    break;
                case Key.A:
                    this.camera.Move(-cameraStep, 0f, 0f);
                    break;
                case Key.D:
                    this.camera.Move(cameraStep, 0f, 0f);
                    break;
                case Key.E:
                    this.camera.Move(0f, 0f, -cameraStep);
                    break;
                case Key.Q:
                    this.camera.Move(0f, 0f, cameraStep);
                    break;
                case Key.S:
                    this.camera.Move(0f, -cameraStep, 0f);
                    break;
                case Key.W:
                    this.camera.Move(0f, cameraStep, 0f);
                    break;
                case Key.Escape:
                    this.Exit();
                    break;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.Initialize();

            this.Title = "Hello OpenTK!";
            GL.ClearColor(Color.CornflowerBlue);
            GL.PointSize(5f);
            this.Cursor = MouseCursor.Empty;
            this.WindowBorder = WindowBorder.Hidden;
            this.WindowState = WindowState.Fullscreen;

            GL.LineWidth(10);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Viewport(0, 0, this.Width, this.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            this.watch.Stop();
            float delta = this.watch.ElapsedMilliseconds / 1000f;
            this.watch.Restart();
            this.angle += delta;


            this.program.Use();


            this.pyramid.SetAngle(this.angle);
            this.pyramid.OnRenderFrame();

            foreach (Cube cube in this.cubes)
            {
                cube.SetAngle(this.angle);
                cube.OnRenderFrame();
            }

            foreach (var line in lines)
            {
                line.OnRenderFrame();
            }
            //GL.LineWidth(10);
            //GL.Begin(PrimitiveType.Lines);
            //foreach (var cube in cubes)
            //{
            //    GL.Color4(Color.Black);
            //    GL.Vertex3(cube.Position);
            //}
            //GL.End();

            this.SwapBuffers();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            this.SetProjectionMatrix(this.GetFieldOfView());
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            if (this.Focused)
            {
                Vector2 delta = this.lastMousePos - new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);

                this.camera.AddRotation(delta.X, delta.Y);
                this.ResetCursor();
            }

            this.SetProjectionMatrix(this.camera.GetViewMatrix() * this.GetFieldOfView());
        }

        private Matrix4 GetFieldOfView()
        {
            return Matrix4.CreatePerspectiveFieldOfView(0.45f, (float)this.Width / this.Height, 0.1f, 1000.0f);
        }

        private void Initialize()
        {
            this.program = new ShaderProgram(File.ReadAllText("vs.glsl"), File.ReadAllText("fs.glsl"));
            this.program.Use();
            this.SetProjectionMatrix(this.GetFieldOfView());
            this.SetViewMatrix(Matrix4.LookAt(new Vector3(0, 0, 10), Vector3.Zero, new Vector3(0, 1, 0)));

            this.pyramid = new Pyramid(this.program);
            this.pyramid.OnLoad();
            this.pyramid.SetPosition(new Vector3(-1.5f, 0, 0));

            var rand = new Random();
            for (int i = 0; i < 10; i++)
            {
                var cube = new Cube(this.program);
                cube.OnLoad();
                cube.RotationSpeed = (float)(rand.NextDouble() + 1);
                cube.SetPosition(new Vector3(rand.Next(-8, 8), rand.Next(-8, 8), rand.Next(-100, -1)));
                //cube.SetScale(new Vector3((float)rand.NextDouble()));
                this.cubes.Add(cube);
            }

            for (int i = 0; i < 9; i++)
            {
                var firstPos = this.cubes[i].Position;
                var secondPos = this.cubes[i+1].Position;
                var line = new Line(this.program, firstPos, secondPos);
                line.OnLoad();
                this.lines.Add(line);
            }

            this.watch.Start();
        }

        private void ResetCursor()
        {
            OpenTK.Input.Mouse.SetPosition(this.Bounds.Left + this.Bounds.Width / 2, this.Bounds.Top + this.Bounds.Height / 2);
            this.lastMousePos = new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
        }

        private void SetProjectionMatrix(Matrix4 matrix)
        {
            this.program["projection_matrix"].SetValue(matrix);
        }

        private void SetViewMatrix(Matrix4 matrix)
        {
            this.program["view_matrix"].SetValue(matrix);
        }
    }
}