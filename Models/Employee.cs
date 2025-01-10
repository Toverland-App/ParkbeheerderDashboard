using System;

public class Employee
{
    private int _id;
    private string _name;
    private string _role;
    private int _areaId;
    private string _email;
    private string _phoneNumber;
    private DateTime _hireDate;
    private bool _isActive;

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

    public string Role
    {
        get { return _role; }
        set { _role = value; }
    }

    public int AreaId
    {
        get { return _areaId; }
        set { _areaId = value; }
    }

    public string Email
    {
        get { return _email; }
        set { _email = value; }
    }

    public string PhoneNumber
    {
        get { return _phoneNumber; }
        set { _phoneNumber = value; }
    }

    public DateTime HireDate
    {
        get { return _hireDate; }
        set { _hireDate = value; }
    }

    public bool IsActive
    {
        get { return _isActive; }
        set { _isActive = value; }
    }
}
