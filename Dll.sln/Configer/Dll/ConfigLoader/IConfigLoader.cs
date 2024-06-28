
namespace Dll.Framework.Config.ConfigLoader
{
    /// <summary>
    /// ���ü������ӿ�
    /// </summary>
    public abstract class IConfigLoader
    {
        /// <summary>
        /// ���ؽӿ�
        /// </summary>
        /// <typeparam name="A">��Ҫ��ȡ�Ķ�������</typeparam>
        /// <returns></returns>
        public abstract A Load<A>() where A : class, new();
        /// <summary>
        /// ����ӿ�
        /// </summary>
        /// <param name="t">��Ҫ����Ķ�������</param>
        /// <returns></returns>
        public abstract bool Save(object t);
        public string GetDefaultFileName()
        {
            return ("config." + this.GetType().Name.Split('_')[1]).ToLower();
        }

    }
}