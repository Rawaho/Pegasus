using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using NLog;
using Pegasus.Social;

namespace Pegasus.Network
{
    public static class NetworkManager
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public static volatile bool Shutdown = false;

        public static SequenceQueue<uint> SessionSequence { get; } = new SequenceQueue<uint>(0u);

        private static readonly HashSet<Session> sessions = new HashSet<Session>();
        private static readonly ReaderWriterLockSlim mutex = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        private static readonly ConcurrentQueue<Session> pendingAdd = new ConcurrentQueue<Session>();
        private static readonly Queue<Session> pendingRemove = new Queue<Session>();

        private static TcpListener listener;

        public static void Initialise()
        {
            listener = new TcpListener(IPAddress.Any, 13124);
            listener.Start();

            log.Info("Listening for connections on port 13124...");

            new Thread(() =>
            {
                
                try
                {
                    while (!Shutdown)
                    {
                        Thread.Sleep(1);
                        if (listener.Pending())
                        {
                            var session = new Session();
                            session.Accept(listener.AcceptSocket());
                            AddSession(session);
                        }
                    }
                }
                catch (Exception exception)
                {
                    log.Error(exception);
                }
            }).Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void AddSession(Session session)
        {
            pendingAdd.Enqueue(session);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RemoveSession(Session session)
        {
            pendingRemove.Enqueue(session);
        }

        /// <summary>
        /// 
        /// </summary>
        public static IEnumerable<Session> FindSession(string account)
        {
            mutex.EnterReadLock();
            try
            {
                return sessions.Where(s => s.State == SessionState.SignedIn && s.Account.Username.Equals(account, StringComparison.CurrentCultureIgnoreCase));
            }
            finally
            {
                mutex.ExitReadLock();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static Session FindSessionByCharacter(CharacterObject character)
        {
            mutex.EnterReadLock();
            try
            {
                return sessions.SingleOrDefault(s => s.State == SessionState.SignedIn && s.Character == character);
            }
            finally
            {
                mutex.ExitReadLock();
            }
        }

        public static void Update(double lastTick)
        {
            // remove any sessions needing to be cleaned up
            while (pendingRemove.Count > 0)
            {
                mutex.EnterWriteLock();
                try
                {
                    sessions.Remove(pendingRemove.Dequeue());
                }
                finally
                {
                    mutex.ExitWriteLock();
                }
            }

            // store any pending sessions and start receiving data
            while (!pendingAdd.IsEmpty)
            {
                mutex.EnterWriteLock();
                try
                {
                    if (pendingAdd.TryDequeue(out Session session))
                    {
                        sessions.Add(session);
                        session.BeginReceive();
                    }
                }
                finally
                {
                    mutex.ExitWriteLock();
                }
            }

            mutex.EnterReadLock();
            try
            {
                foreach (Session session in sessions)
                    session.Update(lastTick);
            }
            finally
            {
                mutex.ExitReadLock();
            }
        }
    }
}
