﻿namespace Rafty.Log
{
    using System;
    using System.Collections.Generic;

    public class InMemoryLog : ILog
    {
        private readonly List<LogEntry> _log;

        public InMemoryLog()
        {
            _log = new List<LogEntry>();
        }

        public List<LogEntry> ExposedForTesting => _log;

        public long LastLogIndex
        {
            get
            {
                if(_log.Count == 0)
                {
                    return 0;
                }

                return _log.Count - 1;
            }
        }

        public long LastLogTerm
        {
            get
            {
                if(_log.Count == 0)
                {
                    return 0;
                }
                
                var lastLog = _log[_log.Count - 1];
                return lastLog.Term;
            }
        }
        
        public void Apply(LogEntry logEntry)
        {
            _log.Add(logEntry);
        }

        public long GetTermAtIndex(long index)
        {
            if(_log.Count == 0)
            {
                return 0;
            }

            if(index > _log.Count)
            {
                return 0;
            }

            //todo - fix?
            int i = Convert.ToInt32(index);
            return _log[i].Term;
        }

        public void DeleteConflicts(LogEntry logEntry)
        {
            var index = logEntry.CurrentCommitIndex;

            for (int i = index; i < _log.Count; i++)
            {
                var match = _log[i];
                if (match.Term != logEntry.Term)
                {
                    var toRemove = _log.Count - i;
                    _log.RemoveRange(i, toRemove);
                }
            }
        }
    }
}