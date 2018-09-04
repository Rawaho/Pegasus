using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using NLog;
using Pegasus.Network.Packet;
using Pegasus.Network.Packet.Raw;
using Pegasus.Network.Packet.Update;

namespace Pegasus.Network
{
    public static class PacketManager
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private delegate ClientRawPacket RawPacketFactory();
        private static readonly Dictionary<ClientRawOpcode, RawPacketFactory> rawPacketFactories
            = new Dictionary<ClientRawOpcode, RawPacketFactory>();

        private delegate ClientUpdatePacket UpdatePacketFactory();
        private static readonly Dictionary<UpdateType, UpdatePacketFactory> updatePacketFactories
            = new Dictionary<UpdateType, UpdatePacketFactory>();

        private delegate void ObjectPacketHandler(NetworkSession session, NetworkObject networkObject);
        private delegate void RawPacketHandler(NetworkSession session, ClientRawPacket packet);
        private delegate void UpdatePacketHandler(NetworkSession session, ClientUpdatePacket packet, UpdateParameters parameters);

        private static readonly Dictionary<ObjectOpcode, ObjectPacketHandler> objectPacketHandlers
            = new Dictionary<ObjectOpcode, ObjectPacketHandler>();
        private static readonly Dictionary<ClientRawOpcode, RawPacketHandler> rawPacketHandlers
            = new Dictionary<ClientRawOpcode, RawPacketHandler>();
        private static readonly Dictionary<UpdateType, UpdatePacketHandler> updatePacketHandlers
            = new Dictionary<UpdateType, UpdatePacketHandler>();

        public static void Initialise()
        {
            InitialiseRawPacketFactories();
            InitialiseUpdateacketFactories();

            InitialiseRawPacketHandlers();
            InitialiseObjectPacketHandlers();
            InitialiseUpdatePacketHandlers();
        }

        /// <summary>
        /// 
        /// </summary>
        private static void InitialiseRawPacketFactories()
        {
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                ClientRawPacketAttribute attribute = type.GetCustomAttribute<ClientRawPacketAttribute>();
                if (attribute == null)
                    continue;

                NewExpression newFactory = Expression.New(type);
                Expression<RawPacketFactory> lambda = Expression.Lambda<RawPacketFactory>(newFactory);

                rawPacketFactories.Add(attribute.Opcode, lambda.Compile());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static ClientRawPacket CreateRawPacket(ClientRawOpcode opcode)
        {
            if (!rawPacketFactories.TryGetValue(opcode, out RawPacketFactory factory))
            {
                log.Warn($"Received unknown raw opcode 0x{opcode:X}!");
                return null;
            }

            return factory.Invoke();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void InitialiseUpdateacketFactories()
        {
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (UpdatePacketAttribute attribute in type.GetCustomAttributes<UpdatePacketAttribute>())
                {
                    NewExpression newFactory = Expression.New(type);
                    Expression<UpdatePacketFactory> lambda = Expression.Lambda<UpdatePacketFactory>(newFactory);

                    updatePacketFactories.Add(attribute.UpdateType, lambda.Compile());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static ClientUpdatePacket CreateUpdatePacket(UpdateType type)
        {
            if (!updatePacketFactories.TryGetValue(type, out UpdatePacketFactory factory))
            {
                log.Warn($"Received unknown update type 0x{type:X}!");
                return null;
            }

            return factory.Invoke();
        }

        /// <summary>
        /// 
        /// </summary>
        private static void InitialiseRawPacketHandlers()
        {
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (MethodInfo methodInfo in type.GetMethods())
                {
                    RawPacketHandlerAttribute attribute = methodInfo.GetCustomAttribute<RawPacketHandlerAttribute>();
                    if (attribute == null)
                        continue;

                    ParameterInfo[] parameters = methodInfo.GetParameters();

                    Debug.Assert(parameters.Length == 2);
                    Debug.Assert(typeof(NetworkSession).IsAssignableFrom(parameters[0].ParameterType));
                    Debug.Assert(typeof(ClientRawPacket).IsAssignableFrom(parameters[1].ParameterType));

                    ParameterExpression sessionParameter = Expression.Parameter(typeof(NetworkSession));
                    ParameterExpression packetParameter = Expression.Parameter(typeof(ClientRawPacket));

                    MethodCallExpression methodCall = Expression.Call(methodInfo,
                        Expression.Convert(sessionParameter, parameters[0].ParameterType),
                        Expression.Convert(packetParameter, parameters[1].ParameterType));
                    Expression<RawPacketHandler> lambda = Expression.Lambda<RawPacketHandler>(methodCall, sessionParameter, packetParameter);

                    rawPacketHandlers.Add(attribute.Opcode, lambda.Compile());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void InvokeRawPacketHandler(NetworkSession session, ClientRawOpcode opcode, ClientRawPacket packet)
        {
            if (!rawPacketHandlers.TryGetValue(opcode, out RawPacketHandler handler))
            {
                log.Warn($"Received unhandled raw opcode 0x{opcode:X}!");
                return;
            }

            handler.Invoke(session, packet);
        }

        /// <summary>
        /// 
        /// </summary>
        private static void InitialiseObjectPacketHandlers()
        {
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (MethodInfo methodInfo in type.GetMethods())
                {
                    ObjectPacketHandlerAttribute attribute = methodInfo.GetCustomAttribute<ObjectPacketHandlerAttribute>();
                    if (attribute == null)
                        continue;

                    ParameterInfo[] parameters = methodInfo.GetParameters();

                    Debug.Assert(parameters.Length == 2);
                    Debug.Assert(typeof(NetworkSession).IsAssignableFrom(parameters[0].ParameterType));

                    ParameterExpression sessionParameter = Expression.Parameter(typeof(NetworkSession));
                    ParameterExpression objectParameter = Expression.Parameter(typeof(NetworkObject));

                    MethodCallExpression methodCall = Expression.Call(methodInfo, Expression.Convert(sessionParameter, parameters[0].ParameterType), objectParameter);
                    Expression<ObjectPacketHandler> lambda = Expression.Lambda<ObjectPacketHandler>(methodCall, sessionParameter, objectParameter);

                    objectPacketHandlers.Add(attribute.Opcode, lambda.Compile());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void InvokeObjectPacketHandler(NetworkSession session, ObjectOpcode opcode, NetworkObject networkObject)
        {
            if (!objectPacketHandlers.TryGetValue(opcode, out ObjectPacketHandler handler))
            {
                log.Warn($"Received unhandled object opcode 0x{opcode:X}!");
                return;
            }

            handler.Invoke(session, networkObject);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void InitialiseUpdatePacketHandlers()
        {
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (MethodInfo methodInfo in type.GetMethods())
                {
                    UpdatePacketHandlerAttribute attribute = methodInfo.GetCustomAttribute<UpdatePacketHandlerAttribute>();
                    if (attribute == null)
                        continue;

                    ParameterInfo[] parameters = methodInfo.GetParameters();

                    Debug.Assert(parameters.Length == 3);
                    Debug.Assert(typeof(NetworkSession).IsAssignableFrom(parameters[0].ParameterType));
                    Debug.Assert(typeof(ClientUpdatePacket).IsAssignableFrom(parameters[1].ParameterType));
                    Debug.Assert(parameters[2].ParameterType == typeof(UpdateParameters));

                    ParameterExpression sessionParameter = Expression.Parameter(typeof(NetworkSession));
                    ParameterExpression packetParameter  = Expression.Parameter(typeof(ClientUpdatePacket));
                    ParameterExpression parmsParameter   = Expression.Parameter(typeof(UpdateParameters));

                    MethodCallExpression methodCall = Expression.Call(methodInfo,
                        Expression.Convert(sessionParameter, parameters[0].ParameterType), Expression.Convert(packetParameter, parameters[1].ParameterType), parmsParameter);
                    Expression<UpdatePacketHandler> lambda = Expression.Lambda<UpdatePacketHandler>(methodCall, sessionParameter, packetParameter, parmsParameter);

                    updatePacketHandlers.Add(attribute.UpdateType, lambda.Compile());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void InvokeUpdatePacketHandler(NetworkSession session, UpdateType updateType, ClientUpdatePacket packet, UpdateParameters parameters)
        {
            if (!updatePacketHandlers.TryGetValue(updateType, out UpdatePacketHandler handler))
            {
                log.Warn($"Received unhandled update type 0x{updateType:X}!");
                return;
            }

            handler.Invoke(session, packet, parameters);
        }
    }
}
