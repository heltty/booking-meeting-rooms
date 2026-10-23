public class Room
{
    public required string Name { get; set; }
    public int Id { get; set; }
    public int Capacity { get; set; }
    public required string Floor { get; set; }
    public List<EquipmentItems> Equipment { get; set; } = new();
    public bool IsActive { get; set; }
}