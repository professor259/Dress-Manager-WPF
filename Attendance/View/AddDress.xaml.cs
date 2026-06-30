using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Attendance.View
{
    /// <summary>
    /// Interaction logic for AddAdmin.xaml
    /// </summary>
    public partial class AddDress : UserControl
    {
        public Model.DressModel Model;
        public AddDress(Model.DressModel model)
        {
            InitializeComponent();
            if(model != null)
            {
                Model = model;
                EditMode = true;
                Name.Text = model.Name;
                Code.Text = model.ID;
                Code.IsEnabled = false;
                Color.Text = model.Color;
                SizeSelect.SelectedIndex = model.Size;
                CostPrice.Text = model.CostPrice;
                RentPrice.Text = model.RentPrice;
                AddBtn.Content = "Edit Dress";
            }
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+"); // allows only digits
            e.Handled = regex.IsMatch(e.Text);
        }
        public bool EditMode = false;
        public async Task AddDressMember()
        {
            await Task.Run(() =>
            {
                {
                    if (!EditMode)
                    {
                        if (Database.DB.VerifyFacultyName(CodeContext))
                        {

                            int ID =  Database.DB.AddDress(NameContext,SizeContext,ColorContext,CodeContext,CostPriceContext,RentPriceContext);

                            Model.DressModel dress = new Model.DressModel();
                            dress.Name = NameContext;
                            dress.ID = CodeContext;
                            dress.Color = ColorContext;
                            dress.Size =SizeContext;
                            dress.SizeS = SizeS;
                            dress.RentPrice = RentPriceContext;
                            dress.CostPrice = CostPriceContext;
                            if (dress.Name != string.Empty)
                                dress.Charater = dress.Name.Substring(0, 1);
                            Program.DressPool.Add(int.Parse(dress.ID), dress);

                            Program.MyApp.ShowBox("Dress has been added!");
                            Program.Application.DressManager.ShowPageRaws();
                            this.Dispatcher.Invoke(() =>
                            {
                                Window parentWindow = (Window)this.Parent;
                                parentWindow.Close();
                            });
                        }
                        else
                        {
                            Program.MyApp.ShowBox("Dress already exists!");
                            Reset();
                        }
                    }
                    else
                    {
                        //if (Database.DB.VerifyFacultyName(CodeContext))
                        {
                            Database.DB.EditDress(NameContext, SizeContext, ColorContext, CodeContext, CostPriceContext, RentPriceContext);
                            Database.DB.EditDressCollection(NameContext, SizeContext, ColorContext, CodeContext, CostPriceContext, RentPriceContext);
                            Program.MyApp.ShowBox("dress has been edited successfully!");
                            Program.Application.DressManager.ShowPageRaws();
                            this.Dispatcher.Invoke(() =>
                            {
                                Window parentWindow = (Window)this.Parent;
                                parentWindow.Close();
                            });
                        }
                       
                    }
                }
            });
        }

        public void Reset()
        {
            this.Dispatcher.Invoke(() =>
            {
                AddBtn.IsEnabled = true;
                if (!EditMode)
                    AddBtn.Content = "Add Admin";
                else AddBtn.Content = "Edit";
            });
        }
        public string Price;
        public string NameContext;
        public string CodeContext;
        public string ColorContext;
        public string SizeS;
        public int SizeContext;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Name.Text == string.Empty)
            {
                Program.MyApp.ShowBox("Please enter dress name first!");
                return;
            }
            if (SizeSelect.SelectionBoxItem == "")
            {
                Program.MyApp.ShowBox("Please select dress size first!");
                return;
            }
            if (Color.Text == string.Empty)
            {
                Program.MyApp.ShowBox("Please enter Color first!");
                return;
            }
            if (Code.Text == string.Empty)
            {
                Program.MyApp.ShowBox("Please enter dress code first!");
                return;
            }
            if (RentPrice.Text == string.Empty)
            {
                Program.MyApp.ShowBox("Please enter rent price first!");
                return;
            }
            if (CostPrice.Text == string.Empty)
            {
                Program.MyApp.ShowBox("Please enter cost price first!");
                return;
            }
            AddBtn.IsEnabled = false;
            AddBtn.Content = "Please Wait...";        
            NameContext = Name.Text;
            ColorContext = Color.Text;
            SizeContext = SizeSelect.SelectedIndex;
            SizeS = SizeSelect.Text;
            CodeContext = Code.Text;
            RentPriceContext = RentPrice.Text;
            CostPriceContext = CostPrice.Text;
            AddDressMember();
        }
        public string RentPriceContext;
        public string CostPriceContext;
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = (Window)this.Parent;
            parentWindow.Close();
        }

    }
}
