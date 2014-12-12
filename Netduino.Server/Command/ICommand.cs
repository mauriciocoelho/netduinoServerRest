namespace Netduino.Server.Command
{
    public interface ICommand
    {
        void Execute(int portCommand);
    }
}
