using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BikeShop
{
    class Human
    {
        public string FirstName { get; set; }
        public bool HasDrivingLicense { get; set; }
    }
    class Car : Notifier
    {
        private double speed;
        public double Speed { 
            get { return speed; }
            set
            { 
                speed = value;
                OnPropertyChanged("Speed"); //  속성값이 변경되는 것을 클라이언트에 통보해줌
            }
        }
        public Color Color { get; set; }
        public Human Driver { get; set; }
    }
}
