using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicsProject.Helpers
{
    public class Cube : Shape
    {
        private VBO<Vector3> cube;
        private VBO<Vector3> cubeColor;
        private VBO<int> cubeElements;
        private ShaderProgram program;
        private Matrix4 modelMatrix;
        private float angle;

        public Cube(ShaderProgram program)
        {
            this.program = program;
        }

        public override void OnLoad()
        {
            this.cube = new VBO<Vector3>(new[]
                                         {
                                             new Vector3( 1,  1, -1), new Vector3(-1,  1, -1), new Vector3(-1,  1,  1), new Vector3( 1,  1,  1),
                                             new Vector3( 1, -1,  1), new Vector3(-1, -1,  1), new Vector3(-1, -1, -1), new Vector3( 1, -1, -1),
                                             new Vector3( 1,  1,  1), new Vector3(-1,  1,  1), new Vector3(-1, -1,  1), new Vector3( 1, -1,  1),
                                             new Vector3( 1, -1, -1), new Vector3(-1, -1, -1), new Vector3(-1,  1, -1), new Vector3( 1,  1, -1),
                                             new Vector3(-1,  1,  1), new Vector3(-1,  1, -1), new Vector3(-1, -1, -1), new Vector3(-1, -1,  1),
                                             new Vector3( 1,  1, -1), new Vector3( 1,  1,  1), new Vector3( 1, -1,  1), new Vector3( 1, -1, -1),
                                         });
            this.cubeColor =
                new VBO<Vector3>(new[]
                                 {
                                     new Vector3( 0,  1,  0),    new Vector3( 0,  1,  0),    new Vector3( 0,  1,  0),    new Vector3( 0,  1,  0),
                                     new Vector3( 1,  0.5f,  0), new Vector3( 1,  0.5f,  0), new Vector3( 0.5f,  1,  0), new Vector3( 1,  0.5f,  0),
                                     new Vector3( 1,  0,  0),    new Vector3( 1,  0,  0),    new Vector3( 1,  0,  0),    new Vector3( 1,  0,  0),
                                     new Vector3( 1,  1,  0),    new Vector3( 1,  1,  0),    new Vector3( 1,  1,  0),    new Vector3( 1,  1,  0),
                                     new Vector3( 0,  0,  1),    new Vector3( 0,  0,  1),    new Vector3( 0,  0,  1),    new Vector3( 0,  0,  1),
                                     new Vector3( 1,  0,  1),    new Vector3( 1,  0,  1),    new Vector3( 1,  0,  1),    new Vector3( 1,  0,  1),
                                 });
            this.cubeElements = new VBO<int>(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 }, BufferTarget.ElementArrayBuffer);
        }

        public override void OnRenderFrame()
        {
            this.program["model_matrix"].SetValue(this.modelMatrix);//new Vector3(1.5f, 0, 0)
            GLMethods.BindBufferToShaderAttribute(this.cube, this.program, "vertexPosition");
            GLMethods.BindBufferToShaderAttribute(this.cubeColor, this.program, "vertexColor");
            GLMethods.BindBuffer(this.cubeElements);

            GL.DrawElements(PrimitiveType.Quads, this.cubeElements.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }

        private void CalculateModelMatrix()
        {
            modelMatrix = Matrix4.CreateScale(Scale) *
                Matrix4.CreateRotationX(this.angle) *
                Matrix4.CreateRotationY(this.angle / 2) * 
                Matrix4.CreateRotationZ(Rotation.Z) *
                Matrix4.CreateTranslation(Position);
        }

        public void SetAngle(float angle)
        {
            this.angle = angle;
            this.CalculateModelMatrix();
        }

        public override void Dispose()
        {
            this.cube.Dispose();
            this.cubeColor.Dispose();
            this.cubeElements.Dispose();
        }
    }
}
