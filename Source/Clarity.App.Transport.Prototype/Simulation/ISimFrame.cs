namespace Clarity.App.Transport.Prototype.Simulation
{
    public interface ISimFrame
    {
        double Timestamp { get; }
        int? IncedentalEntryIndex { get; }
        void Apply(ISimState state);
        void Undo(ISimState state);
    }
}