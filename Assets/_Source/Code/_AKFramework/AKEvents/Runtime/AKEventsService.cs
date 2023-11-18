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
    }
}