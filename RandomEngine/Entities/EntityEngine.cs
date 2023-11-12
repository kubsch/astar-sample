using System.Collections;

namespace RandomEngine.Entities;

public class EntityEngine : IEnumerable<Entity>
{
    private readonly Engine _engine;
    private Dictionary<ulong, Entity> _entities = new Dictionary<ulong, Entity>();

    public EntityEngine(Engine engine)
    {
        _entities = new Dictionary<ulong, Entity>();
        _engine = engine;
    }

    public Entity Create()
    {
        var newEntity = new Entity();
        _entities.Add(newEntity.Id, newEntity);

        return newEntity;
    }

    public void Destroy(Entity entity)
    {
        _entities.Remove(entity.Id);
    }

    public void Draw(Engine engine, GameTime gameTime)
    {
        foreach (var entity in _entities.Values.Where(x => x.CurrentAnimation != null))
        {
            engine.Render.Entity(entity);
        }
    }

    public void Update(GameTime gameTime)
    {
        foreach (var entity in _entities.Values)
            entity.Update(gameTime);
    }

    public IEnumerator<Entity> GetEnumerator() => _entities.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _entities.Values.GetEnumerator();
}