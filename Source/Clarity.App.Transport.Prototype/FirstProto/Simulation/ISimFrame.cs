namespace Clarity.App.Transport.Prototype.FirstProto.Simulation
{
    public interface ISimFrame
    {
        double Timestamp { get; }
        int? IncedentalEntryIndex { get; }
        void Apply(ISimState state);
        void Undo(ISimState state);
    }
}