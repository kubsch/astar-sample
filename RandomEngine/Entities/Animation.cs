namespace RandomEngine.Entities;

public class Animation
{
    private int _timeSinceLastFrame = 0;

    public Animation(string name, Texture2D texture, int timePerFrame, int frameWidth)
    {
        Texture = texture;
        TimePerFrame = timePerFrame;
        FrameWidth = frameWidth;
        FrameHeight = texture.Height;
        Name = name;
        FrameCount = texture.Width / frameWidth;

        UpdateFrame(0);
    }

    public string Name { get; }
    public Texture2D Texture { get; }
    public int CurrentFrame { get; private set; }
    public int FrameCount { get; }
    public int TimePerFrame { get; }
    public Rectangle SourceRectangle { get; private set; }

    public int FrameWidth { get; }
    public int FrameHeight { get; }

    public void Update(GameTime gameTime)
    {
        _timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
        var frameChange = _timeSinceLastFrame / TimePerFrame;

        if (frameChange > 0)
        {
            _timeSinceLastFrame = 0;
            var nextFrame = CurrentFrame + frameChange;
            nextFrame %= FrameCount;

            UpdateFrame(nextFrame);
        }
    }

    public void Reset()
    {
        UpdateFrame(0);
    }

    private void UpdateFrame(int nextFrame)
    {
        CurrentFrame = nextFrame;
        var frameIndex = CurrentFrame;

        SourceRectangle = new Rectangle(FrameWidth * frameIndex, 0, FrameWidth, FrameHeight);
    }
}