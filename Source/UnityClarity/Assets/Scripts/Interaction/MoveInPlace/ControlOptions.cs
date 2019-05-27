namespace Assets.Scripts.Interaction.MoveInPlace
{
    public enum ControlOptions
    {
        /// <summary>
        /// Track both headset and controllers for movement calculations.
        /// </summary>
        HeadsetAndControllers,
        /// <summary>
        /// Track only the controllers for movement calculations.
        /// </summary>
        ControllersOnly,
        /// <summary>
        /// Track only headset for movement caluclations.
        /// </summary>
        HeadsetOnly,
    }
}