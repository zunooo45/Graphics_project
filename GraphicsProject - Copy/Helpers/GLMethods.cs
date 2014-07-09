﻿using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicsProject.Helpers
{
    public class GLMethods
    {
        // pre-allocate the float[] for matrix data
        private static float[] matrixFloat = new float[16];
        private static int[] id = new int[1];

        private static int _version = 0;
        private static int currentProgram = 0;

        public static int CurrentProgram { get { return currentProgram; } }

        /// <summary>
        /// Shortcut for quickly generating a single buffer id without creating an array to
        /// pass to the gl function.  Calls GL.GenBuffers(1, id).
        /// </summary>
        /// <returns>The ID of the generated buffer.  0 on failure.</returns>
        public static int GenBuffer()
        {
            id[0] = 0;
            GL.GenBuffers(1, id);
            return id[0];
        }

        /// <summary>
        /// Shortcut for quickly generating a single texture id without creating an array to
        /// pass to the gl function.  Calls GL.GenTexture(1, id).
        /// </summary>
        /// <returns>The ID of the generated texture.  0 on failure.</returns>
        public static int GenTexture()
        {
            id[0] = 0;
            GL.GenTextures(1, id);
            return id[0];
        }

        /// <summary>
        /// Shortcut for quickly generating a single vertex array id without creating an array to
        /// pass to the gl function.  Calls GL.GenVertexArrays(1, id).
        /// </summary>
        /// <returns>The ID of the generated vertex array.  0 on failure.</returns>
        public static int GenVertexArray()
        {
            id[0] = 0;
            GL.GenVertexArrays(1, id);
            return id[0];
        }

        /// <summary>
        /// Shortcut for quickly generating a single framebuffer object without creating an array
        /// to pass to the gl function.  Calls GL.GenFramebuffers(1, id).
        /// </summary>
        /// <returns>The ID of the generated framebuffer.  0 on failure.</returns>
        public static int GenFramebuffer()
        {
            int[] id = new int[1];
            GL.GenFramebuffers(1, id);
            return id[0];
        }

        /// <summary>
        /// Shortcut for quickly generating a single renderbuffer object without creating an array
        /// to pass to the gl function.  Calls GL.GenRenderbuffers(1, id).
        /// </summary>
        /// <returns>The ID of the generated framebuffer.  0 on failure.</returns>
        public static int GenRenderbuffer()
        {
            id[0] = 0;
            GL.GenRenderbuffers(1, id);
            return id[0];
        }

        /// <summary>
        /// Gets the program info from a shader program.
        /// </summary>
        /// <param name="program">The ID of the shader program.</param>
        public static string GetProgramInfoLog(int program)
        {
            int[] length = new int[1];
            GL.GetProgram(program, GetProgramParameterName.InfoLogLength, length);
            if (length[0] == 0) return String.Empty;
            System.Text.StringBuilder sb = new System.Text.StringBuilder(length[0]);
            GL.GetProgramInfoLog(program, sb.Capacity, out length[0], sb);
            return sb.ToString();
        }

        /// <summary>
        /// Gets the program info from a shader program.
        /// </summary>
        /// <param name="program">The ID of the shader program.</param>
        public static string GetShaderInfoLog(int shader)
        {
            int[] length = new int[1];
            GL.GetShader(shader, ShaderParameter.InfoLogLength, length);
            if (length[0] == 0) return String.Empty;
            System.Text.StringBuilder sb = new System.Text.StringBuilder(length[0]);
            GL.GetShaderInfoLog(shader, sb.Capacity, out length[0], sb);
            return sb.ToString();
        }

        /// <summary>
        /// Replaces the source code in a shader object.
        /// </summary>
        /// <param name="shader">Specifies the handle of the shader object whose source code is to be replaced.</param>
        /// <param name="source">Specifies a string containing the source code to be loaded into the shader.</param>
        public static void ShaderSource(int shader, string source)
        {
            GL.ShaderSource(shader, 1, new string[] { source }, new int[] { source.Length });
        }

        /// <summary>
        /// Creates and initializes a buffer object's data store.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">Specifies the target buffer object.</param>
        /// <param name="size">Specifies the size in bytes of the buffer object's new data store.</param>
        /// <param name="data">Specifies a pointer to data that will be copied into the data store for initialization, or NULL if no data is to be copied.</param>
        /// <param name="usage">Specifies expected usage pattern of the data store.</param>
        public static void BufferData<T>(BufferTarget target, Int32 size, [In, Out] T[] data, BufferUsageHint usage)
            where T : struct
        {
            GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                GL.BufferData(target, new IntPtr(size), data_ptr.AddrOfPinnedObject(), usage);
            }
            finally
            {
                data_ptr.Free();
            }
        }

        /// <summary>
        /// Creates a standard VBO of type T.
        /// </summary>
        /// <typeparam name="T">The type of the data being stored in the VBO (make sure it's byte aligned).</typeparam>
        /// <param name="target">The VBO BufferTarget (usually ArrayBuffer or ElementArrayBuffer).</param>
        /// <param name="data">The data to store in the VBO.</param>
        /// <param name="hint">The buffer usage hint (usually StaticDraw).</param>
        /// <returns>The buffer ID of the VBO on success, 0 on failure.</returns>
        public static int CreateVBO<T>(BufferTarget target, [In, Out] T[] data, BufferUsageHint hint)
            where T : struct
        {
            int vboHandle = GL.GenBuffer();
            if (vboHandle == 0) return 0;

            GL.BindBuffer(target, vboHandle);
            GL.BufferData<T>(target, (IntPtr)(data.Length * Marshal.SizeOf(typeof(T))), data, hint);
            GL.BindBuffer(target, 0);
            return vboHandle;
        }

        /// <summary>
        /// Creates a standard VBO of type T where the length of the VBO is less than or equal to the length of the data.
        /// </summary>
        /// <typeparam name="T">The type of the data being stored in the VBO (make sure it's byte aligned).</typeparam>
        /// <param name="target">The VBO BufferTarget (usually ArrayBuffer or ElementArrayBuffer).</param>
        /// <param name="data">The data to store in the VBO.</param>
        /// <param name="hint">The buffer usage hint (usually StaticDraw).</param>
        /// <param name="length">The length of the VBO (will take the first 'length' elements from data).</param>
        /// <returns>The buffer ID of the VBO on success, 0 on failure.</returns>
        public static int CreateVBO<T>(BufferTarget target, [In, Out] T[] data, BufferUsageHint hint, int length)
            where T : struct
        {
            var vboHandle = GL.GenBuffer();
            if (vboHandle == 0) return 0;

            GL.BindBuffer(target, vboHandle);
            GL.BufferData<T>(target, (IntPtr)(length * Marshal.SizeOf(typeof(T))), data, hint);
            GL.BindBuffer(target, 0);
            return vboHandle;
        }

        #region CreateInterleavedVBO
        public static int CreateInterleavedVBO(BufferTarget target, Vector3[] data1, Vector3[] data2, BufferUsageHint hint)
        {
            if (data2.Length != data1.Length) throw new Exception("Data lengths must be identical to construct an interleaved VBO.");

            float[] interleaved = new float[data1.Length * 6];

            for (int i = 0, j = 0; i < data1.Length; i++)
            {
                interleaved[j++] = data1[i].X;
                interleaved[j++] = data1[i].Y;
                interleaved[j++] = data1[i].Z;

                interleaved[j++] = data2[i].X;
                interleaved[j++] = data2[i].Y;
                interleaved[j++] = data2[i].Z;
            }

            return CreateVBO<float>(target, interleaved, hint);
        }

        public static int CreateInterleavedVBO(BufferTarget target, Vector3[] data1, Vector3[] data2, Vector2[] data3, BufferUsageHint hint)
        {
            if (data2.Length != data1.Length || data3.Length != data1.Length) throw new Exception("Data lengths must be identical to construct an interleaved VBO.");

            float[] interleaved = new float[data1.Length * 8];

            for (int i = 0, j = 0; i < data1.Length; i++)
            {
                interleaved[j++] = data1[i].X;
                interleaved[j++] = data1[i].Y;
                interleaved[j++] = data1[i].Z;

                interleaved[j++] = data2[i].X;
                interleaved[j++] = data2[i].Y;
                interleaved[j++] = data2[i].Z;

                interleaved[j++] = data3[i].X;
                interleaved[j++] = data3[i].Y;
            }

            return CreateVBO<float>(target, interleaved, hint);
        }

        public static int CreateInterleavedVBO(BufferTarget target, Vector3[] data1, Vector3[] data2, Vector3[] data3, BufferUsageHint hint)
        {
            if (data2.Length != data1.Length || data3.Length != data1.Length) throw new Exception("Data lengths must be identical to construct an interleaved VBO.");

            float[] interleaved = new float[data1.Length * 9];

            for (int i = 0, j = 0; i < data1.Length; i++)
            {
                interleaved[j++] = data1[i].X;
                interleaved[j++] = data1[i].Y;
                interleaved[j++] = data1[i].Z;

                interleaved[j++] = data2[i].X;
                interleaved[j++] = data2[i].Y;
                interleaved[j++] = data2[i].Z;

                interleaved[j++] = data3[i].X;
                interleaved[j++] = data3[i].Y;
                interleaved[j++] = data3[i].Z;
            }

            return CreateVBO<float>(target, interleaved, hint);
        }

        public static int CreateInterleavedVBO(BufferTarget target, Vector3[] data1, Vector3[] data2, Vector3[] data3, Vector2[] data4, BufferUsageHint hint)
        {
            if (data2.Length != data1.Length || data3.Length != data1.Length) throw new Exception("Data lengths must be identical to construct an interleaved VBO.");

            float[] interleaved = new float[data1.Length * 11];

            for (int i = 0, j = 0; i < data1.Length; i++)
            {
                interleaved[j++] = data1[i].X;
                interleaved[j++] = data1[i].Y;
                interleaved[j++] = data1[i].Z;

                interleaved[j++] = data2[i].X;
                interleaved[j++] = data2[i].Y;
                interleaved[j++] = data2[i].Z;

                interleaved[j++] = data3[i].X;
                interleaved[j++] = data3[i].Y;
                interleaved[j++] = data3[i].Z;

                interleaved[j++] = data4[i].X;
                interleaved[j++] = data4[i].Y;
            }

            return CreateVBO<float>(target, interleaved, hint);
        }
        #endregion

        /// <summary>
        /// Creates a vertex array object based on a series of attribute arrays and and attribute names.
        /// </summary>
        /// <param name="program">The shader program that contains the attributes to be bound to.</param>
        /// <param name="vbo">The VBO containing all of the attribute data.</param>
        /// <param name="sizes">An array of sizes which correspond to the size of each attribute.</param>
        /// <param name="types">An array of the attribute pointer types.</param>
        /// <param name="targets">An array of the buffer targets.</param>
        /// <param name="names">An array of the attribute names.</param>
        /// <param name="stride">The stride of the VBO.</param>
        /// <param name="eboHandle">The element buffer handle.</param>
        /// <returns>The vertex array object (VAO) ID.</returns>
        public static int CreateVAO(ShaderProgram program, int vbo, int[] sizes, VertexAttribPointerType[] types, BufferTarget[] targets, string[] names, int stride, int eboHandle)
        {
            int vaoHandle = GL.GenVertexArray();
            GL.BindVertexArray(vaoHandle);

            int offset = 0;

            for (int i = 0; i < names.Length; i++)
            {
                GL.EnableVertexAttribArray(i);
                GL.BindBuffer(targets[i], vbo);
                GL.VertexAttribPointer(i, sizes[i], types[i], true, stride, new IntPtr(offset));
                GL.BindAttribLocation(program.ProgramID, i, names[i]);
            }

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, eboHandle);
            GL.BindVertexArray(0);

            return vaoHandle;
        }

        /// <summary>
        /// Gets the current OpenGL version (returns a cached result on subsequent calls).
        /// </summary>
        /// <returns>The current OpenGL version, or 0 on an error.</returns>
        public static int Version()
        {
            if (_version != 0) return _version;	// cache the version information

            try
            {
                string version = GL.GetString(StringName.Version);
                return (_version = int.Parse(version.Substring(0, version.IndexOf('.'))));
            }
            catch (Exception)
            {
                Console.WriteLine("Error while retrieving the OpenGL version.");
                return 0;
            }
        }

        /// <summary>
        /// Installs a program object as part of current rendering state.
        /// </summary>
        /// <param name="Program">Specifies the handle of the program object whose executables are to be used as part of current rendering state.</param>
        public static void UseProgram(ShaderProgram Program)
        {
            GL.UseProgram(Program.ProgramID);
        }

        /// <summary>
        /// Bind a named texture to a texturing target
        /// </summary>
        /// <param name="Texture">Specifies the texture.</param>
        public static void BindTexture(Texture Texture)
        {
            GL.BindTexture(Texture.TextureTarget, Texture.TextureID);
        }

        /// <summary>
        /// Return the value of the selected parameter.
        /// </summary>
        /// <param name="name">Specifies the parameter value to be returned.</param>
        public static int GetInteger(GetPName name)
        {
            int[] temp = new int[1];
            GL.GetInteger(name, temp);
            return temp[0];
        }

        /// <summary>
        /// Get the index of a uniform block in the provided shader program.
        /// Note:  This method will use the provided shader program, so make sure to
        /// store which program is currently active and reload it if required.
        /// </summary>
        /// <param name="program">The shader program that contains the uniform block.</param>
        /// <param name="uniformBlockName">The uniform block name.</param>
        /// <returns>The index of the uniform block.</returns>
        public static int GetUniformBlockIndex(ShaderProgram program, string uniformBlockName)
        {
            program.Use();  // take care of a crash that can occur on NVIDIA drivers by using the program first
            return GL.GetUniformBlockIndex(program.ProgramID, uniformBlockName);
        }

        /// <summary>
        /// Binds a VBO based on the buffer target.
        /// </summary>
        /// <param name="buffer">The VBO to bind.</param>
        public static void BindBuffer<T>(VBO<T> buffer)
            where T : struct
        {
            GL.BindBuffer(buffer.BufferTarget, buffer.vboID);
        }

        /// <summary>
        /// Binds a VBO to a shader attribute.
        /// </summary>
        /// <param name="buffer">The VBO to bind to the shader attribute.</param>
        /// <param name="program">The shader program whose attribute will be bound to.</param>
        /// <param name="attributeName">The name of the shader attribute to be bound to.</param>
        public static void BindBufferToShaderAttribute<T>(VBO<T> buffer, ShaderProgram program, string attributeName)
            where T : struct
        {
            int location = (int)GL.GetAttribLocation(program.ProgramID, attributeName);

            GL.EnableVertexAttribArray(location);
            BindBuffer(buffer);
            GL.VertexAttribPointer(location, buffer.Size, buffer.PointerType, true, Marshal.SizeOf(typeof(T)), IntPtr.Zero);
        }

        /// <summary>
        /// Delete a single OpenGL buffer.
        /// </summary>
        /// <param name="buffer">The OpenGL buffer to delete.</param>
        public static void DeleteBuffer(int buffer)
        {
            id[0] = buffer;
            GL.DeleteBuffers(1, id);
            id[0] = 0;
        }

        /// <summary>
        /// Set a uniform mat4 in the shader.
        /// Uses a cached float[] to reduce memory usage.
        /// </summary>
        /// <param name="location">The location of the uniform in the shader.</param>
        /// <param name="param">The Matrix4 to load into the shader uniform.</param>
        public static void UniformMatrix4fv(int location, Matrix4 param)
        {
            // use the statically allocated float[] for setting the uniform
            matrixFloat[0] = param.Row0.X; matrixFloat[1] = param.Row0.Y; matrixFloat[2] = param.Row0.Z; matrixFloat[3] = param.Row0.W;
            matrixFloat[4] = param.Row1.X; matrixFloat[5] = param.Row1.Y; matrixFloat[6] = param.Row1.Z; matrixFloat[7] = param.Row1.W;
            matrixFloat[8] = param.Row2.X; matrixFloat[9] = param.Row2.Y; matrixFloat[10] = param.Row2.Z; matrixFloat[11] = param.Row2.W;
            matrixFloat[12] = param.Row3.X; matrixFloat[13] = param.Row3.Y; matrixFloat[14] = param.Row3.Z; matrixFloat[15] = param.Row3.W;

            GL.UniformMatrix4(location, 1, false, matrixFloat);
        }
    }
}
