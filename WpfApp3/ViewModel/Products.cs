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
    public class Products : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


        [Key]private int Products_id;
        private int Suppliers_id;
        private string Name;
        private decimal Price;


        [Key]
        public int products_id
        {
            get { return Products_id; }
            set
            {
                Products_id = value;
                OnPropertyChanged("products_id");
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

        public string name
        {
            get { return Name; }
            set
            {
                Name = value;
                OnPropertyChanged("name");
            }
        }

        public decimal price
        {
            get { return Price; }
            set
            {
                Price = value;
                OnPropertyChanged("price");
            }
        }
        [Required]
        public ICollection<Receipts> Receipts { get; set; }
        

    }
}
