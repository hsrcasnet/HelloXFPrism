namespace HelloXFPrism.ViewModels
{
    public class BaseViewModel : Prism.Mvvm.BindableBase
    {
        private bool isBusy = false;
        private string title = string.Empty;

        public bool IsBusy
        {
            get { return this.isBusy; }
            set { this.SetProperty(ref this.isBusy, value); }
        }

        public string Title
        {
            get => this.title;
            set => this.SetProperty(ref this.title, value);
        }
    }
}