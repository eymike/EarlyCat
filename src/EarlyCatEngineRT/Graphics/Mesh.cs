using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace EarlyCatEngineRT.Graphics
{
    interface IMesh
    {
        void Render(DeviceContext context);
    }

    public class Mesh<VertexType> : IMesh, IDisposable where VertexType : struct
    {
        private readonly PrimitiveTopology m_topology;
        private readonly SharpDX.Direct3D11.Buffer m_vertexBuffer;
        private readonly SharpDX.Direct3D11.Buffer m_indexBuffer;
        private readonly VertexBufferBinding m_vertexBufferBinding;

        private readonly int m_vertexCount;
        public int VertexCount
        {
            get { return m_vertexCount; }
        }

        private readonly int m_indexCount;
        public int IndexCount
        {
            get { return m_indexCount; }
        }

        public Mesh(GraphicsDevice device, VertexType[] vertexes, int[] indexes, int elementSize, PrimitiveTopology topology)
        {
            m_topology = topology;
            m_vertexCount = vertexes.Length;
            m_indexCount = indexes.Length;

            using (var stream = DataStream.Create<VertexType>(vertexes, true, true))
            {
                var vertexDesc = new BufferDescription
                {
                    BindFlags = BindFlags.VertexBuffer,
                    CpuAccessFlags = CpuAccessFlags.None,
                    OptionFlags = ResourceOptionFlags.None,
                    SizeInBytes = vertexes.Length * elementSize,
                    StructureByteStride = elementSize,
                    Usage = ResourceUsage.Default
                };
                m_vertexBuffer = new SharpDX.Direct3D11.Buffer(device.Device, stream, vertexDesc);
                m_vertexBufferBinding = new VertexBufferBinding(m_vertexBuffer, elementSize, 0);
            }

            using (var stream = DataStream.Create<Int32>(indexes, true, true))
            {
                var indexDesc = new BufferDescription
                {
                    BindFlags = BindFlags.IndexBuffer,
                    CpuAccessFlags = CpuAccessFlags.None,
                    OptionFlags = ResourceOptionFlags.None,
                    SizeInBytes = sizeof(int) * indexes.Length,
                    StructureByteStride = sizeof(int),
                    Usage = ResourceUsage.Default
                };
                m_indexBuffer = new SharpDX.Direct3D11.Buffer(device.Device, stream, indexDesc);
            }
        }

        public void Render(DeviceContext context)
        {
            context.InputAssembler.PrimitiveTopology = m_topology;
            context.InputAssembler.SetVertexBuffers(0, m_vertexBufferBinding);
            context.InputAssembler.SetIndexBuffer(m_indexBuffer, SharpDX.DXGI.Format.R32_UInt, 0);
            context.DrawIndexed(m_indexCount, 0, 0);
        }

        public void Dispose()
        {
            m_vertexBuffer.Dispose();
            m_indexBuffer.Dispose();
        }
    }
}
