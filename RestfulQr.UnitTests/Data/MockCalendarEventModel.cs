using RestfulQr.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulQr.UnitTests.Data
{
    public class MockCalendarEventModel : IMockModel<CreateCalendarEventModel>
    {
        public CreateCalendarEventModel Invalid()
        {
            return new CreateCalendarEventModel
            {
                Description = "mock description"
            };
        }

        public CreateCalendarEventModel Valid()
        {
            return new CreateCalendarEventModel
            {
                Description = "mocked",
                AllDay = true,
                EventEncoding = QRCoder.PayloadGenerator.CalendarEvent.EventEncoding.Universal,
                Location = "123 Test St, Testerville",
                Start = DateTime.Now,
                Subject = "mocked"
            };
        }
    }
}
