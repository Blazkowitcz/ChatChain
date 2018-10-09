using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace ChatChain.Classes
{
    public class Client
    {
        IDictionary<string, WebSocket> webSocketDictionnary = new Dictionary<string, WebSocket>();
        public MainWindow mainWindow;
        public void Connect(string url)
        {
            if (!webSocketDictionnary.ContainsKey("ws://127.0.0.1:12000"))
            {
                url = "ws://127.0.0.1:12000";
                WebSocket webSocket = new WebSocket(url);
                webSocket.OnMessage += (sender, e) =>
                {
                    if(e.Data == "Hi Client")
                    {

                    }
                    else
                    {
                        Blockchain newChain = JsonConvert.DeserializeObject<Blockchain>(e.Data);
                        if(newChain.Chain.Count > mainWindow.blockchain.Chain.Count)
                        {
                            List<Message> newMessages = new List<Message>();
                            newMessages.AddRange(newChain.PendingMessage);
                            newMessages.AddRange(mainWindow.blockchain.PendingMessage);
                            newChain.PendingMessage = newMessages;
                            mainWindow.blockchain = newChain;
                        }
                    }
                };
                webSocket.Connect();
                webSocket.Send("Hi Server");
                webSocket.Send(JsonConvert.SerializeObject(mainWindow.blockchain));
                webSocketDictionnary.Add(url, webSocket);
            }
        }
    }
}
