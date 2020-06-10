using System.Windows.Input;
using HelloXFPrism.Model;
using HelloXFPrism.Services;
using Prism.Navigation;
using Xamarin.Forms;

namespace HelloXFPrism.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel, INavigatedAware
    {
        private readonly INavigationService navigationService;
        private readonly IDataStore<Item> itemDataStore;
        private Command saveCommand;
        private bool isNewItem;
        private string id;
        private string text;
        private decimal price;
        private string description;

        public ItemDetailViewModel(INavigationService navigationService, IDataStore<Item> itemDataStore)
        {
            this.navigationService = navigationService;
            this.itemDataStore = itemDataStore;
        }

        public string Id
        {
            get => this.id;
            private set => this.SetProperty(ref this.id, value, nameof(this.Id));
        }

        public string Text
        {
            get => this.text;
            set
            {
                if (this.SetProperty(ref this.text, value, nameof(this.Text)))
                {
                    this.RaisePropertyChanged(nameof(this.Summary));
                }
            }
        }

        public decimal Price
        {
            get => this.price;
            set
            {
                if (this.SetProperty(ref this.price, value, nameof(this.Price)))
                {
                    this.RaisePropertyChanged(nameof(this.Summary));
                }
            }
        }

        public string Description
        {
            get => this.description;
            set => this.SetProperty(ref this.description, value, nameof(this.Description));
        }

        // DEMO: Cascading updates: If Text or Price property changes, Summary needs to be updated too.
        public string Summary
        {
            get { return $"Summary: {this.Text} @ CHF {this.Price:0.00}"; }
        }

        public ICommand SaveCommand
        {
            get
            {
                return this.saveCommand ?? (this.saveCommand = new Command(async () =>
                {
                    var item = new Item
                    {
                        Id = this.Id,
                        Text = this.Text,
                        Price = this.Price,
                        Description = this.Description,
                    };

                    if (this.isNewItem)
                    {
                        await this.itemDataStore.AddItemAsync(item);
                    }
                    else
                    {
                        await this.itemDataStore.UpdateItemAsync(item);
                    }

                    var navigationParameters = new NavigationParameters { { "isNewItem", this.isNewItem } };
                    await this.navigationService.GoBackAsync(navigationParameters);
                }));
            }
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            var existingItem = parameters["model"] as Item;
            if (existingItem == null)
            {
                this.isNewItem = true;
            }
            else
            {
                this.Id = existingItem.Id;
                this.Text = existingItem.Text;
                this.Price = existingItem.Price;
                this.Description = existingItem.Description;

                this.isNewItem = false;
            }
        }
    }
}