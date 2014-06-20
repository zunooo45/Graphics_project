using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace GraphicsProject
{
    public class Camera
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Orientation = new Vector3((float)Math.PI, 0f, 0f);
        public float MoveSpeed = 0.2f;
        public float MouseSensitivity = 0.01f;
        
        public Matrix4 GetViewMatrix()
        {
            var lookat = new Vector3
                         {
                             X = (float)(Math.Sin(this.Orientation.X) * Math.Cos(this.Orientation.Y)),
                             Y = (float)Math.Sin(this.Orientation.Y),
                             Z = (float)(Math.Cos(this.Orientation.X) * Math.Cos(this.Orientation.Y))
                         };

            return Matrix4.LookAt(Position, Position + lookat, Vector3.UnitY);
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
            offset = Vector3.Multiply(offset, MoveSpeed);

            Position += offset;
        }

        public void AddRotation(float x, float y)
        {
            x = x * MouseSensitivity;
            y = y * MouseSensitivity;

            Orientation.X = (Orientation.X + x) % ((float)Math.PI * 2.0f);
            Orientation.Y = Math.Max(Math.Min(Orientation.Y + y, (float)Math.PI / 2.0f - 0.1f), (float)-Math.PI / 2.0f + 0.1f);
        }

    }
}
