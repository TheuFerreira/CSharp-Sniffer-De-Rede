using System.Collections.Generic;

namespace SnifferDeRede.UseCases.Interface
{
    public interface IGetAllLocalIpAddressCase
    {
        IList<string> Execute();
    }
}
