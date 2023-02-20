using System;
using Task.Orders;

namespace Task
{
    class WendingMachine
    {
        public int Balance { get; private set; }

        private Good[] _goods;

        public WendingMachine(int balance, params Good[] goods)
        {
            Balance = balance;
            _goods = goods;
        }

        public void AddBalance(int balanceToAdd)
        {
            if (balanceToAdd < 0)
                throw new ArgumentOutOfRangeException("Balance to add can not be less than 0");

            Balance += balanceToAdd;
        }

        public void DiscardBalance (int balanceToDiscard)
        {
            if (balanceToDiscard < 0 &&  Balance >= balanceToDiscard )
                throw new ArgumentOutOfRangeException("Wrong balanceToDiscard");

            Balance -= balanceToDiscard;
        }

        public bool Contains(int id) => id >= 0 && id < _goods.Length;

        public Good GetGoodById(int id)
        {
             if (!Contains(id))
                throw new ArgumentOutOfRangeException("Wrong id");

             return _goods[id];  
        }

        public void ProccessOrder(IOrder order)
        {
            TryProcessOrder(order, out bool success);

            if (!success)
                throw new ArgumentException("Order could not be proccessed");
        }

        public void TryProcessOrder(IOrder order, out bool succes)
        {
            succes = IsOrderPossible(order);

            if (!succes)
                return;

            DiscardBalance(order.GetCost());
            order.Process();
        }

        private bool IsOrderPossible(IOrder order)

        {
            return order.Available && order.GetCost() <= Balance;
        }

    }
}
