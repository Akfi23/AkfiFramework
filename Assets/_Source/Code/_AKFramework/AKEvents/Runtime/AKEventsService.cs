using System;
using System.Collections.Generic;

namespace _Source.Code._AKFramework.AKEvents.Runtime
{
    public class AKEventsService : IAKEventsService
    {
        private Dictionary<AKEvent, Delegate> eventTable = new Dictionary<AKEvent, Delegate>();

        #region Listener
        
        public void AddListener(AKEvent akEvent, AKCallback handler)
        {
            if (akEvent.IsNone) return;

            onListenerAdding(akEvent, handler);
            eventTable[akEvent] = (AKCallback) eventTable[akEvent] + handler;
        }

        public void RemoveListener(AKEvent akEvent, AKCallback handler)
        {
            if (akEvent.IsNone) return;
            if (!eventTable.ContainsKey(akEvent)) return;
            onListenerRemoving(akEvent, handler);
            eventTable[akEvent] = (AKCallback) eventTable[akEvent] - handler;
            onListenerRemoved(akEvent);
        }

        public void AddListener<T1>(AKEvent akEvent, AKCallback<T1> handler)
        {
            if (akEvent.IsNone) return;

            onListenerAdding(akEvent, handler);
            eventTable[akEvent] = (AKCallback<T1>) eventTable[akEvent] + handler;
        }

        public void RemoveListener<T1>(AKEvent akEvent, AKCallback<T1> handler)
        {
            if (akEvent.IsNone) return;
            if (!eventTable.ContainsKey(akEvent)) return;
            onListenerRemoving(akEvent, handler);
            eventTable[akEvent] = (AKCallback<T1>) eventTable[akEvent] - handler;
            onListenerRemoved(akEvent);
        }

        public void AddListener<T1, T2>(AKEvent akEvent, AKCallback<T1, T2> handler)
        {
            if (akEvent.IsNone) return;
            onListenerAdding(akEvent, handler);
            eventTable[akEvent] = (AKCallback<T1, T2>) eventTable[akEvent] + handler;
        }

        public void RemoveListener<T1, T2>(AKEvent akEvent, AKCallback<T1, T2> handler)
        {
            if (akEvent.IsNone) return;
            if (!eventTable.ContainsKey(akEvent)) return;
            onListenerRemoving(akEvent, handler);
            eventTable[akEvent] = (AKCallback<T1, T2>) eventTable[akEvent] - handler;
            onListenerRemoved(akEvent);
        }

        public void AddListener<T1, T2, T3>(AKEvent akEvent, AKCallback<T1, T2, T3> handler)
        {
            if (akEvent.IsNone) return;
            onListenerAdding(akEvent, handler);
            eventTable[akEvent] = (AKCallback<T1, T2, T3>) eventTable[akEvent] + handler;
        }

        public void RemoveListener<T1, T2, T3>(AKEvent akEvent, AKCallback<T1, T2, T3> handler)
        {
            if (akEvent.IsNone) return;
            if (!eventTable.ContainsKey(akEvent)) return;
            onListenerRemoving(akEvent, handler);
            eventTable[akEvent] = (AKCallback<T1, T2, T3>) eventTable[akEvent] - handler;
            onListenerRemoved(akEvent);
        }

        public void AddListener<T1, T2, T3, T4>(AKEvent akEvent, AKCallback<T1, T2, T3, T4> handler)
        {
            if (akEvent.IsNone) return;
            onListenerAdding(akEvent, handler);
            eventTable[akEvent] = (AKCallback<T1, T2, T3, T4>) eventTable[akEvent] + handler;
        }

        public void RemoveListener<T1, T2, T3, T4>(AKEvent akEvent, AKCallback<T1, T2, T3, T4> handler)
        {
            if (akEvent.IsNone) return;
            if (!eventTable.ContainsKey(akEvent)) return;
            onListenerRemoving(akEvent, handler);
            eventTable[akEvent] = (AKCallback<T1, T2, T3, T4>) eventTable[akEvent] - handler;
            onListenerRemoved(akEvent);
        }

        #endregion

        #region Command

        public void Broadcast(AKEvent akEvent)
        {
            if (akEvent.IsNone) return;
            
            if (!eventTable.TryGetValue(akEvent, out var deleg)) return;

            if (deleg is AKCallback akCallback)
            {
                akCallback(akEvent);
            }
            else
            {
                throw createBroadcastSignatureException(akEvent);
            }
        }

        public void Broadcast<T1>(AKEvent akEvent, T1 arg1)
        {
            if (akEvent.IsNone) return;

            if (!eventTable.TryGetValue(akEvent, out var deleg)) return;

            if (deleg is AKCallback<T1> akCallback)
            {
                akCallback(akEvent, arg1);
            }
            else
            {
                throw createBroadcastSignatureException(akEvent);
            }
        }

        public void Broadcast<T1, T2>(AKEvent akEvent, T1 arg1, T2 arg2)
        {
            if (akEvent.IsNone) return;

            if (!eventTable.TryGetValue(akEvent, out var deleg)) return;

            if (deleg is AKCallback<T1, T2> akCallback)
            {
                akCallback(akEvent, arg1, arg2);
            }
            else
            {
                throw createBroadcastSignatureException(akEvent);
            }
        }

        public void Broadcast<T1, T2, T3>(AKEvent akEvent, T1 arg1, T2 arg2, T3 arg3)
        {
            if (akEvent.IsNone) return;

            if (!eventTable.TryGetValue(akEvent, out var deleg)) return;

            if (deleg is AKCallback<T1, T2, T3> akCallback)
            {
                akCallback(akEvent, arg1, arg2, arg3);
            }
            else
            {
                throw createBroadcastSignatureException(akEvent);
            }
        }

        public void Broadcast<T1, T2, T3, T4>(AKEvent akEvent, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (akEvent.IsNone) return;

            if (!eventTable.TryGetValue(akEvent, out var deleg)) return;

            if (deleg is AKCallback<T1, T2, T3, T4> akCallback)
            {
                akCallback(akEvent, arg1, arg2, arg3, arg4);
            }
            else
            {
                throw createBroadcastSignatureException(akEvent);
            }
        }

        #endregion

        #region AKEvents Private

        private void onListenerAdding(AKEvent akEvent, Delegate listenerBeingAdded)
        {
            if (akEvent.IsNone) return;

            if (!eventTable.ContainsKey(akEvent))
            {
                eventTable.Add(akEvent, null);
            }

            var deleg = eventTable[akEvent];

            if (deleg != null && deleg.GetType() != listenerBeingAdded.GetType())
            {
                throw new AKListenerException(
                    $"Attempting to add listener with inconsistent signature for event type {akEvent}. Current listeners have type {deleg.GetType().Name} and listener being added has type {listenerBeingAdded.GetType().Name}");
            }
        }


        private void onListenerRemoving(AKEvent akEvent, Delegate listenerBeingRemoved)
        {
            if (eventTable.ContainsKey(akEvent))
            {
                var deleg = eventTable[akEvent];

                if (deleg == null)
                {
                    throw new AKListenerException(
                        $"Attempting to remove listener with for event type \"{akEvent}\" but current listener is null.");
                }
                else if (deleg.GetType() != listenerBeingRemoved.GetType())
                {
                    throw new AKListenerException(
                        $"Attempting to remove listener with inconsistent signature for event type {akEvent}. Current listeners have type {deleg.GetType().Name} and listener being removed has type {listenerBeingRemoved.GetType().Name}");
                }
            }
        }
        
        private void onListenerRemoved(AKEvent akEvent)
        {
            if (!eventTable.ContainsKey(akEvent)) return;
            if (eventTable[akEvent] == null)
            {
                eventTable.Remove(akEvent);
            }
        }
        

        private static AKBroadcastException createBroadcastSignatureException(AKEvent akEvent)
        {
            return
                new AKBroadcastException(
                    $"Broadcasting message \"{akEvent}\" but listeners have a different signature than the broadcaster.");
        }

        #endregion

        public void PrintEventTable()
        {
            AKDebug.Log("\t\t\t=== MESSENGER PrintEventTable ===");

            foreach (var pair in eventTable)
            {
                AKDebug.Log("\t\t\t" + pair.Key + "\t\t" + pair.Value);
            }

            AKDebug.Log("\n");
        }

        //         private readonly Dictionary<AKEvent, List<AKCallback>> table0 = new();
        // private readonly Dictionary<AKEvent, List<object>> table1 = new();
        // private readonly Dictionary<AKEvent, List<object>> table2 = new();
        // private readonly Dictionary<AKEvent, List<object>> table3 = new();
        // private readonly Dictionary<AKEvent, List<object>> table4 = new();

        // #region Listener

        // public void AddListener(AKEvent e, AKCallback h) => add(table0, e, h);
        // public void RemoveListener(AKEvent e, AKCallback h) => remove(table0, e, h);

        // public void AddListener<T1>(AKEvent e, AKCallback<T1> h) => add(table1, e, h);
        // public void RemoveListener<T1>(AKEvent e, AKCallback<T1> h) => remove(table1, e, h);

        // public void AddListener<T1, T2>(AKEvent e, AKCallback<T1, T2> h) => add(table2, e, h);
        // public void RemoveListener<T1, T2>(AKEvent e, AKCallback<T1, T2> h) => remove(table2, e, h);

        // public void AddListener<T1, T2, T3>(AKEvent e, AKCallback<T1, T2, T3> h) => add(table3, e, h);
        // public void RemoveListener<T1, T2, T3>(AKEvent e, AKCallback<T1, T2, T3> h) => remove(table3, e, h);

        // public void AddListener<T1, T2, T3, T4>(AKEvent e, AKCallback<T1, T2, T3, T4> h) => add(table4, e, h);
        // public void RemoveListener<T1, T2, T3, T4>(AKEvent e, AKCallback<T1, T2, T3, T4> h) => remove(table4, e, h);

        // #endregion

        // #region Broadcast

        // public void Broadcast(AKEvent e)
        // {
        //     if (!table0.TryGetValue(e, out var list)) return;

        //     for (int i = list.Count - 1; i >= 0; i--)
        //     {
        //         var cb = list[i];
        //         if (cb == null || (cb.Target is UnityEngine.Object uo && uo == null))
        //         {
        //             list.RemoveAt(i);
        //             continue;
        //         }
        //         try { cb(e); }
        //         catch (Exception ex) { Debug.LogError($"[AKEvents] {ex}"); }
        //     }
        //     if (list.Count == 0) table0.Remove(e);
        // }

        // public void Broadcast<T1>(AKEvent e, T1 a1)
        //     => broadcast(table1, e, (AKCallback<T1> cb) => cb(e, a1));

        // public void Broadcast<T1, T2>(AKEvent e, T1 a1, T2 a2)
        //     => broadcast(table2, e, (AKCallback<T1, T2> cb) => cb(e, a1, a2));

        // public void Broadcast<T1, T2, T3>(AKEvent e, T1 a1, T2 a2, T3 a3)
        //     => broadcast(table3, e, (AKCallback<T1, T2, T3> cb) => cb(e, a1, a2, a3));

        // public void Broadcast<T1, T2, T3, T4>(AKEvent e, T1 a1, T2 a2, T3 a3, T4 a4)
        //     => broadcast(table4, e, (AKCallback<T1, T2, T3, T4> cb) => cb(e, a1, a2, a3, a4));

        // #endregion

        // #region Helpers

        // private static void add<T>(Dictionary<AKEvent, List<T>> table, AKEvent e, T h) where T : class
        // {
        //     if (e.IsNone || h == null) return;
        //     if (!table.TryGetValue(e, out var list))
        //     {
        //         list = new List<T>(4);
        //         table[e] = list;
        //     }
        //     if (!list.Contains(h)) list.Add(h);
        // }

        // private static void remove<T>(Dictionary<AKEvent, List<T>> table, AKEvent e, T h) where T : class
        // {
        //     if (e.IsNone || h == null) return;
        //     if (!table.TryGetValue(e, out var list)) return;

        //     list.Remove(h);
        //     if (list.Count == 0) table.Remove(e);
        // }

        // private static void broadcast<T>(Dictionary<AKEvent, List<object>> table, AKEvent e, Action<T> invoke) where T : class
        // {
        //     if (!table.TryGetValue(e, out var list)) return;

        //     for (int i = list.Count - 1; i >= 0; i--)
        //     {
        //         if (list[i] is not T cb)
        //         {
        //             list.RemoveAt(i);
        //             continue;
        //         }

        //         if (cb is Delegate d && d.Target is UnityEngine.Object uo && uo == null)
        //         {
        //             list.RemoveAt(i);
        //             continue;
        //         }

        //         try { invoke(cb); }
        //         catch (Exception ex) { Debug.LogError($"[AKEvents] {ex}"); }
        //     }
        //     if (list.Count == 0) table.Remove(e);
        // }

        // #endregion

        // public void PrintEventTable()
        // {
        //     Debug.Log("=== AKEvents EventTable ===");
        //     Debug.Log($"NoArgs: {table0.Count}, Args1: {table1.Count}, Args2: {table2.Count}, Args3: {table3.Count}, Args4: {table4.Count}");
        // }
    }
}
