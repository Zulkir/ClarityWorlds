namespace Assets.Scripts.Gui
{
    public interface IVrHeadPositionService
    {
        // todo: create a common enum VerticalDirection (Up, Down)
        void TryUseElevator(bool up);
        void RotateRight(float degrees);
        void ResetHeadPosition();
    }
}