namespace RandomEngine;

public abstract class Transition : IGameState
{
    public Transition(Engine engine, IGameState gameStateOld, IGameState gameStateNew)
    {
        Engine = engine;
        GameStateOld = gameStateOld;
        GameStateNew = gameStateNew;
    }

    public bool Finished { get; protected set; } = false;
    public abstract string Name { get; }
    protected Engine Engine { get; }
    protected IGameState GameStateOld { get; }
    protected IGameState GameStateNew { get; }

    public abstract void Update(GameTime gameTime);

    public abstract void Draw(GameTime gameTime);

    public void WakeUp(GameTime gameTime, IGameState gameStateOld)
    {
    }

    public void Initialize()
    {
    }

    protected virtual void Finish(GameTime gameTime)
    {
        Finished = true;
        Engine.ToGameState(gameTime, Engine.GetGameState(GameStateNew.Name));
    }
}