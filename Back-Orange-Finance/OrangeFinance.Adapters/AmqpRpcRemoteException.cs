﻿namespace OrangeFinance.Adapters;

[Serializable]
public class AmqpRpcRemoteException : Exception
{
    private readonly string remoteStackTrace;

    public AmqpRpcRemoteException() : this(message: null, remoteStackTrace: null, inner: null) { }


    public AmqpRpcRemoteException(string message, string remoteStackTrace, Exception inner) : base(message, inner)
    {
        this.remoteStackTrace = remoteStackTrace;
    }

    public override string StackTrace => remoteStackTrace;

    protected AmqpRpcRemoteException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {

    }


}
