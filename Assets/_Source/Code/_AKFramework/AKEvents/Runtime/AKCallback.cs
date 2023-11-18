namespace _Source.Code._AKFramework.AKEvents.Runtime
{
    public delegate void AKCallback(AKEvent akEvent);

    public delegate void AKCallback<in T1>(AKEvent akEvent, T1 arg1);

    public delegate void AKCallback<in T1, in T2>(AKEvent akEvent, T1 arg1, T2 arg2);

    public delegate void AKCallback<in T1, in T2, in T3>(AKEvent akEvent, T1 arg1, T2 arg2, T3 arg3);

    public delegate void AKCallback<in T1, in T2, in T3, in T4>(AKEvent akEvent, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
}