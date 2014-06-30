using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicsProject.Helpers
{
    public enum ParamType 
    { 
        Uniform,
        Attribute
    }

    public class ProgramParam
    {
        #region Variables
        private Type type;
        private int location;
        private int programid;
        private ParamType ptype;
        private string name;
        #endregion

        #region Properties
        /// <summary>
        /// Specifies the C# equivalent of the GLSL data type.
        /// </summary>
        public Type Type { get { return this.type; } }

        /// <summary>
        /// Specifies the location of the parameter in the OpenGL program.
        /// </summary>
        public int Location { get { return this.location; } }

        /// <summary>
        /// Specifies the OpenGL program ID.
        /// </summary>
        public int Program { get { return this.programid; } }

        /// <summary>
        /// Specifies the parameter type (either attribute or uniform).
        /// </summary>
        public ParamType ParamType { get { return this.ptype; } }

        /// <summary>
        /// Specifies the case-sensitive name of the parameter.
        /// </summary>
        public string Name { get { return this.name; } }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a program parameter with a given type and name.
        /// The location must be found after the program is compiled
        /// by using the GetLocation(ShaderProgram Program) method.
        /// </summary>
        /// <param name="Type">Specifies the C# equivalent of the GLSL data type.</param>
        /// <param name="ParamType">Specifies the parameter type (either attribute or uniform).</param>
        /// <param name="Name">Specifies the case-sensitive name of the parameter.</param>
        public ProgramParam(Type Type, ParamType ParamType, string Name)
        {
            this.type = Type;
            this.ptype = ParamType;
            this.name = Name;
        }

        /// <summary>
        /// Creates a program parameter with a type, name, program and location.
        /// </summary>
        /// <param name="Type">Specifies the C# equivalent of the GLSL data type.</param>
        /// <param name="ParamType">Specifies the parameter type (either attribute or uniform).</param>
        /// <param name="Name">Specifies the case-sensitive name of the parameter.</param>
        /// <param name="Program">Specifies the OpenGL program ID.</param>
        /// <param name="Location">Specifies the location of the parameter.</param>
        public ProgramParam(Type Type, ParamType ParamType, string Name, int Program, int Location)
            : this(Type, ParamType, Name)
        {
            this.programid = Program;
            this.location = Location;
        }
        #endregion

        #region GetLocation
        /// <summary>
        /// Gets the location of the parameter in a compiled OpenGL program.
        /// </summary>
        /// <param name="Program">Specifies the shader program that contains this parameter.</param>
        public void GetLocation(ShaderProgram Program)
        {
            Program.Use();
            if (this.programid == 0)
            {
                this.programid = Program.ProgramID;
                this.location = (this.ptype == ParamType.Uniform ? Program.GetUniformLocation(this.name) : Program.GetAttributeLocation(this.name));
            }
        }
        #endregion

        #region SetValue Overrides
        public void SetValue(bool param)
        {
            if (this.Type != typeof(bool)) throw new Exception(string.Format("SetValue({0}) was given a bool.", this.Type));
            GL.Uniform1(this.location, (param) ? 1 : 0);
        }

        public void SetValue(int param)
        {
            if (this.Type != typeof(int) && this.Type != typeof(Texture)) throw new Exception(string.Format("SetValue({0}) was given a int.", this.Type));
            GL.Uniform1(this.location, param);
        }

        public void SetValue(float param)
        {
            if (this.Type != typeof(float)) throw new Exception(string.Format("SetValue({0}) was given a float.", this.Type));
            GL.Uniform1(this.location, param);
        }

        public void SetValue(Vector2 param)
        {
            if (this.Type != typeof(Vector2)) throw new Exception(string.Format("SetValue({0}) was given a Vector2.", this.Type));
            GL.Uniform2(this.location, param.X, param.Y);
        }

        public void SetValue(Vector3 param)
        {
            if (this.Type != typeof(Vector3)) throw new Exception(string.Format("SetValue({0}) was given a Vector3.", this.Type));
            GL.Uniform3(this.location, param.X, param.Y, param.Z);
        }

        public void SetValue(Vector4 param)
        {
            if (this.Type != typeof(Vector4)) throw new Exception(string.Format("SetValue({0}) was given a Vector4.", this.Type));
            GL.Uniform4(this.location, param.X, param.Y, param.Z, param.W);
        }

        

        public void SetValue(Matrix4 param)
        {
            if (this.Type != typeof(Matrix4)) 
                throw new Exception(string.Format("SetValue({0}) was given a Matrix4.", this.Type));

            GL.UniformMatrix4(this.location, false, ref param);
        }

        public void SetValue(float[] param)
        {
            if (this.Type != typeof(Matrix4)) 
                throw new Exception(string.Format("SetValue({0}) was given a Matrix4.", this.Type));
            if (param.Length != 16) 
                throw new Exception(string.Format("Expected a float[] of 16 for a Matrix4, but instead got {0}.", param.Length));
            
            GL.UniformMatrix4(this.location, 1, false, param);
        }

        /*public void SetValue(Texture param)
        {
            if (Type != typeof(Texture)) throw new Exception(string.Format("SetValue({0}) was given a Texture.", Type));
            GL.Uniform1(location, param.Binding);
        }*/
        #endregion
    }

    public class Shader : IDisposable
    {
        #region Properties
        /// <summary>
        /// Specifies the OpenGL ShaderID.
        /// </summary>
        public int ShaderID { get; private set; }

        /// <summary>
        /// Specifies the type of shader.
        /// </summary>
        public ShaderType ShaderType { get; private set; }

        /// <summary>
        /// Contains all of the attributes and uniforms parsed from this shader source.
        /// </summary>
        public ProgramParam[] ShaderParams { get; private set; }

        /// <summary>
        /// Returns GL.GetShaderInfoLog(ShaderID), which contains any compilation errors.
        /// </summary>
        public string ShaderLog
        {
            get { return GL.GetShaderInfoLog(this.ShaderID); }
        }
        #endregion

        #region Constructor and Destructor
        /// <summary>
        /// Compiles a shader, which can be either vertex, fragment or geometry.
        /// </summary>
        /// <param name="source">Specifies the source code of the shader object.</param>
        /// <param name="type">Specifies the type of shader to create (either vertex, fragment or geometry).</param>
        public Shader(string source, ShaderType type)
        {
            this.ShaderType = type;
            this.ShaderID = GL.CreateShader(this.ShaderType);

            GL.ShaderSource(this.ShaderID, source);
            GL.CompileShader(this.ShaderID);

            this.GetParams(source);
        }

        ~Shader()
        {
            if (this.ShaderID != 0) System.Diagnostics.Debug.Fail("Shader was not disposed of properly.");
        }
        #endregion

        #region GetParams
        /// <summary>
        /// Parses the shader source code and finds all attributes and uniforms
        /// to cache their location for speedy lookup.
        /// </summary>
        /// <param name="source">Specifies the source code of the shader.</param>
        private void GetParams(string source)
        {
            List<ProgramParam> shaderParams = new List<ProgramParam>();
            Regex searchTerm = new Regex("(uniform\\s\\S+\\s\\S+;|attribute\\s\\S+\\s\\S+;)");
            MatchCollection Matches = searchTerm.Matches(source);

            foreach (Match pMatch in Matches)
            {
                // [0] = attibute/uniform, [1] = type, [2] = name
                string[] param = pMatch.Value.Split(' ');
                if (param.Length != 3) continue;

                ParamType type = (param[0].ToLower() == "attribute") ? ParamType.Attribute : ParamType.Uniform;
                string name = param[2].Trim(';');

                switch (param[1].ToLower())
                {
                    case "float": shaderParams.Add(new ProgramParam(typeof(float), type, name)); break;
                    case "bool": shaderParams.Add(new ProgramParam(typeof(bool), type, name)); break;
                    case "int": shaderParams.Add(new ProgramParam(typeof(int), type, name)); break;
                    case "vec2": shaderParams.Add(new ProgramParam(typeof(Vector2), type, name)); break;
                    case "vec3": shaderParams.Add(new ProgramParam(typeof(Vector3), type, name)); break;
                    case "vec4": shaderParams.Add(new ProgramParam(typeof(Vector4), type, name)); break;
                    case "mat4": shaderParams.Add(new ProgramParam(typeof(Matrix4), type, name)); break;
                    case "sampler2d": shaderParams.Add(new ProgramParam(typeof(Texture), type, name)); break;
                    case "sampler2dshadow": shaderParams.Add(new ProgramParam(typeof(Texture), type, name)); break;
                    case "sampler1d": shaderParams.Add(new ProgramParam(typeof(Texture), type, name)); break;
                    case "sampler1dshadow": shaderParams.Add(new ProgramParam(typeof(Texture), type, name)); break;
                    case "sampler3d": shaderParams.Add(new ProgramParam(typeof(Texture), type, name)); break;
                    case "sampler2darray": shaderParams.Add(new ProgramParam(typeof(Texture), type, name)); break;
                    case "sampler2darrayshadow": shaderParams.Add(new ProgramParam(typeof(Texture), type, name)); break;
                    default: throw new Exception(string.Format("Unsupported GLSL type {0}", param[1]));
                }
            }

            this.ShaderParams = shaderParams.ToArray();
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            if (this.ShaderID != 0)
            {
                GL.DeleteShader(this.ShaderID);
                this.ShaderID = 0;
            }
        }
        #endregion
    }

    public class ShaderProgram : IDisposable
    {
        #region Properties
        /// <summary>
        /// Specifies the OpenGL shader program ID.
        /// </summary>
        public int ProgramID { get; private set; }

        /// <summary>
        /// Specifies the vertex shader used in this program.
        /// </summary>
        public Shader VertexShader { get; private set; }

        /// <summary>
        /// Specifies the fragment shader used in this program.
        /// </summary>
        public Shader FragmentShader { get; private set; }

        /// <summary>
        /// Specifies whether this program will dispose of the child 
        /// vertex/fragment programs when the IDisposable method is called.
        /// </summary>
        public bool DisposeChildren { get; set; }

        private Dictionary<string, ProgramParam> shaderParams;

        /// <summary>
        /// Queries the shader parameter hashtable to find a matching attribute/uniform.
        /// </summary>
        /// <param name="name">Specifies the case-sensitive name of the shader attribute/uniform.</param>
        /// <returns>The requested attribute/uniform, or null on a failure.</returns>
        public ProgramParam this[string name]
        {
            get { return this.shaderParams.ContainsKey(name) ? this.shaderParams[name] : null; }
        }

        /// <summary>
        /// Returns GL.GetShaderInfoLog(ShaderID), which contains any linking errors.
        /// </summary>
        public string ProgramLog
        {
            get { return GL.GetProgramInfoLog(this.ProgramID); }
        }
        #endregion

        #region Constructors and Destructor
        /// <summary>
        /// Links a vertex and fragment shader together to create a shader program.
        /// </summary>
        /// <param name="vertexShader">Specifies the vertex shader.</param>
        /// <param name="fragmentShader">Specifies the fragment shader.</param>
        public ShaderProgram(Shader vertexShader, Shader fragmentShader)
        {
            this.VertexShader = vertexShader;
            this.FragmentShader = fragmentShader;
            this.ProgramID = GL.CreateProgram();
            this.DisposeChildren = false;

            GL.AttachShader(this.ProgramID, vertexShader.ShaderID);
            GL.AttachShader(this.ProgramID, fragmentShader.ShaderID);
            GL.LinkProgram(this.ProgramID);

            this.GetParams();
        }

        /// <summary>
        /// Creates two shaders and then links them together to create a shader program.
        /// </summary>
        /// <param name="vertexShaderSource">Specifies the source code of the vertex shader.</param>
        /// <param name="fragmentShaderSource">Specifies the source code of the fragment shader.</param>
        public ShaderProgram(string vertexShaderSource, string fragmentShaderSource)
            : this(new Shader(vertexShaderSource, ShaderType.VertexShader), new Shader(fragmentShaderSource, ShaderType.FragmentShader))
        {
            this.DisposeChildren = true;
        }

        ~ShaderProgram()
        {
            if (this.ProgramID != 0) System.Diagnostics.Debug.Fail("ShaderProgram was not disposed of properly.");
        }
        #endregion

        #region GetParams
        /// <summary>
        /// Parses all of the parameters (attributes/uniforms) from the two attached shaders
        /// and then loads their location by passing this shader program into the parameter object.
        /// </summary>
        private void GetParams()
        {
            this.shaderParams = new Dictionary<string, ProgramParam>();
            foreach (ProgramParam pParam in this.VertexShader.ShaderParams)
            {
                if (!this.shaderParams.ContainsKey(pParam.Name))
                {
                    this.shaderParams.Add(pParam.Name, pParam);
                    pParam.GetLocation(this);
                }
            }
            foreach (ProgramParam pParam in this.FragmentShader.ShaderParams)
            {
                if (!this.shaderParams.ContainsKey(pParam.Name))
                {
                    this.shaderParams.Add(pParam.Name, pParam);
                    pParam.GetLocation(this);
                }
            }
        }
        #endregion

        #region Methods
        public void Use()
        {
            //if (GL.CurrentProgram != ProgramID) 
            GL.UseProgram(this.ProgramID);
        }

        public int GetUniformLocation(string Name)
        {
            this.Use();
            return GL.GetUniformLocation(this.ProgramID, Name);
        }

        public int GetAttributeLocation(string Name)
        {
            this.Use();
            return GL.GetAttribLocation(this.ProgramID, Name);
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            if (this.ProgramID != 0)
            {
                // Make sure this program isn't being used
                //if (GL.CurrentProgram == ProgramID) 
                GL.UseProgram(0);

                GL.DetachShader(this.ProgramID, this.VertexShader.ShaderID);
                GL.DetachShader(this.ProgramID, this.FragmentShader.ShaderID);
                GL.DeleteProgram(this.ProgramID);

                if (this.DisposeChildren)
                {
                    this.VertexShader.Dispose();
                    this.FragmentShader.Dispose();
                }

                this.ProgramID = 0;
            }
        }
        #endregion
    }
}
