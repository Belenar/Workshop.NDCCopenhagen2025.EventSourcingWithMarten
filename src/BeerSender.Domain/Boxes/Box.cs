namespace BeerSender.Domain.Boxes;

public class Box
{
    public Guid Id { get; set; }
    public BoxCapacity? BoxType { get; set; }
}