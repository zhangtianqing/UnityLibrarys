
namespace Dll.Framework.Config.ConfigLoader
{
    /// <summary>
    /// 配置加载器接口
    /// </summary>
    public abstract class IConfigLoader
    {
        /// <summary>
        /// 加载接口
        /// </summary>
        /// <typeparam name="A">需要读取的对象类型</typeparam>
        /// <returns></returns>
        public abstract A Load<A>() where A : class, new();
        /// <summary>
        /// 储存接口
        /// </summary>
        /// <param name="t">需要保存的对象类型</param>
        /// <returns></returns>
        public abstract bool Save(object t);
        public string GetDefaultFileName()
        {
            return ("config." + this.GetType().Name.Split('_')[1]).ToLower();
        }

    }
}