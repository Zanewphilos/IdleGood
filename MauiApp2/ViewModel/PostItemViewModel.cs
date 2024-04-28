using System.Windows.Input;
using MauiApp2.Model.ItemDto;
using MauiApp2.Services;
using Microsoft.Maui.Controls; // 更新使用正确的命名空间
using System.Threading.Tasks; // 确保引入了Task相关的命名空间
using Microsoft.Maui.Storage;
using System.Diagnostics;
using System.Collections.ObjectModel;


namespace MauiApp2.ViewModel
{
    public class PostItemViewModel : BindableObject
    {
        private readonly ApiService_Item _apiService = new ApiService_Item();
        
        private string title;
        private string description;
        private float price;

        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged();
            }
        }

        public float Price
        {
            get => price;
            set
            {
                price = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> MediaUrls { get; set; } = new ObservableCollection<string>();
        // ICommand在Microsoft.Maui.Controls命名空间下是可用的
        public ICommand PostItemCommand { get; }
        public ICommand UploadCommand { get; private set; }

        public PostItemViewModel()
        {
            PostItemCommand = new Command(async () => await PostItemAsync());
            UploadCommand = new Command(async () => await UploadAsync());
        }

        private async Task PostItemAsync()
        {
            // create the item DTO
            var itemCreateDto = new ItemCreateDto
            {
                Title = this.Title,
                Description = this.Description,
                Price = this.Price,
                ImageUrl = MediaUrls.Where(url => url.EndsWith(".jpg") || url.EndsWith(".png")).ToList(),
                VideoUrl = MediaUrls.Where(url => url.EndsWith(".mp4")).ToList(),
                ReleaseDate = DateTime.UtcNow 
            };

            // make the API call to post the item
            var isSuccess = await _apiService.PostItemForSaleAsync(itemCreateDto);
            if (isSuccess)
            {
               
                await Application.Current.MainPage.DisplayAlert("Success", "Item posted successfully", "OK");
                ClearInputs();
            }
            else
            {
                
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to post the item", "OK");
            }
        }

        private async Task UploadAsync()
        {
            try
            {
                var pickResult = await PickMediaAsync();
                if (pickResult != null)
                {
                    //get the stream of the file
                    using var stream = await pickResult.OpenReadAsync();

                    // according to the file type, upload the file to the server
                    var fileType = pickResult.FileName.EndsWith(".mp4") ? "video" : "image";
                    var fileUrl = await _apiService.UploadFileAsync(stream, pickResult.FileName, fileType);

                    if (!string.IsNullOrEmpty(fileUrl))
                    {
                        MediaUrls.Add(fileUrl);
                    }
                }
            }
            catch (Exception ex)
            {
                //deal with the exception
                Debug.WriteLine(ex.Message);
            }
        }
        private async Task<FileResult> PickMediaAsync()
        {
            // define the file type
            var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.iOS, new[] { "public.image", "public.movie" } },
                { DevicePlatform.Android, new[] { "image/*", "video/*" } },
            });

            var pickOptions = new PickOptions
            {
                PickerTitle = "Please select an image or video",
                FileTypes = customFileType,
            };

            // choose the file
            return await FilePicker.PickAsync(pickOptions);
        }

        private void ClearInputs()
        {
            Title = string.Empty;
            Description = string.Empty;
            Price = 0; 
            MediaUrls.Clear(); 
        }
    }


}
