using System.Windows.Input;
using HelloXFPrism.Models;
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
        private Item item;
        private bool isNewItem;

        public ItemDetailViewModel(INavigationService navigationService, IDataStore<Item> itemDataStore)
        {
            this.navigationService = navigationService;
            this.itemDataStore = itemDataStore;
        }

        public Item Item
        {
            get => this.item;
            set => this.SetProperty(ref this.item, value, nameof(this.Item));
        }

        public ICommand SaveCommand
        {
            get
            {
                return this.saveCommand ?? (this.saveCommand = new Command(async () =>
                {
                    if (this.isNewItem)
                    {
                        await this.itemDataStore.AddItemAsync(this.Item);
                    }
                    else
                    {
                        await this.itemDataStore.UpdateItemAsync(this.Item);
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
                this.Item = new Item();
                this.isNewItem = true;
            }
            else
            {
                this.Item = existingItem;
                this.isNewItem = false;
            }

        }
    }
}