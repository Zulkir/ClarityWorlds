using System;
using Clarity.Engine.Platforms;

namespace Clarity.App.Transport.Prototype.DataSources
{
    public interface IDataSource : IDisposable
    {
        void AttachReceiver(IDataSourceReceiver receiver);
        void Open();
        void OnNewFrame(FrameTime frameTime);
    }
}