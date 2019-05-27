namespace Clarity.Ext.Rendering.Ogl3.Helpers
{
    public interface IOffScreenContainer
    {
        IOffScreen Get(object service, object surface, int width, int height, int samples, float ttl);
    }
}