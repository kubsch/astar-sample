namespace RandomEngine.Entities;

public class LinearMovementEntityBehaviour : IEntityBehaviour
{
    private readonly Entity _entity;
    private readonly float _rotationAngle;
    private readonly float _speed;
    private Vector2 _direction;

    public LinearMovementEntityBehaviour(Entity entity, float rotationAngle, float speed)
    {
        _entity = entity;
        _rotationAngle = rotationAngle;
        var angle = MathHelper.ToRadians(rotationAngle);
        _direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        _direction.Normalize();
        _speed = speed;
    }

    public void Update(GameTime gameTime)
    {
        var newPos = new Vector2((float)Math.Cos(_rotationAngle) * _speed, (float)Math.Sin(_rotationAngle) * _speed);
        _entity.Position -= newPos;
    }
}