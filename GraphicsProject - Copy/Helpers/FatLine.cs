﻿using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicsProject.Helpers
{
    public class FatLine : Shape
    {
        private VBO<Vector3> cube;
        private VBO<Vector3> cubeColor;
        private VBO<int> cubeElements;
        private ShaderProgram program;
        private readonly Vector3 start;
        private readonly Vector3 end;
        private Matrix4 modelMatrix;

        public float RotationSpeed { get; set; }

        public FatLine(ShaderProgram program, Vector3 start, Vector3 end)
        {
            this.program = program;
            this.start = start;
            this.end = end;

            Vector3.CalculateAngle(start, end);
        }

        public override void OnLoad()
        {
            base.OnLoad();

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
            this.cube.Dispose();
            this.cubeColor.Dispose();
            this.cubeElements.Dispose();
        }
    }
}
