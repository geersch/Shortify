using System;
using CGeers.Bitly;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;

namespace Shortify.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private string _url;
        private string _error;
        private bool _urlFocused;
        private bool _isLoading;

        public MainViewModel()
        {
            ShortenCommand = new RelayCommand(() =>
            {
                try
                {
                    IsLoading = true;
                    Shorten();
                }
                catch(Exception)
                {
                    IsLoading = false;
                    throw;
                }
            });
        }

        public string Url
        {
            get
            {
                return _url;
            }

            set
            {
                if (_url != value)
                {
                    _url = value;
                    RaisePropertyChanged("Url");
                }
            }
        }

        public string Error
        {
            get
            {
                return _error;
            }

            set
            {
                if (_error != value)
                {
                    _error = value;
                    RaisePropertyChanged("Error");
                }
            }
        }

        public bool UrlFocused
        {
            get
            {
                return _urlFocused;
            }

            set
            {
                if (_urlFocused != value)
                {
                    _urlFocused = value;
                    RaisePropertyChanged("UrlFocused");
                }
            }
        }

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }

            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    RaisePropertyChanged("IsLoading");
                }
            }
        }

        public RelayCommand ShortenCommand { get; private set; }

        public void Shorten()
        {
            Error = string.Empty;
            UrlFocused = false;

            new Bitly().Shorten(Url, BitlyShortenCallback);
        }

        private void BitlyShortenCallback(BitlyResponse bitlyUrl)
        {
            try
            {
                Action updateUi;
                if (bitlyUrl.StatusCode == 200)
                {
                    updateUi = () => { Url = bitlyUrl.Data.Url; };
                }
                else
                {
                    updateUi = () => { Error = DescriptionAttribute.GetDescription(bitlyUrl.Error); };
                }

                DispatcherHelper.CheckBeginInvokeOnUI(updateUi);
            }
            finally
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() => { IsLoading = false; UrlFocused = true; });
            }
        }
    }
}