namespace Monitoring.Core.Extentions
{
    public static class Convertors
    {
        public static T ConvertTo<T>(this object obj)
        {
            return (T)Convert.ChangeType(obj, typeof(T));
        }

        public static T ConvertTo<T>(this object obj, T defaultValue)
        {
            try
            {
                return (T)Convert.ChangeType(obj, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
