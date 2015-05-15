using GalaSoft.MvvmLight.Command;

namespace DianeGUI.ViewModel
{
    public partial class MainViewModel
    {
        private RelayCommand _LoginCommand;

         

        public RelayCommand LoginCommand
        {
            get
            {
                return _LoginCommand
                       ?? (_LoginCommand = new RelayCommand(
                           () => ExecuteLoginCommand(),
                           () => CanExecuteLoginCommand()
                           ));
            }
        }

        private void ExecuteLoginCommand()
        {
            _ingressApi.Signup();
        }

        private bool CanExecuteLoginCommand()
        {
            // Replace this condition if needed
            return true;
        }

    }
}