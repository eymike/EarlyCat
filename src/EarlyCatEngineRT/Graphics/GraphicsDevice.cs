using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using Windows.UI.Core;

namespace EarlyCatEngineRT.Graphics
{
    public class GraphicsDevice : IDisposable
    {
        private readonly CoreWindow m_window;
        private readonly Device m_device;
        private readonly DeviceContext m_immContext;
        private SharpDX.DXGI.SwapChain1 m_swapChain;
        private RenderTargetView m_renderTarget;

        private Texture2D m_depthBuffer;
        private DepthStencilView m_depthStencil;

        public Device Device
        {
            get { return m_device; }
        }

        public GraphicsDevice(CoreWindow coreWindow)
        {
            m_window = coreWindow;
            DeviceCreationFlags flags = DeviceCreationFlags.BgraSupport;
#if DEBUG
            flags |= DeviceCreationFlags.Debug;
#endif

            FeatureLevel[] featureLevels = {
                                                FeatureLevel.Level_11_0,
                                                FeatureLevel.Level_10_1,
                                                FeatureLevel.Level_10_0
                                            };

            m_device = new Device(SharpDX.Direct3D.DriverType.Hardware, flags, featureLevels);
            m_immContext = m_device.ImmediateContext;

            coreWindow.SizeChanged += coreWindow_SizeChanged;

            Reset((int)coreWindow.Bounds.Width, (int)coreWindow.Bounds.Height);
        }

        void coreWindow_SizeChanged(CoreWindow sender, WindowSizeChangedEventArgs args)
        {
            Reset((int)sender.Bounds.Width, (int)sender.Bounds.Height);
        }

        public void Reset(int width, int height)
        {
            if (m_renderTarget != null) { m_renderTarget.Dispose(); }
            if (m_depthBuffer != null) { m_depthBuffer.Dispose(); }
            if (m_depthStencil != null) { m_depthStencil.Dispose(); }

            if (m_swapChain != null)
            {
                m_swapChain.ResizeBuffers(2, width, height, SharpDX.DXGI.Format.B8G8R8A8_UNorm, SharpDX.DXGI.SwapChainFlags.None);
            }
            else
            {
                // SwapChain description
                var desc = new SharpDX.DXGI.SwapChainDescription1
                {
                    // Automatic sizing
                    Width = width,
                    Height = height,
                    Format = SharpDX.DXGI.Format.B8G8R8A8_UNorm,
                    Stereo = false,
                    SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                    Usage = SharpDX.DXGI.Usage.BackBuffer | SharpDX.DXGI.Usage.RenderTargetOutput,
                    // Use two buffers to enable flip effect.
                    BufferCount = 2,
                    Scaling = SharpDX.DXGI.Scaling.Stretch,
                    SwapEffect = SharpDX.DXGI.SwapEffect.FlipSequential,
                };

                using (var dxgiDevice2 = m_device.QueryInterface<SharpDX.DXGI.Device2>())
                using (var dxgiAdapter = dxgiDevice2.Adapter)
                using (var dxgiFactory2 = dxgiAdapter.GetParent<SharpDX.DXGI.Factory2>())
                {
                   
                    m_swapChain = dxgiFactory2.CreateSwapChainForComposition(m_device, ref desc, null);
                    

                    dxgiDevice2.MaximumFrameLatency = 1;
                }
            }

            using(var backBuffer = Resource.FromSwapChain<Texture2D>(m_swapChain, 0))
            {
                // Create a view interface on the rendertarget to use on bind.
                m_renderTarget = new RenderTargetView(m_device, backBuffer);
            }

            Texture2DDescription depthStencilDesc = new Texture2DDescription
            {
                ArraySize = 1,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = SharpDX.DXGI.Format.D24_UNorm_S8_UInt,
                Height = height,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.Shared,
                SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                Width = width
            };

            m_depthBuffer = new Texture2D(m_device, depthStencilDesc);
            m_depthStencil = new DepthStencilView(m_device, m_depthBuffer);

            ResetBackBuffer();
        }

        public void ResetBackBuffer()
        {
            m_device.ImmediateContext.OutputMerger.SetRenderTargets(m_depthStencil, m_renderTarget);
            // Create a viewport descriptor of the full window size.
            var viewport = new Viewport(0, 0, (int)m_window.Bounds.Width, (int)m_window.Bounds.Height, 0.0f, 1.0f);

            // Set the current viewport using the descriptor.
            m_device.ImmediateContext.Rasterizer.SetViewport(viewport);
        }

        public void Clear(Color4 color)
        {
            m_device.ImmediateContext.ClearRenderTargetView(m_renderTarget, color);
            m_device.ImmediateContext.ClearDepthStencilView(m_depthStencil, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 0, 0);
        }

        public void Present()
        {
            m_swapChain.Present(1, SharpDX.DXGI.PresentFlags.None);
        }

        public void Dispose()
        {
            m_renderTarget.Dispose();
            m_swapChain.Dispose();
            m_device.Dispose();
        }

        public SharpDX.DXGI.SwapChain1 SwapChain
        {
            get { return m_swapChain; }
        }
    }
}
