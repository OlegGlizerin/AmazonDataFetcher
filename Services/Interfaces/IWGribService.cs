using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IWGribService
    {
        public string CalcKelvins(string fileName);
    }
}
