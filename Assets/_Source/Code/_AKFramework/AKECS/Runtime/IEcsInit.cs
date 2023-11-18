namespace _Source.Code._AKFramework.AKECS.Runtime
{
    public interface IEcsInit<T> where T: struct
    {
        public void Init(ref T c);
    }
}