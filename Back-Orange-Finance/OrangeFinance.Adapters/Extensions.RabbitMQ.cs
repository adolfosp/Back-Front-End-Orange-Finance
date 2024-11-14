using System.Text;

using RabbitMQ.Client;

namespace OrangeFinance.Adapters;

internal static partial class Extension
{
    public static IBasicProperties SetMessageId(this IBasicProperties basicProperties, string messageId = null)
    {
        ArgumentNullException.ThrowIfNull(basicProperties);
        basicProperties.MessageId = messageId ?? Guid.NewGuid().ToString("D");
        return basicProperties;
    }

    public static IBasicProperties SetReplyTo(this IBasicProperties basicProperties, string replyTo = null)
    {
        ArgumentNullException.ThrowIfNull(basicProperties);

        if (!string.IsNullOrEmpty(replyTo))
        {
            basicProperties.ReplyTo = replyTo;
        }

        return basicProperties;
    }

    private static string AsString(this object objectToConvert)
    {
        return objectToConvert != null ? Encoding.UTF8.GetString((byte[])objectToConvert) : null;
    }

    public static string AsString(this IDictionary<string, object> dic, string key)
    {
        object content = dic?[key];
        return (content != null) ? Encoding.UTF8.GetString((byte[])content) : null;
    }

    public static bool TryReconstructException(this IBasicProperties basicProperties, out AmqpRpcRemoteException remoteException)
    {
        remoteException = default;
        if (basicProperties?.Headers?.ContainsKey("exception.type") ?? false)
        {
            string exceptionTypeString = basicProperties.Headers.AsString("exception.type");
            string exceptionMessage = basicProperties.Headers.AsString("exception.message");
            string exceptionStackTrace = basicProperties.Headers.AsString("exception.stacktrace");
            Exception exceptionInstance = (Exception)Activator.CreateInstance(Type.GetType(exceptionTypeString) ?? typeof(Exception), exceptionMessage);
            remoteException = new AmqpRpcRemoteException("Remote consumer report a exception during execution", exceptionStackTrace, exceptionInstance);
            return true;
        }
        return false;
    }

}
