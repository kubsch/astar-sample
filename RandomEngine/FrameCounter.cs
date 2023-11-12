namespace RandomEngine.Debug;

public class FrameCounter
{
    public const int MaximumSamples = 100;

    private Queue<double> _sampleBuffer = new();
    public long TotalFrames { get; private set; }
    public double TotalSeconds { get; private set; }
    public double AverageFramesPerSecond { get; private set; }
    public double CurrentFramesPerSecond { get; private set; }

    public void Update(GameTime gameTime)
    {
        CurrentFramesPerSecond = 1.0d / gameTime.ElapsedGameTime.TotalSeconds;

        _sampleBuffer.Enqueue(CurrentFramesPerSecond);

        if (_sampleBuffer.Count > MaximumSamples)
        {
            _sampleBuffer.Dequeue();
            AverageFramesPerSecond = _sampleBuffer.Average(i => i);
        }
        else
            AverageFramesPerSecond = CurrentFramesPerSecond;

        TotalFrames++;
        TotalSeconds += gameTime.ElapsedGameTime.TotalSeconds;
    }
}