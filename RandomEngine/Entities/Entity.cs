namespace RandomEngine.Entities;

public class Entity
{
    private static ulong _lastId = 0;

    public Entity()
    {
        Id = ++_lastId;
    }

    public List<IEntityBehaviour> Behaviours { get; set; } = new();

    public ulong Id { get; }

    public Animation? CurrentAnimation { get; private set; }
    public Dictionary<string, Animation> Animations { get; } = new();
    public Color Color { get; set; } = Color.White;
    public Vector2 Position { get; set; }
    public float Rotation { get; set; }
    public float Scale { get; set; } = 1f;

    public Directions Directions { get; set; } = Directions.Right;

    public void SetAnimation(string name)
    {
        if (CurrentAnimation?.Name == name)
            return;

        CurrentAnimation = Animations[name];
        CurrentAnimation.Reset();
    }

    public void Update(GameTime gameTime)
    {
        foreach (var behaviour in Behaviours)
            behaviour.Update(gameTime);

        CurrentAnimation?.Update(gameTime);
    }
}