using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp2.Model.UserDto
{
    public class UserDto
    {

        public string Id { get; set; }

        private string userName;

        private string email;

        private string address;

        private string selfIntro;
        public string UserName
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string Address
        {
            get => address;
            set => SetProperty(ref address, value);
        }

        public string SelfIntro
        {
            get => selfIntro;
            set => SetProperty(ref selfIntro, value);
        }
        public string UserIconUrl { get; set; }


        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
