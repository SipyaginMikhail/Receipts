using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3.ViewModel
{
    public class Receipts : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


        [Key] private int Receipts_id;
        private int Suppliers_id;
        private int Products_id;
        private DateTime Date;
        private decimal Sum;
        private decimal Weight;

        [Key]
        public int receipts_id
        {
            get { return Receipts_id; }
            set
            {
                Receipts_id = value;
                OnPropertyChanged("receipts_id");
            }
        }

        public int suppliers_id
        {
            get { return Suppliers_id; }
            set
            {
                Suppliers_id = value;
                OnPropertyChanged("suppliers_id");
            }
        }
      

        public int products_id
        {
            get { return Products_id; }
            set
            {
                Products_id = value;
                OnPropertyChanged("products_id");
            }
        }

        public DateTime  date
        {
            get { return Date; }
            set
            {
                Date = value;
                OnPropertyChanged("date");
            }
        }
        public decimal sum
        {
            get { return Sum; }
            set
            {
                Sum = value;
                OnPropertyChanged("sum");
            }
        }
        public decimal weight
        {
            get { return Weight; }
            set
            {
                Weight = value;
                OnPropertyChanged("weight");
            }
        }
      
        public virtual Suppliers Suppliers { get; set; }
       
        public virtual Products Products { get; set; }
    }
}
