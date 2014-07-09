using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicsProject.Helpers
{
    public class Pyramid : Shape
    {
        private ShaderProgram program;
        private VBO<Vector3> pyramid;
        private VBO<Vector3> pyramidColor;
        private VBO<int> pyramidElements;
        private float angle;
        private Matrix4 modelMatrix;

        public Pyramid(ShaderProgram program)
        {
            this.program = program;
        }

        public override void OnLoad()
        {
            base.OnLoad();

            this.pyramid = new VBO<Vector3>(new[]
                                            {
                                                new Vector3(0, 1, 0), new Vector3(-1, -1,  1), new Vector3( 1, -1,  1),
                                                new Vector3(0, 1, 0), new Vector3( 1, -1,  1), new Vector3( 1, -1, -1),
                                                new Vector3(0, 1, 0), new Vector3( 1, -1, -1), new Vector3(-1, -1, -1),
                                                new Vector3(0, 1, 0), new Vector3(-1, -1, -1), new Vector3(-1, -1,  1),
                                            });

            this.pyramidColor = new VBO<Vector3>(new[]
                                                 {
                                                     new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1),
                                                     new Vector3(1, 0, 0), new Vector3(0, 0, 1), new Vector3(0, 1, 0),
                                                     new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1),
                                                     new Vector3(1, 0, 0), new Vector3(0, 0, 1), new Vector3(0, 1, 0)
                                                 });
            this.pyramidElements = new VBO<int>(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }, BufferTarget.ElementArrayBuffer);
        }

        public override void OnRenderFrame()
        {
            this.program["model_matrix"].SetValue(this.modelMatrix);

            GLMethods.BindBufferToShaderAttribute(this.pyramid, this.program, "vertexPosition");
            GLMethods.BindBufferToShaderAttribute(this.pyramidColor, this.program, "vertexColor");
            GLMethods.BindBuffer(this.pyramidElements);

            GL.DrawElements(PrimitiveType.Triangles, this.pyramidElements.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }

        protected override void CalculateModelMatrix()
        {
            modelMatrix = Matrix4.CreateScale(Scale) *
                          Matrix4.CreateRotationX(Rotation.X) *
                          Matrix4.CreateRotationY(this.angle) *
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
            this.pyramid.Dispose();
            this.pyramidColor.Dispose();
            this.pyramidElements.Dispose();
        }
    }
}
