namespace Domain.Models;

public class ShapedEntity
{
    public ShapedEntity()
    {
        Entity = new Entity();
    }
    public Entity Entity { get; set; }
    public Guid Id { get; set; }
}