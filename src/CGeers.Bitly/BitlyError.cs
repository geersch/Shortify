using CGeers.Bitly.Resources;

namespace CGeers.Bitly
{
    public enum BitlyError
    {
        None,

        [Description(typeof(Text), "NoInternetConnection")]
        NoInternetConnection,

        [Description(typeof(Text), "InvalidUrl")]
        InvalidUrl,

        [Description(typeof(Text), "AlreadyABitlyLink")]
        AlreadyABitlyLink,

        [Description(typeof(Text), "UnknownError")]
        Unknown
    }
}
