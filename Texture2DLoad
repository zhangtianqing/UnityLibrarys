public class Texture2DLoad{
 /// <summary>
    /// 将Texture2d转换为Sprite
    /// </summary>
    /// <param name="tex">参数是texture2d纹理</param>
    /// <returns></returns>
    private Sprite TextureToSprite(Texture2D tex)
    {
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        return sprite;
    }
    private Texture2D LoadTexture2DByFilePath(string filePath)
    {
        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        fs.Seek(0, SeekOrigin.Begin);//游标的操作，可有可无
        byte[] bytes = new byte[fs.Length];//生命字节，用来存储读取到的图片字节
        try
        {
            fs.Read(bytes, 0, bytes.Length);//开始读取，这里最好用trycatch语句，防止读取失败报错

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        fs.Close();//切记关闭

        int width = 2048;//图片的宽（这里两个参数可以提到方法参数中）
        int height = 2048;//图片的高（这里说个题外话，pico相关的开发，这里不能大于4k×4k不然会显示异常，当时开发pico的时候应为这个问题找了大半天原因，因为美术给的图是6000*3600，导致出现切几张图后就黑屏了。。。
        Texture2D texture = new Texture2D(width, height);
        if (texture.LoadImage(bytes))
        {
            print("图片加载完毕 ");
            return texture;//将生成的texture2d返回，到这里就得到了外部的图片，可以使用了

        }
        else
        {
            print("图片尚未加载");
            return null;
        }
    }
}
