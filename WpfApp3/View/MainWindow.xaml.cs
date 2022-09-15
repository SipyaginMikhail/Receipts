using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp3.ViewModel;

namespace WpfApp3
{
   
    public partial class MainWindow : Window
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            var db = new ApplicationContext();
            Receipts_dg.ItemsSource = db.Receipts.ToList();
            Products = new ObservableCollection<Products>(db.Products.ToList());
            Suppliers = new ObservableCollection<Suppliers>(db.Suppliers.ToList());
            Receipts = new ObservableCollection<Receipts>(db.Receipts.ToList());
            products_cb.IsEnabled = false;
            weight_tb.IsEnabled = false;
            save_bt.IsEnabled = false;


        }
        private ObservableCollection<Products> _products;
        private ObservableCollection<Suppliers> _suppliers;
        private ObservableCollection<Receipts> _receipts;

        public ObservableCollection<Receipts> Receipts
        {
            get { return _receipts; }
            set
            {
                _receipts = value;
                OnPropertyChanged("Receipts");
            }
        }
        public ObservableCollection<Products> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                OnPropertyChanged("Products");
            }
        }
        public ObservableCollection<Suppliers> Suppliers
        {
            get { return _suppliers; }
            set
            {
                _suppliers = value;
                OnPropertyChanged("Suppliers");
            }
        }

        private void Suppliers_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            products_cb.IsEnabled = true;
           
                var db = new ApplicationContext();
            if (suppliers_cb.SelectedValue == null) { }
            else
            {
                try
                {
                    int sup_id = (int)suppliers_cb.SelectedValue;
                    products_cb.ItemsSource = (from c in db.Products where c.suppliers_id == sup_id select c).ToList();
                }
                catch
                {

                }
            }
        }

        decimal price_gb;
        private void weight_tb_TextChanged(object sender, TextChangedEventArgs e)
        {

          
            if (weight_tb.Text == "" || weight_tb.Text == "0" || weight_tb.Text[0].ToString() == "0") { sum_lb.Content = "Итоговая стоимость: "; weight_tb.Text = ""; }
            else
            {
                try
                {
                    var db = new ApplicationContext();
                    int prod_id = (int)products_cb.SelectedValue;
                  
                   
                        decimal weight = int.Parse(weight_tb.Text);
                        Products price = (from c in db.Products where c.products_id == prod_id select c).SingleOrDefault();
                        price_gb = weight * price.price;
                        sum_lb.Content = "Итоговая стоимость: " + price_gb.ToString() + " руб.";
                  
                }
                catch 
                {
                    MessageBox.Show($"Слишком большое значение поля 'Вес' ", " Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

        }
        private void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0) || (e.Text == ".")
               && (!weight_tb.Text.Contains(".")
               && weight_tb.Text.Length != 0)))
            {
                e.Handled = true;
            }
        }

        private void add_receipts(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(suppliers_cb.Text) || string.IsNullOrWhiteSpace(products_cb.Text) ||
                string.IsNullOrWhiteSpace(weight_tb.Text)) { MessageBox.Show($"Необходимо ввести все данные.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Error); }
            else 
            {
                var db = new ApplicationContext();

                Receipts receipts = new Receipts();
                receipts.suppliers_id = (int)suppliers_cb.SelectedValue;
                receipts.products_id = (int)products_cb.SelectedValue;
                receipts.sum = price_gb;
                receipts.date = DateTime.Today;
                receipts.weight = int.Parse(weight_tb.Text);

                db.Receipts.Add(receipts);
                db.SaveChanges();
                MessageBox.Show($"Поставка добавлена.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                Receipts_dg.ItemsSource = db.Receipts.ToList();
                suppliers_cb.Text = null;
                products_cb.Text = null;
                weight_tb.Text = "";
                products_cb.IsEnabled = false;
                weight_tb.IsEnabled = false;



            }

        }

        private void products_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            weight_tb.IsEnabled = true;
            weight_tb.Text = "";
         
        }

       

        private void Sorting_Receipts(object sender, RoutedEventArgs e)
        {
            if (first_date.SelectedDate == null || second_date.SelectedDate == null)
            {
                MessageBox.Show($"Необходимо выбрать период.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (first_date.SelectedDate > second_date.SelectedDate)
            {
                MessageBox.Show($"Первая дата периода не может быть меньше второй!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                first_date.SelectedDate = null;
            }
            else 
            {
                var db = new ApplicationContext();

                DateTime _first = (DateTime)first_date.SelectedDate;
                DateTime _second = (DateTime)second_date.SelectedDate;
                var result = (from Receipts in db.Receipts where Receipts.date >= _first && Receipts.date <= _second select Receipts).ToList();
                if (result.Count > 0)
                {
                    Receipts_dg.ItemsSource = result.ToList();

                    decimal sum = db.Receipts.Where(u => u.date >= _first && u.date <= _second)
                       .Sum(u => u.sum);

                    decimal weight = db.Receipts.Where(u => u.date >= _first && u.date <= _second)
                      .Sum(u => u.weight);

                    result_sum.Content = "Общая стоимость за период: " + sum.ToString() + " руб.";
                    result_weight.Content = "Общий вес за период: " + weight.ToString() + " кг.";




                }
                else
                {
                    MessageBox.Show($"За выбранный период нет поставок", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                }



            }
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Сбросить данные?", " Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var db = new ApplicationContext();
                Receipts_dg.ItemsSource = db.Receipts.ToList();
                Products = new ObservableCollection<Products>(db.Products.ToList());
                Suppliers = new ObservableCollection<Suppliers>(db.Suppliers.ToList());
                Receipts = new ObservableCollection<Receipts>(db.Receipts.ToList());
                suppliers_cb.Text = null;
                products_cb.Text = null;
                weight_tb.Text = "";
                result_sum.Content = "Общая стоимость за период:";
                result_weight.Content = "Общий вес за период:";
                products_cb.IsEnabled = false;
                weight_tb.IsEnabled = false;
                first_date.SelectedDate = null;
                second_date.SelectedDate = null;
            }


        }

        private int receipts_id_g;
        private int suppliers_id_g;
        private int products_id_g;
        

        private void Upd_receipts(object sender, RoutedEventArgs e)
        {
            var db = new ApplicationContext();

            if (MessageBox.Show($"Внести изменения?", " Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    save_bt.IsEnabled = true;

                    int receipts_id = (Receipts_dg.SelectedItem as Receipts).receipts_id;
                    int suppliers_id = (int)(Receipts_dg.SelectedItem as Receipts).suppliers_id;
                    int products_id = (int)(Receipts_dg.SelectedItem as Receipts).products_id;
                   
                    Receipts rec = (from o in db.Receipts where o.receipts_id == receipts_id select o).SingleOrDefault();
                    Suppliers sup = (from c in db.Suppliers where c.suppliers_id == suppliers_id select c).SingleOrDefault();
                    Products prod = (from p in db.Products where p.products_id == products_id select p).SingleOrDefault();

                    receipts_id_g = receipts_id;
                    suppliers_id_g = suppliers_id;
                    products_id_g = products_id;
                    

                    suppliers_cb.Text = sup.name.ToString();
                    products_cb.Text = prod.name.ToString();
                    weight_tb.Text = rec.weight.ToString();

                    add_receipts_bt.IsEnabled = false;
                    save_bt.IsEnabled = true;
                }
                catch

                {
                    MessageBox.Show("Ошибка!");
                }




            }

        }

        private void save_receipts(object sender, RoutedEventArgs e)
        {
            var db = new ApplicationContext();

            Receipts rec = (from o in db.Receipts where o.receipts_id == receipts_id_g select o).SingleOrDefault();
            Suppliers sup = (from c in db.Suppliers where c.suppliers_id == suppliers_id_g select c).SingleOrDefault();
            Products prod = (from p in db.Products where p.products_id == products_id_g select p).SingleOrDefault();
           


            if (MessageBox.Show($"Сохранить изменения?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {

                    rec.suppliers_id = (int)suppliers_cb.SelectedValue;
                    rec.products_id = (int)products_cb.SelectedValue;
                    rec.sum = price_gb;
                    rec.weight = int.Parse(weight_tb.Text);

                    
                    add_receipts_bt.IsEnabled = true;
                }
                catch
                {
                    MessageBox.Show("Ошибка!");
                }
                try
                {

                    db.SaveChanges();
                    Products = new ObservableCollection<Products>(db.Products.ToList());
                    Suppliers = new ObservableCollection<Suppliers>(db.Suppliers.ToList());
                    Receipts = new ObservableCollection<Receipts>(db.Receipts.ToList());
                    Receipts_dg.ItemsSource = db.Receipts.ToList();
                    MessageBox.Show($"Поставка под номером - '{rec.receipts_id}' была изменена.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                    suppliers_cb.Text = null;
                    products_cb.Text = null;
                    weight_tb.Text = "";
                    products_cb.IsEnabled = false;
                    weight_tb.IsEnabled = false;
                    save_bt.IsEnabled=false;

                }
                catch
                {
                    MessageBox.Show("Ошибка!");
                }

            }
        }
    }
}
