public class Attraction
{
    private int _id;
    private string _name;
    private double _minHeight;
    private int _areaId;
    private string _description;
    private string _openingTime;
    private string _closingTime;
    private int _capacity;
    private int _queueSpeed;
    private int _queueLength;

    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public double MinHeight
    {
        get { return _minHeight; }
        set { _minHeight = value; }
    }

    public int AreaId
    {
        get { return _areaId; }
        set { _areaId = value; }
    }

    public string Description
    {
        get { return _description; }
        set { _description = value; }
    }

    public string OpeningTime
    {
        get { return _openingTime; }
        set { _openingTime = value; }
    }

    public string ClosingTime
    {
        get { return _closingTime; }
        set { _closingTime = value; }
    }

    public int Capacity
    {
        get { return _capacity; }
        set { _capacity = value; }
    }

    public int QueueSpeed
    {
        get { return _queueSpeed; }
        set { _queueSpeed = value; }
    }

    public int QueueLength
    {
        get { return _queueLength; }
        set { _queueLength = value; }
    }
}
