namespace BaseUnityDll.BaseClass
{

    public abstract class BaseOdds<T>
    {

        public float start;
        public bool withStart = false;

        public float end;
        public bool withEnd = false;

        public bool toEnd = false;
        public T value;
        public void TimeDecode(string input)
        {
            withStart = input.StartsWith("[");
            withEnd = input.EndsWith("]");
            string str = input.Substring(0, input.Length - 1);
            str = str.Substring(1, str.Length - 1);
            string[] vs1 = str.Split('-');
            start = float.Parse(vs1[0]);
            if (vs1[1] == "End")
            {
                end = float.MaxValue;
            }
            else
            {
                end = float.Parse(vs1[1]);
            }
        }
        public bool CheckByTime(float time)
        {
            return start <= time && time < end;
        }
        protected abstract void ValueDecode(string[] input);
        protected void DecodeData(string value)
        {
            string[] statePara = value.Split(',');
            TimeDecode(statePara[0]);
            ValueDecode(statePara);

        }
    }
}
