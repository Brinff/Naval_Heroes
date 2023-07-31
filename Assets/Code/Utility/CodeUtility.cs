
public static class CodeUtility
{
    public static T As<T>(this object obj)
    {
        if (obj is T) return (T)obj;
        return default(T);
    }
}

