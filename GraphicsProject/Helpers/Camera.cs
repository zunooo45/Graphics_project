using System;
using OpenTK;

namespace GraphicsProject.Helpers
{
    public class Camera
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Orientation = new Vector3((float)Math.PI, 0f, 0f);
        public float MoveSpeed = 2f;
        public float MouseSensitivity = 0.001f;
        
        public Matrix4 GetViewMatrix()
        {
            var lookat = new Vector3
                         {
                             X = (float)(Math.Sin(this.Orientation.X) * Math.Cos(this.Orientation.Y)),
                             Y = (float)Math.Sin(this.Orientation.Y),
                             Z = (float)(Math.Cos(this.Orientation.X) * Math.Cos(this.Orientation.Y))
                         };

            return Matrix4.LookAt(this.Position, this.Position + lookat, Vector3.UnitY);
        }

        public void Move(float x, float y, float z)
        {
            var offset = new Vector3();

            var forward = new Vector3((float)Math.Sin(this.Orientation.X), 0, (float)Math.Cos(this.Orientation.X));
            var right = new Vector3(-forward.Z, 0, forward.X);

            offset += x * right;
            offset += y * forward;
            offset.Y += z;

            offset.NormalizeFast();
            offset = Vector3.Multiply(offset, this.MoveSpeed);

            this.Position += offset;

            var handle = this.CameraChanged;
            if (handle != null)
                handle();
        }

        public void AddRotation(float x, float y)
        {
            x = x * this.MouseSensitivity;
            y = y * this.MouseSensitivity;

            this.Orientation.X = (this.Orientation.X + x) % ((float)Math.PI * 2.0f);
            this.Orientation.Y = Math.Max(Math.Min(this.Orientation.Y + y, (float)Math.PI / 2.0f - 0.1f), (float)-Math.PI / 2.0f + 0.1f);

            var handle = this.CameraChanged;
            if (handle != null)
                handle();
        }

        public event Action CameraChanged;
    }
}
