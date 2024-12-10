public class Attraction
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double MinHeight { get; set; }
    public int AreaId { get; set; }
    public string Description { get; set; }
    public string OpeningTime { get; set; }
    public string ClosingTime { get; set; }
    public int Capacity { get; set; }
    public int QueueSpeed { get; set; }
    public int QueueLength { get; set; }
}
