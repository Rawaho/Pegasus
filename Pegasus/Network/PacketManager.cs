using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NLog;
using Pegasus.Network.Packet.Object;
using Pegasus.Network.Packet.Raw;
using Pegasus.Network.Packet.Update;

namespace Pegasus.Network
{
    public static class PacketManager
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private delegate IReadable RawMessageFactory();
        private static ImmutableDictionary<ClientRawOpcode, RawMessageFactory> rawMessageFactories;
        private static ImmutableDictionary<Type, ServerRawOpcode> rawMessageOpcodes;

        private delegate IReadable UpdateMessageFactory();
        private static ImmutableDictionary<UpdateType, UpdateMessageFactory> updateMessageFactories;
        private static ImmutableDictionary<Type, UpdateType> updateMessageTypes;

        private delegate void ObjectMessageHandler(NetworkSession session, NetworkObject networkObject);
        private delegate void RawMessageHandler(NetworkSession session, IReadable packet);
        private delegate void UpdateMessageHandler(NetworkSession session, IReadable packet, UpdateParameters parameters);

        private static ImmutableDictionary<ClientRawOpcode, RawMessageHandler> rawMessageHandlers;
        private static ImmutableDictionary<ObjectOpcode, ObjectMessageHandler> objectMessageHandlers;
        private static ImmutableDictionary<UpdateType, UpdateMessageHandler> updateMessageHandlers;

        public static void Initialise()
        {
            InitialiseRawMessages();
            InitialiseUpdateMessages();

            InitialiseRawMessageHandlers();
            InitialiseObjectMessageHandlers();
            InitialiseUpdateMessageHandlers();
        }

        private static void InitialiseRawMessages()
        {
            var clientBuilder = ImmutableDictionary.CreateBuilder<ClientRawOpcode, RawMessageFactory>();
            var serverBuilder = ImmutableDictionary.CreateBuilder<Type, ServerRawOpcode>();

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                ClientRawPacketAttribute clientAttribute = type.GetCustomAttribute<ClientRawPacketAttribute>();
                if (clientAttribute != null)
                {
                    NewExpression @new = Expression.New(type);
                    clientBuilder.Add(clientAttribute.Opcode, Expression.Lambda<RawMessageFactory>(@new).Compile());
                }

                ServerRawMessageAttribute serverAttribute = type.GetCustomAttribute<ServerRawMessageAttribute>();
                if (serverAttribute != null)
                    serverBuilder.Add(type, serverAttribute.Opcode);
            }

            rawMessageFactories = clientBuilder.ToImmutable();
            rawMessageOpcodes   = serverBuilder.ToImmutable();
        }

        /// <summary>
        /// Return a new <see cref="IReadable"/> for supplied <see cref="ClientRawOpcode"/>.
        /// </summary>
        public static IReadable CreateRawMessage(ClientRawOpcode opcode)
        {
            if (!rawMessageFactories.TryGetValue(opcode, out RawMessageFactory factory))
            {
                log.Warn($"Received unknown raw opcode 0x{opcode:X}!");
                return null;
            }

            return factory.Invoke();
        }

        /// <summary>
        /// Return <see cref="ServerRawOpcode"/> for supplied <see cref="IWritable"/>.
        /// </summary>
        public static bool GetRawOpcode(IWritable message, out ServerRawOpcode opcode)
        {
            return rawMessageOpcodes.TryGetValue(message.GetType(), out opcode);
        }

        private static void InitialiseUpdateMessages()
        {
            var clientBuilder = ImmutableDictionary.CreateBuilder<UpdateType, UpdateMessageFactory>();
            var serverBuilder = ImmutableDictionary.CreateBuilder<Type, UpdateType>();

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (UpdateMessageAttribute attribute in type.GetCustomAttributes<UpdateMessageAttribute>())
                {
                    if (typeof(IReadable).IsAssignableFrom(type))
                    {
                        NewExpression @new = Expression.New(type);
                        clientBuilder.Add(attribute.UpdateType, Expression.Lambda<UpdateMessageFactory>(@new).Compile());
                    }
                    if (typeof(IWritable).IsAssignableFrom(type))
                        serverBuilder.Add(type, attribute.UpdateType);
                }
            }

            updateMessageFactories = clientBuilder.ToImmutable();
            updateMessageTypes     = serverBuilder.ToImmutable();
        }

        /// <summary>
        /// Return a new <see cref="IReadable"/> for supplied <see cref="UpdateType"/>.
        /// </summary>
        public static IReadable CreateUpdateMessage(UpdateType type)
        {
            if (!updateMessageFactories.TryGetValue(type, out UpdateMessageFactory factory))
            {
                log.Warn($"Received unknown update type 0x{type:X}!");
                return null;
            }

            return factory.Invoke();
        }

        /// <summary>
        /// Return <see cref="UpdateType"/> for supplied <see cref="IWritable"/>.
        /// </summary>
        public static bool GetUpdateType(IWritable message, out UpdateType updateType)
        {
            return updateMessageTypes.TryGetValue(message.GetType(), out updateType);
        }

        private static void InitialiseRawMessageHandlers()
        {
            var builder = ImmutableDictionary.CreateBuilder<ClientRawOpcode, RawMessageHandler>();

            foreach (MethodInfo methodInfo in Assembly.GetExecutingAssembly()
                .GetTypes()
                .SelectMany(t => t.GetMethods()))
            {
                RawMessageHandlerAttribute attribute = methodInfo.GetCustomAttribute<RawMessageHandlerAttribute>();
                if (attribute == null)
                    continue;

                ParameterInfo[] parameters = methodInfo.GetParameters();

                #region Debug
                Debug.Assert(parameters.Length == 2);
                Debug.Assert(typeof(NetworkSession).IsAssignableFrom(parameters[0].ParameterType));
                Debug.Assert(typeof(IReadable).IsAssignableFrom(parameters[1].ParameterType));
                #endregion

                ParameterExpression sessionParameter = Expression.Parameter(typeof(NetworkSession));
                ParameterExpression messageParameter = Expression.Parameter(typeof(IReadable));

                MethodCallExpression methodCall = Expression.Call(methodInfo,
                    Expression.Convert(sessionParameter, parameters[0].ParameterType),
                    Expression.Convert(messageParameter, parameters[1].ParameterType));
                Expression<RawMessageHandler> lambda = Expression.Lambda<RawMessageHandler>(methodCall, sessionParameter, messageParameter);

                builder.Add(attribute.Opcode, lambda.Compile());
            }

            rawMessageHandlers = builder.ToImmutable();
        }

        /// <summary>
        /// Invoke message handler delegate for supplied <see cref="ClientRawOpcode"/>.
        /// </summary>
        public static void InvokeRawMessageHandler(NetworkSession session, ClientRawOpcode opcode, IReadable message)
        {
            if (!rawMessageHandlers.TryGetValue(opcode, out RawMessageHandler handler))
            {
                log.Warn($"Received unhandled raw opcode 0x{opcode:X}!");
                return;
            }

            handler.Invoke(session, message);
        }

        private static void InitialiseObjectMessageHandlers()
        {
            var builder = ImmutableDictionary.CreateBuilder<ObjectOpcode, ObjectMessageHandler>();

            foreach (MethodInfo methodInfo in Assembly.GetExecutingAssembly()
                .GetTypes()
                .SelectMany(t => t.GetMethods()))
            {
                ObjectPacketHandlerAttribute attribute = methodInfo.GetCustomAttribute<ObjectPacketHandlerAttribute>();
                if (attribute == null)
                    continue;

                ParameterInfo[] parameters = methodInfo.GetParameters();

                #region Debug
                Debug.Assert(parameters.Length == 2);
                Debug.Assert(typeof(NetworkSession).IsAssignableFrom(parameters[0].ParameterType));
                #endregion

                ParameterExpression sessionParameter = Expression.Parameter(typeof(NetworkSession));
                ParameterExpression messageParameter = Expression.Parameter(typeof(NetworkObject));

                MethodCallExpression methodCall = Expression.Call(methodInfo, Expression.Convert(sessionParameter, parameters[0].ParameterType), messageParameter);
                Expression<ObjectMessageHandler> lambda = Expression.Lambda<ObjectMessageHandler>(methodCall, sessionParameter, messageParameter);

                builder.Add(attribute.Opcode, lambda.Compile());
            }

            objectMessageHandlers = builder.ToImmutable();
        }

        /// <summary>
        /// Invoke message handler delegate for supplied <see cref="ObjectOpcode"/>.
        /// </summary>
        public static void InvokeObjectMessageHandler(NetworkSession session, ObjectOpcode opcode, NetworkObject networkObject)
        {
            if (!objectMessageHandlers.TryGetValue(opcode, out ObjectMessageHandler handler))
            {
                log.Warn($"Received unhandled object opcode 0x{opcode:X}!");
                return;
            }

            handler.Invoke(session, networkObject);
        }

        private static void InitialiseUpdateMessageHandlers()
        {
            var builder = ImmutableDictionary.CreateBuilder<UpdateType, UpdateMessageHandler>();

            foreach (MethodInfo methodInfo in Assembly.GetExecutingAssembly()
                .GetTypes()
                .SelectMany(t => t.GetMethods()))
            {
                foreach (UpdateMessageHandlerAttribute attribute in methodInfo.GetCustomAttributes<UpdateMessageHandlerAttribute>())
                {
                    ParameterInfo[] parameters = methodInfo.GetParameters();

                    #region Debug
                    Debug.Assert(parameters.Length == 3);
                    Debug.Assert(typeof(NetworkSession).IsAssignableFrom(parameters[0].ParameterType));
                    Debug.Assert(typeof(IReadable).IsAssignableFrom(parameters[1].ParameterType));
                    Debug.Assert(parameters[2].ParameterType == typeof(UpdateParameters));
                    #endregion

                    ParameterExpression sessionParameter = Expression.Parameter(typeof(NetworkSession));
                    ParameterExpression messageParameter = Expression.Parameter(typeof(IReadable));
                    ParameterExpression parmsParameter = Expression.Parameter(typeof(UpdateParameters));

                    MethodCallExpression methodCall = Expression.Call(methodInfo,
                        Expression.Convert(sessionParameter, parameters[0].ParameterType), Expression.Convert(messageParameter, parameters[1].ParameterType), parmsParameter);
                    Expression<UpdateMessageHandler> lambda = Expression.Lambda<UpdateMessageHandler>(methodCall, sessionParameter, messageParameter, parmsParameter);

                    builder.Add(attribute.UpdateType, lambda.Compile());
                }
            }

            updateMessageHandlers = builder.ToImmutable();
        }

        /// <summary>
        /// Invoke message handler delegate for supplied <see cref="UpdateType"/>.
        /// </summary>
        public static void InvokeUpdatePacketHandler(NetworkSession session, UpdateType updateType, IReadable packet, UpdateParameters parameters)
        {
            if (!updateMessageHandlers.TryGetValue(updateType, out UpdateMessageHandler handler))
            {
                log.Warn($"Received unhandled update type 0x{updateType:X}!");
                return;
            }

            handler.Invoke(session, packet, parameters);
        }
    }
}
