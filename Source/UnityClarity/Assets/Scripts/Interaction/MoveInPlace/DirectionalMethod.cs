namespace Assets.Scripts.Interaction.MoveInPlace
{
    /// <summary>
    /// Options for which method is used to determine direction while moving.
    /// </summary>
    public enum DirectionalMethod
    {
        /// <summary>
        /// Will always move in the direction they are currently looking.
        /// </summary>
        Gaze,
        /// <summary>
        /// Will move in the direction that the controllers are pointing (averaged).
        /// </summary>
        ControllerRotation,
        /// <summary>
        /// Will move in the direction they were first looking when they engaged Move In Place.
        /// </summary>
        DumbDecoupling,
        /// <summary>
        /// Will move in the direction they are looking only if their headset point the same direction as their controllers.
        /// </summary>
        SmartDecoupling,
        /// <summary>
        /// Will move in the direction that the controller with the engage button pressed is pointing.
        /// </summary>
        EngageControllerRotationOnly,
        /// <summary>
        /// Will move in the direction that the left controller is pointing.
        /// </summary>
        LeftControllerRotationOnly,
        /// <summary>
        /// Will move in the direction that the right controller is pointing.
        /// </summary>
        RightControllerRotationOnly
    }
}