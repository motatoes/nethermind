using System.Numerics;
using Nevermind.Core;
using Nevermind.Core.Encoding;

namespace Nevermind.Blockchain
{
    public interface IBlockchainProcessor
    {
        Block HeadBlock { get; }
        BigInteger TotalDifficulty { get; }
        void Process(Rlp blockRlp); // TODO: potentially do not return anything
    }
}