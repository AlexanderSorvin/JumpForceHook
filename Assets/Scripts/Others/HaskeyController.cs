using UnityEngine;

public class HaskeyIntController
{

    public HaskeyIntController(string hasKey, int defaultValue = 0)
    {
        HasKey = hasKey;

        if (!PlayerPrefs.HasKey(HasKey)) Set(defaultValue);
        else parameter = PlayerPrefs.GetInt(HasKey);
    }

    private readonly string HasKey;
    private int parameter;

    public void Set(int value)
    {
        PlayerPrefs.SetInt(HasKey, value);
        parameter = value;
    }

    public static implicit operator int(HaskeyIntController p)
    {
        return p.parameter;
    }
}

public class HaskeyFloatController
{
    public HaskeyFloatController(string hasKey, float defaultValue = 0.0f)
    {
        HasKey = hasKey;

        if (!PlayerPrefs.HasKey(HasKey)) Set(defaultValue);
        else parameter = PlayerPrefs.GetFloat(HasKey);
    }

    private readonly string HasKey;
    private float parameter;

    public void Set(float value)
    {
        PlayerPrefs.SetFloat(HasKey, value);
        parameter = value;
    }

    public static implicit operator float(HaskeyFloatController p)
    {
        return p.parameter;
    }
}


public class HaskeyStringController
{
    public HaskeyStringController(string hasKey, string defaultValue = "")
    {
        HasKey = hasKey;

        if (!PlayerPrefs.HasKey(HasKey)) Set(defaultValue);
        else parameter = PlayerPrefs.GetString(HasKey);
    }

    private readonly string HasKey;
    private string parameter;

    public void Set(string value)
    {
        PlayerPrefs.SetString(HasKey, value);
        parameter = value;
    }

    public static implicit operator string(HaskeyStringController p)
    {
        return p.parameter;
    }
}