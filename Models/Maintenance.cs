using System;

public class Maintenance
{
    private int _id;
    private int _attractionId;
    private string _description;
    private DateTime? _date;
    private string _status;
    private string _attractionName;

    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }

    public int AttractionId
    {
        get { return _attractionId; }
        set { _attractionId = value; }
    }

    public string Description
    {
        get { return _description; }
        set { _description = value; }
    }

    public DateTime? Date
    {
        get { return _date; }
        set { _date = value; }
    }

    public string Status
    {
        get { return _status; }
        set { _status = value; }
    }

    public string AttractionName
    {
        get { return _attractionName; }
        set { _attractionName = value; }
    }
}
