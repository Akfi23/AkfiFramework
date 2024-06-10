using _Source.Code._AKFramework.AKTags.Runtime;

namespace _Source.Code.Interfaces
{
    public interface IVibrationService : IAKService
    {
        void PlayVibro(AKTag vibroTag, float delay);
    }
}