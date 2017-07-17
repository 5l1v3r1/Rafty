using System;

namespace Rafty.Concensus
{
    public sealed class Follower : IState
    {
        private readonly ISendToSelf _sendToSelf;

        public Follower(CurrentState state, ISendToSelf sendToSelf)
        {
            CurrentState = state;
            _sendToSelf = sendToSelf;
        }

        public CurrentState CurrentState { get; }

        public IState Handle(Timeout timeout)
        {
            _sendToSelf.Publish(new BeginElection());
            return new Candidate(CurrentState, _sendToSelf);
        }

        public IState Handle(BeginElection beginElection)
        {
            return this;
        }

        public IState Handle(AppendEntries appendEntries)
        {
            throw new NotImplementedException();
        }
    }
}