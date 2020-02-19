using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace RGalatico10.SimpleUdpServer.Core.Common.Listener
{


    public class UdpListener : IListener
    {
        private const int _bufferSize = 8 * 1024;
        private Socket _socket;
        private State _state = new State();
        private EndPoint _endPointFrom = new IPEndPoint(IPAddress.Any, 0);
        private AsyncCallback _messageReceivedCallback = null;
        private readonly ILogger _logger;

        public UdpListener(
            ILogger logguer, 
            IOptions<UdpListenerConfiguration> listenerConfiguration
            )
        {
            //Criando e configurando o skocket Udp
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.Bind(new IPEndPoint(IPAddress.Parse(listenerConfiguration.Value.IpAddress), listenerConfiguration.Value.Port));
            _logger = logguer;


            _logger.Information("Ip: {ip} - Porta: {porta}", listenerConfiguration.Value.IpAddress, listenerConfiguration.Value.Port);
        }

        public void Listen()
        {
            _socket.BeginReceiveFrom(_state.buffer, 0, _bufferSize, SocketFlags.None, ref _endPointFrom, _messageReceivedCallback = (result) =>
            {
                var message = string.Empty;
                var state = (State)result.AsyncState;
                var bytes = 0;

                try
                {

                    bytes = _socket.EndReceiveFrom(result, ref _endPointFrom);

                    message = Encoding.ASCII.GetString(state.buffer, 0, bytes);

                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        _logger.Information("MENSAGEM RECEBIDA: {message}", message);
                      
                    }
                   
                    _socket.BeginReceiveFrom(state.buffer, 0, _bufferSize, SocketFlags.None, ref _endPointFrom, _messageReceivedCallback, state);
                }
                catch (Exception ex)
                {
                   
                    _logger.Error(ex, "Ops algo ruim aconteceu.");
                    
                    _socket.BeginReceiveFrom(state.buffer, 0, _bufferSize, SocketFlags.None, ref _endPointFrom, _messageReceivedCallback, state);
                }
            }, _state);
        }


        internal class State
        {
            public byte[] buffer = new byte[_bufferSize];
        }
    }
}


