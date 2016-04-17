using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stateless
{
    public partial class StateMachine<TState, TTrigger>
    {
        internal class DynamicPairTriggerBehaviour : TriggerBehaviour
        {
            readonly Func<object[], TState> _destination;

            protected Func<bool> _guardWrapper;
            protected Func<object[], bool> _guardConditional;

            public DynamicPairTriggerBehaviour(TTrigger trigger, Func<object[], TState> destination, Func<object[], bool> guardConditional, string description)
                : base(trigger, () => { return false; }, description)
            {
                _guardConditional = guardConditional;
                _destination = Enforce.ArgumentNotNull(destination, "destination");
            }

            private Func<bool> OverriddenGuard
            {
                get
                {
                    return _guard;
                }
                set {
                    _guard = value;
                }
            }

            public override bool ResultsInTransitionFrom(TState source, object[] args, out TState destination)
            {
                OverriddenGuard = () => { return _guardConditional(args); };
                destination = _destination(args);
                return true;
            }
        }
    }
}
