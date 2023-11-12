namespace RandomEngine.Entities;

public class PlayerControlledEntityBehaviour : IEntityBehaviour
{
    private readonly Entity _entity;
    private readonly Engine _engine;

    public PlayerControlledEntityBehaviour(Entity entity, Engine engine)
    {
        _entity = entity;
        _engine = engine;
    }

    public void Update(GameTime gameTime)
    {
        var playerSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 4;

        var up = false;
        var right = false;
        var down = false;
        var left = false;

        if (_engine.Keyboard.IsKeyDown(Keys.W))
        {
            _entity.Position -= new Vector2(0, playerSpeed);
            _entity.SetAnimation("run");

            up = true;
        }
        else if (_engine.Keyboard.IsKeyDown(Keys.S))
        {
            _entity.Position += new Vector2(0, playerSpeed);
            _entity.SetAnimation("run");

            down = true;
        }

        if (_engine.Keyboard.IsKeyDown(Keys.A))
        {
            _entity.Position -= new Vector2(playerSpeed, 0);
            _entity.SetAnimation("run");

            left = true;
        }
        else if (_engine.Keyboard.IsKeyDown(Keys.D))
        {
            _entity.Position += new Vector2(playerSpeed, 0);
            _entity.SetAnimation("run");

            right = true;
        }

        if (up && down)
            up = down = false;

        if (left && right)
            left = right = false;

        if (up)
        {
            _entity.Directions &= ~Directions.Down;
            _entity.Directions |= Directions.Up;
        }

        if (down)
        {
            _entity.Directions &= ~Directions.Up;
            _entity.Directions |= Directions.Down;
        }

        if (left)
        {
            _entity.Directions &= ~Directions.Right;
            _entity.Directions |= Directions.Left;
        }

        if (right)
        {
            _entity.Directions &= ~Directions.Left;
            _entity.Directions |= Directions.Right;
        }

        if (!_engine.Keyboard.PressedKeys.Any())
            _entity.SetAnimation("idle");
    }
}