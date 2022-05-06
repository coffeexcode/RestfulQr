using QRCoder;
using RestfulQr.Domain;
using RestfulQr.Domain.ViewModels;
using System;
using System.Threading.Tasks;
using static QRCoder.PayloadGenerator;

namespace RestfulQr.Core.Rendering
{
    internal class QrCodeDataFactory
    {
        private readonly QrCodeRenderOptions renderOptions;
        private readonly QRCodeGenerator generator;
        public QrCodeDataFactory(QrCodeRenderOptions renderOptions)
        {
            this.renderOptions = renderOptions;
            generator = new QRCodeGenerator();
        }

        public async Task<QRCodeData?> CreateAsync(QrCodeType type, object content)
        {
           var data = type switch
            {
                QrCodeType.Text or QrCodeType.Json => await GenerateTextContentQrCode((string)content),
                QrCodeType.Bookmark => await GenerateBookmarkQrCode((CreateBookmarkModel)content),
                QrCodeType.CalendarEvent => await GenerateCalendarEventQrCode((CreateCalendarEventModel)content),
                QrCodeType.Contact => await GenerateContactDataQrCode((CreateContactDataEventModel)content),
                QrCodeType.Email => await GenerateEmailQrCode((CreateEmailModel)content),
                QrCodeType.Geolocation => await GenerateGeolocationQrCode((CreateGeoLocationModel)content),
                QrCodeType.TextMessage => await GenerateTextMessageQrCode((CreateTextMessageModel)content),
                QrCodeType.Url => await GenerateURLQrCode((CreateUrlModel)content),
                QrCodeType.WhatsApp => await GenerateWhatsAppMessageQrCode((CreateWhatsAppMessageModel)content),
                QrCodeType.Wifi => await GenerateWifiQrCode((CreateWifiModel)content),
                _ => null
            };

            return data;
        }

        private async Task<QRCodeData> GenerateTextContentQrCode(string textContent)
        {
            return await GenerateAsync(textContent);
        }

        private async Task<QRCodeData> GenerateBookmarkQrCode(CreateBookmarkModel model)
        {
            var bookmark = new Bookmark(model.Url, model.Title);

            return await GenerateAsync(bookmark);
        }

        private async Task<QRCodeData> GenerateCalendarEventQrCode(CreateCalendarEventModel model)
        {
            var calendarEvent = new CalendarEvent(model.Subject, model.Description, model.Location, model.Start.DateTime, model.End?.DateTime ?? DateTime.MinValue, model.AllDay, model.EventEncoding);

            return await GenerateAsync(calendarEvent);
        }

        private async Task<QRCodeData> GenerateContactDataQrCode(CreateContactDataEventModel model)
        {
            var contactData = new ContactData(
                model.OutputType,
                model.FirstName,
                model.LastName,
                model.Nickname,
                model.Phone,
                model.MobilePhone,
                model.WorkPhone,
                model.Email,
                model.Birthday,
                model.Website,
                model.Street,
                model.HouseNumber,
                model.City,
                model.PostalZip,
                model.Country,
                model.Note,
                model.ProvinceState,
                model.AddressOrder
            );

            return await GenerateAsync(contactData);
        }

        private async Task<QRCodeData> GenerateEmailQrCode(CreateEmailModel model)
        {
            var mail = new Mail(model.Recipient, model.Subject, model.Body, model.Encoding);

            return await GenerateAsync(mail);
        }

        private async Task<QRCodeData> GenerateURLQrCode(CreateUrlModel model)
        {
            var url = new Url(model.Url);

            return await GenerateAsync(url);
        }

        private async Task<QRCodeData> GenerateGeolocationQrCode(CreateGeoLocationModel model)
        {
            var geolocation = new Geolocation(model.Latitude, model.Longitude, model.Encoding);

            return await GenerateAsync(geolocation);
        }

        private async Task<QRCodeData> GenerateWhatsAppMessageQrCode(CreateWhatsAppMessageModel model)
        {
            var message = new WhatsAppMessage(model.Phone, model.Body);

            return await GenerateAsync(message);
        }

        private async Task<QRCodeData> GenerateWifiQrCode(CreateWifiModel model)
        {
            var wifi = new WiFi(model.Ssid, model.Password, model.Authentication, model.Hidden);

            return await GenerateAsync(wifi);
        }

        private async Task<QRCodeData> GenerateTextMessageQrCode(CreateTextMessageModel model)
        {
            Payload payload;

            switch (model.TextMessageType)
            {
                case TextMessageType.SMS:
                    payload = new SMS(model.Phone, model.Body, model.SmsEncoding);
                    break;
                case TextMessageType.MMS:
                    payload = new MMS(model.Phone, model.Body, model.MmsEncoding);
                    break;
                default: throw new NotImplementedException();
            }

            return await GenerateAsync(payload);
        }

        private async Task<QRCodeData> GenerateAsync(Payload payload)
        {
            return await GenerateAsync(payload.ToString());
        }

        private async Task<QRCodeData> GenerateAsync(string payload)
        {
            return await Task.Run(() => generator.CreateQrCode(payload, renderOptions.EccLevel));
        }
    }
}