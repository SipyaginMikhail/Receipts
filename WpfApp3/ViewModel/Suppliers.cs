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
    public class Suppliers : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    
   [Key] private int Suppliers_id;
        private string Name;
        private string Phone;


    [Key]
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

        public string phone
        {
            get { return Phone; }
            set
            {
                Phone = value;
                OnPropertyChanged("phone");
            }
        }

        [Required]
        public ICollection<Receipts> Receipts { get; set; }
       
      

    }
}
