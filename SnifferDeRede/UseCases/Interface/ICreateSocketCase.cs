using System;
using System.Net.Sockets;

namespace SnifferDeRede.UseCases.Interface
{
    public interface ICreateSocketCase
    {
        Socket Execute(string ip, AsyncCallback onReceive);
    }
}
