using System;
using OpenTK;

namespace GraphicsProject.Helpers
{
    public abstract class Shape : IDisposable
    {
        private Vector3 position;
        private Vector3 rotation;
        private Vector3 scale;

        public Shape()
        {
            this.position = Vector3.Zero;
            this.rotation = Vector3.Zero;
            this.scale = Vector3.One;
        }

        public Vector3 Position
        {
            get { return this.position; }
            private set { this.position = value; }
        }

        public Vector3 Rotation
        {
            get { return this.rotation; }
            private set { this.rotation = value; }
        }

        public Vector3 Scale
        {
            get { return this.scale; }
            private set { this.scale = value; }
        }

        public void SetPosition(Vector3 position)
        {
            this.position = position;
            this.CalculateModelMatrix();
        }

        public void SetRotation(Vector3 rotation)
        {
            this.rotation = rotation;
            this.CalculateModelMatrix();
        }

        public void SetScale(Vector3 scale)
        {
            this.scale = scale;
            this.CalculateModelMatrix();
        }

        protected virtual void CalculateModelMatrix()
        { }

        public abstract void OnLoad();
        public abstract void OnRenderFrame();
        public abstract void Dispose();
    }
}
