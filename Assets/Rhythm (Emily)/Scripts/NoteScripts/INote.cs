public interface INote
{
    void SetLane(NoteLane lane);
    void SetHitTime(double dspTime);
    bool TryResolve();

    void SetSpeed(float s);
}