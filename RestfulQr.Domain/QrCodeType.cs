namespace RestfulQr.Domain
{
    public enum QrCodeType
    {
        /// <summary>
        /// Plain text based QR code
        /// </summary>
        Text,

        /// <summary>
        /// Valid JSON document encoded into qr code
        /// </summary>
        Json,

        /// <summary>
        /// QR Code to bookmark something
        /// </summary>
        Bookmark,

        /// <summary>
        /// QR Code to create a calendar event
        /// </summary>
        CalendarEvent,

        /// <summary>
        /// QR code to create a new contact
        /// </summary>
        Contact,

        /// <summary>
        /// QR Code to generate a new email
        /// </summary>
        Email,

        /// <summary>
        /// QR Code representing a place on the earth (or Google Maps)
        /// </summary>
        Geolocation,

        /// <summary>
        /// QR Code to generate a text message (SMS or MMS)
        /// </summary>
        TextMessage,

        /// <summary>
        /// QR Code to link to a valid URL
        /// </summary>
        Url,

        /// <summary>
        /// QR code to generate a whatsapp message
        /// </summary>
        WhatsApp,

        /// <summary>
        /// QR code to join/share a wifi network
        /// </summary>
        Wifi
    }

    public enum TextMessageType
    {
        SMS = 1,
        MMS = 2
    }
}
