using _Source.Code._AKFramework.AKTags.Runtime;

namespace _Source.Code.Interfaces
{
    public interface IParameterAction
    {
        AKTag ParameterTag { get; }
        float Get(int level);
        int GetMaxLevel();
    }
}