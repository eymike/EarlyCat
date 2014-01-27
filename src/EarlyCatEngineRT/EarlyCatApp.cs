using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.DXGI;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace EarlyCatEngineRT
{
    public class EarlyCatApp
    {
        private readonly Graphics.GraphicsDevice m_graphicsDevice;

       // private readonly th
       // private readonly ManualResetEvent m_quitEvent;

        public EarlyCatApp(CoreWindow coreWindow, SwapChainBackgroundPanel panel)
        {
            m_graphicsDevice = new Graphics.GraphicsDevice(coreWindow);
            using (ISwapChainBackgroundPanelNative cPanel = ComObject.As<ISwapChainBackgroundPanelNative>(panel))
            {
                cPanel.SwapChain = m_graphicsDevice.SwapChain;
            }

            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        void CompositionTarget_Rendering(object sender, object e)
        {
            Render();
        }

        

        public void Render()
        {
            m_graphicsDevice.Clear(Color.Azure);

            m_graphicsDevice.Present();
        }
    }
}
