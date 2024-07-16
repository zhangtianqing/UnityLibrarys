


using BaseUnityDll.Enums;

namespace BaseUnityDll.Attributes
{

    public class ValueTypeAttr : System.Attribute
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
}