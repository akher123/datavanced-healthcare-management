using System;

namespace Datavanced.HealthcareManagement.Shared.ExceptionHelper
{
    /// <summary>
    /// Create the derived Dsi exception class.
    /// </summary>
    [Serializable]
    public class DatavancedException : Exception
    {
        /// <summary>
        ///  Set HResult for this exception, and include it in
        /// </summary>
        /// <param name="message"> the exception message.</param>
        /// <param name="errorCode"> Exception Code</param>
        public DatavancedException(string message, int errorCode)
           : base(message)
        {
            HResult = errorCode; // Gets or sets HRESULT, a coded numerical value that is assigned to a specific exception.
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="errorCode"></param>
        /// <param name="innerException"></param>
        public DatavancedException(string message, int errorCode, Exception innerException)
            : base(message, innerException)
        {
            HResult = errorCode;
        }
    }
}
