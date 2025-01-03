using UnityEngine;

namespace HollowKnight.AbstractClass
{
    public interface ITriggerEvent
    { 
        void TriggerEvent(Collider2D collider2d);
    }
}