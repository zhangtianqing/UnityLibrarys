using System;
public class EnumAnno : Attribute
{
    private string desc;

    public EnumAnno(string desc)
    {
        this.desc = desc;
    }
    public string GetDesc() => desc;
}
