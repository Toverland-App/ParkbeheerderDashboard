public class Feedback
{
    private int _id;
    private int _rating;
    private string _description;

    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }

    public int Rating
    {
        get { return _rating; }
        set { _rating = value; }
    }

    public string Description
    {
        get { return _description; }
        set { _description = value; }
    }
}
