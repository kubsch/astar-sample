namespace RandomEngine;

public interface IGameState
{
    public string Name { get; }

    void Update(GameTime gameTime);

    void Draw(GameTime gameTime);

    void WakeUp(GameTime gameTime, IGameState? oldGameState);

    void Initialize();
}