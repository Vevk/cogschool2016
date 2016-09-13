using System;
using System.Net;

namespace Bot_Application1
{
    /// <summary>
    /// The Exception will be shown to client.
    /// </summary>
    public class ClientException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientException"/> class.
        /// </summary>
        /// <param name="error">The error entity.</param>
        /// <param name="httpStatus">The http status.</param>
        public ClientException(ClientError error, HttpStatusCode httpStatus)
        {
            this.Error = error;
            this.HttpStatus = httpStatus;
        }

        /// <summary>
        /// Gets http status of http response.
        /// </summary>
        /// <value>
        /// The HTTP status.
        /// </value>
        public HttpStatusCode HttpStatus { get; private set; }

        /// <summary>
        /// Gets or sets the httpError message.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        public ClientError Error { get; set; }
    }

}