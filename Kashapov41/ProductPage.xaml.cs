using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Kashapov41.Images
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        public ProductPage(User user)
        {
            InitializeComponent();
            if (user != null)
            {
                UserTB.Text = "Вы авторизованы как " + user.UserSurname + " " + user.UserName + " " + user.UserPatronymic;
                RoleTB.Text = "Роль: " + Kashapov41Entities.GetContext().Role.Find(user.UserRole).RoleName.ToString();
            }
            else
            {
                UserTB.Text = "Вы авторизованы как Гость";
                RoleTB.Text = "Роль: Гость";
            }
            var currentProducts = Kashapov41Entities.GetContext().Product.ToList();
            ProductListView.ItemsSource = currentProducts;
            DiscountCombo.SelectedIndex = 0;
            UpdateProducts();
        }

        private void UpdateProducts()
        {
            var currentProducts = Kashapov41Entities.GetContext().Product.ToList();
            if (DiscountCombo.SelectedIndex == 0) 
            {
                currentProducts = currentProducts.Where(p => (p.ProductDiscountAmount >= 0 && p.ProductDiscountAmount <= 100)).ToList();
            }
            if (DiscountCombo.SelectedIndex == 1)
            {
                currentProducts = currentProducts.Where(p => (p.ProductDiscountAmount >= 0 && p.ProductDiscountAmount < 10)).ToList();
            }
            if (DiscountCombo.SelectedIndex == 2)
            {
                currentProducts = currentProducts.Where(p => (p.ProductDiscountAmount >= 10 && p.ProductDiscountAmount < 15)).ToList();
            }
            if (DiscountCombo.SelectedIndex == 3)
            {
                currentProducts = currentProducts.Where(p => (p.ProductDiscountAmount >= 15 && p.ProductDiscountAmount <= 100)).ToList();
            }
            currentProducts = currentProducts.Where(p => p.ProductName.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
            if (RBtn_Asc.IsChecked.Value)
            {
                currentProducts = currentProducts.OrderBy(p => p.ProductCost).ToList();
            }
            if (RBtn_Desc.IsChecked.Value)
            {
                currentProducts = currentProducts.OrderByDescending(p => p.ProductCost).ToList();
            }
            ProductListView.ItemsSource = currentProducts;
            amountCounter.Text = "Кол-во " + currentProducts.Count().ToString() + " из " + Kashapov41Entities.GetContext().Product.ToList().Count().ToString();
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void RBtn_Asc_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProducts();
        }

        private void RBtn_Desc_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProducts();
        }

        private void DiscountCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProducts();
        }
    }
}
