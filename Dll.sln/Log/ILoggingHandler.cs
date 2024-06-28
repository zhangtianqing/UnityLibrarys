

namespace Log
{
    public interface ILoggingStorage
    {
         void WriteLog(string log);
        void Dispose();
    }
}
