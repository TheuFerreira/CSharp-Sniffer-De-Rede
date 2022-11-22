using SnifferDeRede.Models;
using SnifferDeRede.Repository;
using SnifferDeRede.Repository.Interfaces;
using SnifferDeRede.UseCases;
using SnifferDeRede.UseCases.Interface;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace SnifferDeRede
{
    public class Application
    {
        private readonly IFileRepository _exceptionRepository;
        private readonly IFileRepository _emailRepository;

        private readonly IGetAllLocalIpAddressCase getAllLocalIpAddressCase;
        private readonly ICreateSocketCase createSocketCase;
        private readonly ICreateUDPHeaderCase createUDPHeaderCase;
        private readonly ICreateTCPHeaderCase createTCPHeaderCase;
        private readonly ICreateIPHeaderCase createIPHeaderCase;

        private readonly IList<Socket> openedSockets = new List<Socket>();

        private byte[] byteData = new byte[4096];

        public Application()
        {
            string basePath = AppContext.BaseDirectory;
            _exceptionRepository = new FileRepository(basePath + "/exceptions.log");
            _emailRepository = new FileRepository(basePath + "/spy.log");

            getAllLocalIpAddressCase = new GetAllLocalIpAddressCase();
            createSocketCase = new CreateSocketCase(_exceptionRepository);
            createUDPHeaderCase = new CreateUDPHeaderCase();
            createTCPHeaderCase = new CreateTCPHeaderCase();
            createIPHeaderCase = new CreateIPHeaderCase();
        }

        public void Start()
        {
            IList<string> ips = getAllLocalIpAddressCase.Execute();
            foreach (string ip in ips)
            {
                Socket socket = createSocketCase.Execute(ip, new AsyncCallback(OnReceive));
                if (socket == null)
                    continue;

                openedSockets.Add(socket);
            }
        }

        private void OnReceive(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            try
            {
                int nReceived = socket.EndReceive(ar);

                //Analyze the bytes received...

                ParseData(byteData, nReceived);

                byteData = new byte[4096];
            }
            catch (Exception ex)
            {
                _exceptionRepository.WriteInfo(ex);
            }
            finally
            {
                //Another call to BeginReceive so that we continue to receive the incoming packets
                socket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnReceive), socket);
            }
        }

        private void ParseData(byte[] byteData, int nReceived)
        {
            IPHeader ipHeader = createIPHeaderCase.Execute(byteData, nReceived);
            _emailRepository.WriteInfo(ipHeader);

            //Now according to the protocol being carried by the IP datagram we parse 
            //the data field of the datagram
            switch (ipHeader.ProtocolType)
            {
                case Protocol.TCP:
                    TCPHeader tcpHeader = createTCPHeaderCase.Execute(ipHeader.Data, ipHeader.MessageLength);
                    _emailRepository.WriteInfo(tcpHeader);
                    break;

                case Protocol.UDP:
                    UDPHeader udpHeader = createUDPHeaderCase.Execute(ipHeader.Data, (int)ipHeader.MessageLength);
                    _emailRepository.WriteInfo(udpHeader);
                    break;

                case Protocol.Unknown:
                    break;
            }

            _emailRepository.WriteInfo("\n");
        }

        ~Application()
        {
            foreach (Socket socket in openedSockets)
            {
                socket.Close();
                socket.Dispose();
            }
        }
    }
}
