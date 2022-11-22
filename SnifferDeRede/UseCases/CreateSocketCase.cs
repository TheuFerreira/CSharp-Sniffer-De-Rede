using SnifferDeRede.Repository.Interfaces;
using SnifferDeRede.UseCases.Interface;
using System;
using System.Net;
using System.Net.Sockets;

namespace SnifferDeRede.UseCases
{
    public class CreateSocketCase : ICreateSocketCase
    {
        private readonly IFileRepository _fileRepository;
        private readonly byte[] byteData = new byte[4096];

        public CreateSocketCase(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }   

        public Socket Execute(string ip, AsyncCallback onReceive)
        {
            try
            {
                //For sniffing the socket to capture the packets has to be a raw socket, with the address family being of type internetwork, and protocol being IP
                Socket mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);

                IPAddress ipAddress = IPAddress.Parse(ip);

                //Bind the socket to the selected IP address
                mainSocket.Bind(new IPEndPoint(ipAddress, 0));

                //Applies only to IP packets
                SocketOptionLevel socketOptionLevel = SocketOptionLevel.IP;

                //Set the include the header
                SocketOptionName socketOptionName = SocketOptionName.HeaderIncluded;

                //Set the socket  options
                mainSocket.SetSocketOption(socketOptionLevel, socketOptionName, true);

                //Capture outgoing packets
                byte[] byOut = new byte[4] { 1, 0, 0, 0 };
                byte[] byTrue = new byte[4] { 1, 0, 0, 0 };

                //Equivalent to SIO_RCVALL constant of Winsock 2
                IOControlCode ioControlCode = IOControlCode.ReceiveAll;

                //Socket.IOControl is analogous to the WSAIoctl method of Winsock 2
                mainSocket.IOControl(ioControlCode, byTrue, byOut);

                //Start receiving the packets asynchronously
                mainSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, onReceive, mainSocket);
                return mainSocket;
            }
            catch (Exception ex)
            {
                _fileRepository.WriteInfo(ex);
                return null;
            }
        }
    }
}
