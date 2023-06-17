using System;
using Infrastructure.StateMachine;

namespace Infrastructure.StateMachine
{
    public class TransitionParam
    {
        public Type Type;
        public IPayload Payload;
    }
}