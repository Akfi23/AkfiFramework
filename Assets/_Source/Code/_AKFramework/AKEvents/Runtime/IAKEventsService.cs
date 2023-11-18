namespace _Source.Code._AKFramework.AKEvents.Runtime
{
    public interface IAKEventsService : IAKService
    {
        void AddListener(AKEvent akEvent, AKCallback handler);
        void RemoveListener(AKEvent akEvent, AKCallback handler);

        void AddListener<T1>(AKEvent akEvent, AKCallback<T1> handler);
        void RemoveListener<T1>(AKEvent akEvent, AKCallback<T1> handler);

        void AddListener<T1, T2>(AKEvent akEvent,  AKCallback<T1, T2> handler);
        void RemoveListener<T1, T2>(AKEvent akEvent,  AKCallback<T1, T2> handler);

        void AddListener<T1, T2, T3>(AKEvent akEvent,  AKCallback<T1, T2, T3> handler);
        void RemoveListener<T1, T2, T3>(AKEvent akEvent,  AKCallback<T1, T2, T3> handler);

        void AddListener<T1, T2, T3, T4>(AKEvent akEvent,  AKCallback<T1, T2, T3, T4> handler);
        void RemoveListener<T1, T2, T3, T4>(AKEvent akEvent, AKCallback<T1, T2, T3, T4> handler);

        void Broadcast(AKEvent akEvent);
        void Broadcast<T1>(AKEvent akEvent, T1 arg1);
        void Broadcast<T1, T2>(AKEvent akEvent, T1 arg1, T2 arg2);
        void Broadcast<T1, T2, T3>(AKEvent akEvent, T1 arg1, T2 arg2, T3 arg3);
        void Broadcast<T1, T2, T3, T4>(AKEvent akEvent, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    }
}