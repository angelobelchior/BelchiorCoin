using System;
using System.Security.Cryptography;
using System.Text;
using ServiceStack.Text;

namespace Blockchain
{
    public class Block
    {
        public ulong Index { get; private set; }
        public DateTime Timestamp { get; private set; }
        public object Data { get; private set; }
        public string Hash { get; private set; }
        public ulong Nonce { get; private set; }

        public string PreviousHash { get; set; }

        public static Block Create(ulong index, object data, byte effort, Block previousBlock = null)
        {
            var newBlock = new Block(index, data);

            var previousHash = "";
            if (previousBlock != null)
                previousHash = previousBlock.Hash;

            newBlock.PreviousHash = previousHash;
            newBlock.Mine(effort);
            return newBlock;
        }

        internal static Block CreateGenesis(byte effort)
        {
            var genesis = new Genesis();
            var genesisBlock = Create(ulong.MinValue, genesis, effort);
            return genesisBlock;
        }

        private Block(ulong index, DateTime timestamp, object data, string previousHash = "")
        {
            this.Index = index;
            this.Timestamp = timestamp;
            this.Data = data ?? throw new ArgumentNullException(nameof(data));
            this.PreviousHash = previousHash ?? throw new ArgumentNullException(nameof(previousHash));
            this.Nonce = 0;
            this.Hash = this.ComputeHash();
        }

        private Block(ulong index, object data, string previousHash = "")
            : this(index, DateTime.UtcNow, data, previousHash)
        { }

        public string ComputeHash()
        {
            var dataJson = JsonSerializer.SerializeToString(this.Data);
            var content = $"{this.Index}{this.Timestamp}{this.PreviousHash}{this.Nonce}{dataJson}";
            var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(content));
            var hashString = "";
            foreach (byte x in hash)
                hashString += String.Format("{0:x2}", x);
            return hashString;
        }

        private Block Mine(byte effort)
        {
            if (effort == 0)
                this.Hash = this.ComputeHash();
            else
            {
                var zeros = new string('0', effort);
                while (!this.Hash.Substring(0, effort).Equals(zeros))
                {
                    this.Nonce++;
                    this.Hash = this.ComputeHash();
                }
            }

            return this;
        }

        public override string ToString()
            => this.Hash;
    }

    [Serializable]
    internal class Genesis
    {
        public string Data => "Genesis Block";
    }
}
