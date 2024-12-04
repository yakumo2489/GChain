using GChain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GChain.Infrastructure
{
    internal class ChainStore :
        IChainStore,
        IInsertableChainStore<BgrMatrix>,
        IInsertableChainStore<GrayMatrix>,
        IAppendableChainStore<BgrMatrix>, IAppendedChainStore<BgrMatrix, GrayMatrix>,
        IInsertableChainStore<BinaryMatrix>,
        IAppendableChainStore<GrayMatrix>, IAppendedChainStore<GrayMatrix, BinaryMatrix>,
        IInsertableChainStore<Points>,
        IAppendableChainStore<BinaryMatrix>, IAppendedChainStore<BinaryMatrix, Points>,
        IAppendableChainStore<Points>, IAppendedChainStore<Points, Points>,
        IAppendedChainStore<BgrMatrix, BgrMatrix>
    {
        private object? headChain;
        private object? tailChain;

        private IInsertable<T>? GetInsertable<T>()
        {
            return headChain is IInsertable<T> head
                ? head
                : null;
        }

        private IChain<T>? GetChain<T>()
        {
            return tailChain is IChain<T> tail
                ? tail
                : null;
        }

        private void Insert<T>(IChain<T> inserted)
        {
            if (headChain == null && tailChain == null)
            {
                headChain = inserted;
                tailChain = inserted;
                return;
            }

            headChain = inserted;
        }

        private void Append<TIn, TOut>(Chain<TIn, TOut> inserted)
            where TIn : Value
            where TOut : Value
        {
            if (headChain == null && tailChain == null)
            {
                headChain = inserted;
                tailChain = inserted;
                return;
            }

            tailChain = inserted;
        }

        void IInsertableChainStore<BgrMatrix>.Commit(IChain<BgrMatrix> inserted)
        {
            Insert(inserted);
        }

        IInsertable<BgrMatrix>? IInsertableChainStore<BgrMatrix>.GetInsertable()
        {
            return GetInsertable<BgrMatrix>();
        }

        void IInsertableChainStore<GrayMatrix>.Commit(IChain<GrayMatrix> inserted)
        {
            Insert(inserted);
        }

        IInsertable<GrayMatrix>? IInsertableChainStore<GrayMatrix>.GetInsertable()
        {
            return GetInsertable<GrayMatrix>();
        }

        IChain<BgrMatrix>? IAppendableChainStore<BgrMatrix>.GetAppendable()
        {
            return GetChain<BgrMatrix>();
        }

        void IAppendedChainStore<BgrMatrix, GrayMatrix>.Commit(Chain<BgrMatrix, GrayMatrix> appended)
        {
            Append(appended);
        }

        IInsertable<BinaryMatrix>? IInsertableChainStore<BinaryMatrix>.GetInsertable()
        {
            return GetInsertable<BinaryMatrix>();
        }

        void IInsertableChainStore<BinaryMatrix>.Commit(IChain<BinaryMatrix> inserted)
        {
            Insert(inserted);
        }

        IChain<GrayMatrix>? IAppendableChainStore<GrayMatrix>.GetAppendable()
        {
            return GetChain<GrayMatrix>();
        }

        void IAppendedChainStore<GrayMatrix, BinaryMatrix>.Commit(Chain<GrayMatrix, BinaryMatrix> appended)
        {
            Append(appended);
        }

        IInsertable<Points>? IInsertableChainStore<Points>.GetInsertable()
        {
            return GetInsertable<Points>();
        }

        void IInsertableChainStore<Points>.Commit(IChain<Points> inserted)
        {
            Insert(inserted);
        }

        IChain<BinaryMatrix>? IAppendableChainStore<BinaryMatrix>.GetAppendable()
        {
            return GetChain<BinaryMatrix>();
        }

        void IAppendedChainStore<BinaryMatrix, Points>.Commit(Chain<BinaryMatrix, Points> appended)
        {
            Append(appended);
        }

        IChain<Points>? IAppendableChainStore<Points>.GetAppendable()
        {
            return GetChain<Points>();
        }

        void IAppendedChainStore<Points, Points>.Commit(Chain<Points, Points> appended)
        {
            Append(appended);
        }

        void IAppendedChainStore<BgrMatrix, BgrMatrix>.Commit(Chain<BgrMatrix, BgrMatrix> appended)
        {
            Append(appended);
        }

        public bool IsInitialized()
        {
            return headChain != null && tailChain != null;
        }

        public IChain? PullTailChain()
        {
            return tailChain is IChain chain
                ? chain
                : null;
        }
    }
}
