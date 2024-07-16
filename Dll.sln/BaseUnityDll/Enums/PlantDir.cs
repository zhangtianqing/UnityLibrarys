namespace BaseUnityDll.Enums
{
    public enum PlantDir
    {
        Left = 1 << 0,
        Front = 1 << 1 | 1 << 2,
        Front_Left = 1 << 1,
        Front_Right = 1 << 2,
        Right = 1 << 3,
        Back = 1 << 4,
        Ground = 1 << 5 | 1 << 6,
        Ground_Left = 1 << 5,
        Ground_Right = 1 << 6,
        Top = 1 << 7,
    }
}