using _Source.Code._Core.View;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.StateMachines;
using UnityEngine;

namespace _Source.Code._AKFramework.AKNodeCanvas
{
    [DisallowMultipleComponent]
    public class AKNodeCanvasOwnerView : AKView
    {
        protected override void Init()
        {
            foreach (var fsmOwner in GetComponents<FSMOwner>())
            {
                fsmOwner.StartBehaviour();
            }
            
            foreach (var behaviourTreeOwner in GetComponents<BehaviourTreeOwner>())
            {
                behaviourTreeOwner.StartBehaviour();
            }
        }

        private void OnDestroy()
        {
            foreach (var fsmOwner in GetComponents<FSMOwner>())
            {
                fsmOwner.StopBehaviour();
            }
            
            foreach (var behaviourTreeOwner in GetComponents<BehaviourTreeOwner>())
            {
                behaviourTreeOwner.StopBehaviour();
            }
        }
    }
}