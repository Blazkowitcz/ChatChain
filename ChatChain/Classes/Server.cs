using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace ChatChain.Classes
{
    public class Server: WebSocketBehavior
    {
        bool chainSynched = false;
        WebSocketServer webSocketServer = null;
        MainWindow mainWindow;

        public void Start(int port)
        {
            webSocketServer = new WebSocketServer($"ws://127.0.0.1:12000");
            webSocketServer.AddWebSocketService<Server>("/Chat");
            webSocketServer.Start();
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if(e.Data == "Hi Server")
            {
                Send("Hi Client");
            }
            else
            {
                Blockchain newChain = JsonConvert.DeserializeObject<Blockchain>(e.Data);
                if (newChain.IsValid() && newChain.Chain.Count > mainWindow.blockchain.Chain.Count)
                {
                    List<Message> newTransaction = new List<Message>();
                    newTransaction.AddRange(newChain.PendingMessage);
                    newTransaction.AddRange(mainWindow.blockchain.PendingMessage);
                    newChain.PendingMessage = newTransaction;
                    mainWindow.blockchain = newChain;
                }
                if (!chainSynched)
                {
                    Console.WriteLine(this.GetHashCode());
                    Send(JsonConvert.SerializeObject(mainWindow.blockchain));
                    chainSynched = true;
                }
            }
        }
    }
}
