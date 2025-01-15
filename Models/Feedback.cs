public class Feedback
{
    private int _id;
    private int _rating;
    private string _comment;

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

    public string Comment
    {
        get { return _comment; }
        set { _comment = value; }
    }
}
