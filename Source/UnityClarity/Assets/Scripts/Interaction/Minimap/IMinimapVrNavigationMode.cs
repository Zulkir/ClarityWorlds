namespace Assets.Scripts.Interaction.Minimap
{
    public interface IMinimapVrNavigationMode : IVrNavigationMode
    {
        bool ZoomEnabled { get; set; }
    }
}