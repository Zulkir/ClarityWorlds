using Clarity.App.Transport.Prototype.Databases;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Engine.Platforms;

namespace Clarity.App.Transport.Prototype.DataSources
{
    public class EmptyDataSource : IDataSource
    {
        private IDataSourceReceiver receiver;

        public void AttachReceiver(IDataSourceReceiver receiver)
        {
            this.receiver = receiver;
            receiver.Reset(EmptyArrays<IDataTable>.Array);
        }

        public void Open()
        {
        }

        public void OnNewFrame(FrameTime frameTime)
        {
        }

        public void Dispose()
        {
            receiver = null;
        }
    }
}