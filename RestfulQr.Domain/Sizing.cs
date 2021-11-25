using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulQr.Domain
{
    public static class Sizing
    {
        #region Object Sizing
        public const int Url = 2048;
        public const int Name = 80;
        public const int Phone = 20;
        public const int AreaCode = 10;
        public const int Email = Md;
        public const int Geolocation = 25;
        #endregion

        #region Standard Sizing
        public const int Xs = 50;
        public const int Sm = 100;
        public const int Md = 250;
        public const int Lg = 500;
        public const int Xl = 1000;
        #endregion
    }
}