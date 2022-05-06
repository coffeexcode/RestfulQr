using System;
using System.ComponentModel.DataAnnotations;
using static QRCoder.PayloadGenerator.ContactData;

namespace RestfulQr.Domain.ViewModels
{
    public class CreateContactDataEventModel
    {
        #region Required
        [Required]
        [MaxLength(Sizing.Name)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(Sizing.Name)]
        public string LastName { get; set; }

        [Required]
        public ContactOutputType OutputType { get; set; }
        #endregion

        [MaxLength(Sizing.Name)]
        public string? Nickname { get; set; }

        [MaxLength(Sizing.Phone)]
        public string? Phone { get; set; }

        [MaxLength(Sizing.Phone)]
        public string? MobilePhone { get; set; }

        [MaxLength(Sizing.Phone)]
        public string? WorkPhone { get; set; }

        [MaxLength(Sizing.Email)]
        public string? Email { get; set; }

        public DateTime? Birthday { get; set; }

        [MaxLength(Sizing.Url)]
        public string? Website { get; set; }

        [MaxLength(Sizing.Md)]
        public string? Street { get; set; }

        [MaxLength()]
        public string? HouseNumber { get; set; }

        [MaxLength(Sizing.Name)]
        public string? City { get; set; }

        [MaxLength(Sizing.AreaCode)]
        public string? PostalZip { get; set; }

        [MaxLength(Sizing.Name)]
        public string? ProvinceState { get; set; }

        [MaxLength(Sizing.Name)]
        public string? Country { get; set; }

        [MaxLength(Sizing.Xl)]
        public string? Note { get; set; }

        public AddressOrder AddressOrder { get; set; } = AddressOrder.Default;

        [MaxLength(Sizing.Name)]
        public string? Company { get; set; }
    }
}
