using System;
using System.Linq;
using System.Collections.Generic;

namespace Blockchain
{
    public class Chain
    {
        private List<Block> _blocks = new List<Block>();
        public IReadOnlyCollection<Block> Blocks => _blocks;

        private readonly byte _effort;

        public Chain(byte effort)
        {
            this._effort = effort;
            var genesis = Block.CreateGenesis(effort);
            this._blocks.Add(genesis);
        }

        public Block Add(object data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var previousBlock = this._blocks.LastOrDefault();
            var block = Block.Create((ulong)this._blocks.Count, data, this._effort, previousBlock);

            this._blocks.Add(block);
            return block;
        }

        public Block GenesisBlock => this._blocks.First();
        public Block LastBlock => this._blocks.Last();

        public bool CheckIntegrity()
        {
            //Ignore First Block
            for (int i = 1; i < this._blocks.Count; i++)
            {
                var current = this._blocks[i];
                var previous = this._blocks[i - 1];

                var computedCurrentHash = current.ComputeHash();
                if (!current.Hash.Equals(computedCurrentHash))
                    return false;

                if (!current.PreviousHash.Equals(previous.Hash))
                    return false;
            }

            return true;
        }
    }
}
