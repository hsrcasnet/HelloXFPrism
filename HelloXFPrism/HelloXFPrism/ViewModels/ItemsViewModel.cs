using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using HelloXFPrism.Model;
using HelloXFPrism.Services;
using Prism.Navigation;
using Xamarin.Forms;

namespace HelloXFPrism.ViewModels
{
    public class ItemsViewModel : BaseViewModel, INavigatedAware
    {
        private readonly INavigationService navigationService;
        private readonly IDataStore<Item> itemDataStore;
        private Command loadItemsCommand;
        private Command addItemCommand;
        private Item selectedItem;
        private ObservableCollection<Item> items;

        public ItemsViewModel(
            INavigationService navigationService,
            IDataStore<Item> itemDataStore)
        {
            this.navigationService = navigationService;
            this.itemDataStore = itemDataStore;

            this.Title = "HelloXFPrism";
            this.Items = new ObservableCollection<Item>();
        }

        public ObservableCollection<Item> Items
        {
            get => this.items;
            set => this.SetProperty(ref this.items, value, nameof(this.Items));
        }

        public Item SelectedItem
        {
            get => this.selectedItem;
            set
            {
                if (this.SetProperty(ref this.selectedItem, value, nameof(this.SelectedItem)))
                {
                    if (value != null)
                    {
                        var navigationParams = new NavigationParameters { { "model", value } };
                        this.navigationService.NavigateAsync("ItemDetailPage", navigationParams);
                        this.selectedItem = null;
                    }
                }
            }
        }

        public ICommand LoadItemsCommand
        {
            get => this.loadItemsCommand ??= new Command(async () => await this.ExecuteLoadItemsCommand());
        }

        public ICommand AddItemCommand
        {
            get => this.addItemCommand ??= new Command(async () => { await this.navigationService.NavigateAsync("NewItemPage"); });
        }

        private async Task ExecuteLoadItemsCommand()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;

            try
            {
                // Load payload from backend
                var newItems = (await this.itemDataStore.GetItemsAsync(forceRefresh: true)).ToList();

                // Refresh list of items: Alternative 1
                this.Items.Clear();
                foreach (var item in newItems)
                {
                    this.Items.Add(item);
                }

                // Refresh list of items: Alternative 2
                // this.Items = new ObservableCollection<Item>(newItems);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            var navigationMode = parameters.GetNavigationMode();
            if (navigationMode == NavigationMode.New)
            {
                this.LoadItemsCommand.Execute(null);
            }
            else if (navigationMode == NavigationMode.Back)
            {
                var isNewItem = parameters["isNewItem"] as bool?;
                if (isNewItem.HasValue && isNewItem.Value)
                {
                    this.LoadItemsCommand.Execute(null);
                }
            }
        }
    }
}