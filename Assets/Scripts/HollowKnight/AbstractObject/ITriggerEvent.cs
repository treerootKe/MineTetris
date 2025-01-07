using UnityEngine;

namespace HollowKnight.AbstractObject
{
    public interface ITriggerEvent
    { 
        void TriggerEvent(Collider2D collider2d);
        
        void TriggerExitEvent(Collider2D collider2d);
    }
}