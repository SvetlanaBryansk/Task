using System;
using Task.Commands;
using Task.Routing.States;

namespace Task.Routing
{
    class Router
    {
        public WendingMachine Machine { get; }

        private RouterState _state;

        public Router(WendingMachine machine)
        {
            Machine = machine;
            _state = new CustomerState(this);
        }

        public  ICommand CreateCommand(Request request)
        {
            switch (request.Command)
            {
                case nameof(AddMoney):
                    if (request.IsIncorrectParametersCount(1)) return null;
                    return new AddMoney(Machine, request.Parameters[0]);

                case nameof(BuyGood):
                    if (request.IsIncorrectParametersCount(2)) return null;
                    return new BuyGood(Machine, _state.MakeOrder(request));

                case nameof(GetChange):
                    if (request.IsIncorrectParametersCount(0)) return null;
                    return new GetChange(Machine);

                case nameof(Commands.Login):
                    if (request.IsIncorrectParametersCount(0)) return null;
                    return new Login(this);

                case nameof(Commands.Logout):
                    if (request.IsIncorrectParametersCount(0)) return null;
                    return new Logout(this);

                case nameof(ShowCommands):
                    if (request.IsIncorrectParametersCount(0)) return null;
                    return new ShowCommands(nameof(AddMoney), nameof(BuyGood),
                        nameof(GetChange), nameof(Commands.Login), nameof(Commands.Logout));


                default:
                    return null;
            }
        }
        
        public void Login() => _state =new AdminState(this);

        public void Logout() => _state =new CustomerState(this);
    }
}
