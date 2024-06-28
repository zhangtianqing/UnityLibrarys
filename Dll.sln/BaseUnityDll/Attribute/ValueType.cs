using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ValueType
{
    InputFiled,
    Toggle,
    Enum
}


public class ValueTypeAttr : Attribute
{
    private ValueType valueType = ValueType.InputFiled;
    public ValueTypeAttr(ValueType valueType)
    {
        this.valueType = valueType;
    }
    public ValueTypeAttr() { }
    public ValueType GetValueType()
    {
        return valueType;
    }
}