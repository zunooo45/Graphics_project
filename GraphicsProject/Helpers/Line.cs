using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicsProject.Helpers
{
    public class Line : Shape
    {
        private VBO<Vector3> line;
        private VBO<Vector3> lineColor;
        private VBO<int> lineElements;
        private ShaderProgram program;
        private Matrix4 modelMatrix;
        private Vector3[] points;

        public Line(ShaderProgram program, params Vector3[] points)
        {
            this.points = points;
            this.program = program;
        }

        public override void OnLoad()
        {
            base.OnLoad();
            
            this.line = new VBO<Vector3>(this.points);
            this.lineColor =
                new VBO<Vector3>(Enumerable.Repeat(
                    new Vector3(0, 0, 0),
                    this.points.Length).ToArray());
            this.lineElements = new VBO<int>(new[] { 0, 1 }, BufferTarget.ElementArrayBuffer);
        }

        public override void OnRenderFrame()
        {
            this.program["model_matrix"].SetValue(this.modelMatrix);//new Vector3(1.5f, 0, 0)
            GLMethods.BindBufferToShaderAttribute(this.line, this.program, "vertexPosition");
            GLMethods.BindBufferToShaderAttribute(this.lineColor, this.program, "vertexColor");
            GLMethods.BindBuffer(this.lineElements);

            GL.DrawElements(PrimitiveType.Lines, this.lineElements.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }

        protected override void CalculateModelMatrix()
        {
            modelMatrix = Matrix4.CreateScale(Scale) *
                          Matrix4.CreateRotationX(Rotation.X) *
                          Matrix4.CreateRotationY(Rotation.Y) *
                          Matrix4.CreateRotationZ(Rotation.Z) *
                          Matrix4.CreateTranslation(Position);
        }

        public override void Dispose()
        {
            this.line.Dispose();
            this.lineColor.Dispose();
            this.lineElements.Dispose();
        }
    }
}
