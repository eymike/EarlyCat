using System;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Direct3D11;

namespace EarlyCatEngineRT.Graphics.VertexFormats
{

    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct VertexPosition
    {
        public static readonly int SizeInBytes = Marshal.SizeOf<VertexPositionColor>();
        public static readonly InputElement[] InputElements = new[]
        {
            new InputElement("POSITION", 0, SharpDX.DXGI.Format.R32G32B32_Float, InputElement.AppendAligned, 0),
        };

        Vector3 Position;

        public VertexPosition(Vector3 position)
        {
            Position = position;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VertexPositionColor
    {
        public static readonly int SizeInBytes = Marshal.SizeOf<VertexPositionColor>();
        public static readonly InputElement[] InputElements = new[]
        {
            new InputElement("POSITION", 0, SharpDX.DXGI.Format.R32G32B32_Float, InputElement.AppendAligned, 0),
            new InputElement("COLOR",    0, SharpDX.DXGI.Format.R32G32B32A32_Float, InputElement.AppendAligned, 0),
        };

        Vector3 Position;
        Color4 Color;

        public VertexPositionColor(Vector3 position, Color4 color)
        {
            Position = position;
            Color = color;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VertexPositionColorTexCoord
    {
        public static readonly int SizeInBytes = Marshal.SizeOf<VertexPositionColor>();
        public static readonly InputElement[] InputElements = new[]
        {
            new InputElement("POSITION", 0, SharpDX.DXGI.Format.R32G32B32_Float, InputElement.AppendAligned, 0),
            new InputElement("COLOR",    0, SharpDX.DXGI.Format.R32G32B32A32_Float, InputElement.AppendAligned, 0),
            new InputElement("TEXCOORD", 0, SharpDX.DXGI.Format.R32G32_Float, InputElement.AppendAligned, 0),
        };

        Vector3 Position;
        Color4 Color;
        Vector2 TexCoord;

        public VertexPositionColorTexCoord(Vector3 position, Color4 color, Vector2 texCoord)
        {
            Position = position;
            Color = color;
            TexCoord = texCoord;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VertexPositionNormalTexCoord
    {
        public static readonly int SizeInBytes = Marshal.SizeOf<VertexPositionColor>();
        public static readonly InputElement[] InputElements = new[]
        {
            new InputElement("POSITION", 0, SharpDX.DXGI.Format.R32G32B32_Float, InputElement.AppendAligned, 0),
            new InputElement("NORMAL",   0, SharpDX.DXGI.Format.R32G32B32_Float, InputElement.AppendAligned, 0),
            new InputElement("TEXCOORD", 0, SharpDX.DXGI.Format.R32G32_Float, InputElement.AppendAligned, 0),
        };

        Vector3 Position;
        Vector3 Normal;
        Vector2 TexCoord;

        public VertexPositionNormalTexCoord(Vector3 position, Vector3 normal, Vector2 texCoord)
        {
            Position = position;
            Normal = normal;
            TexCoord = texCoord;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VertexPositionColorNormalTexCoord
    {
        public static readonly int SizeInBytes = Marshal.SizeOf<VertexPositionColor>();
        public static readonly InputElement[] InputElements = new[]
        {
            new InputElement("POSITION", 0, SharpDX.DXGI.Format.R32G32B32_Float, InputElement.AppendAligned, 0),
            new InputElement("COLOR",    0, SharpDX.DXGI.Format.R32G32B32A32_Float, InputElement.AppendAligned, 0),
            new InputElement("NORMAL",   0, SharpDX.DXGI.Format.R32G32B32_Float, InputElement.AppendAligned, 0),
            new InputElement("TEXCOORD", 0, SharpDX.DXGI.Format.R32G32_Float, InputElement.AppendAligned, 0),
        };

        Vector3 Position;
        Color4 Color;
        Vector3 Normal;
        Vector2 TexCoord;

        public VertexPositionColorNormalTexCoord(Vector3 position,  Color4 color, Vector3 normal, Vector2 texCoord)
        {
            Position = position;
            Color = color;
            Normal = normal;
            TexCoord = texCoord;
        }
    }
}