using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ChatChain.Classes
{
    public class Block
    {
        public int Index { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public IList<Message> Messages { get; set; }
        public int Nonce { get; set; }

        public Block(DateTime timeStamp, string previousHash, IList<Message> messages)
        {
            Index = 0;
            TimeStamp = timeStamp;
            Messages = messages;
            Hash = CalculateHash();
        }
        
        public string CalculateHash()
        {
            SHA256 sha256 = SHA256.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash ?? ""}-{JsonConvert.SerializeObject(Messages)}-{Nonce}");
            byte[] outputBytes = sha256.ComputeHash(inputBytes);
            return Convert.ToBase64String(outputBytes);
        }

        public void Mine(int difficulty)
        {
            var leadingZero = new string('0', difficulty);
            while (Hash == null || Hash.Substring(0, difficulty) != leadingZero)
            {
                Nonce++;
                Hash = CalculateHash();
            }
        }
    }
}
