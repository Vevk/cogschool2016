using System.Runtime.Serialization;

namespace Bot_Application1
{
    /// <summary>
    /// Error class.
    /// </summary>
    [DataContract]
    public class ClientError
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [DataMember(Name = "code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [DataMember(Name = "message")]
        public string Message { get; set; }
    }

}