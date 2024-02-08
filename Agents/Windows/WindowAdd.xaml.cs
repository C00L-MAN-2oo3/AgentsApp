using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Agents.Windows
{
    public partial class WindowAdd : Window
    {
        Data.Agents agents;

        public WindowAdd(Data.Agents selectedAgent)
        {
            InitializeComponent();

            if (selectedAgent != null)
            {
                DataContext = selectedAgent;
                agents = selectedAgent;
            }
            else
                DataContext = agents;

            ListType.ItemsSource = PleasantRustleEntities.GetContext().Agents.Select(type => type.Type).Distinct().ToList();
        }

        private async void AddWrite(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(agents.Type)) errors.AppendLine("Укажите тип!");
            if (string.IsNullOrWhiteSpace(agents.Name)) errors.AppendLine("Укажите название!");
            if (string.IsNullOrWhiteSpace(agents.Email)) errors.AppendLine("Укажите эл. почту!");
            if (string.IsNullOrWhiteSpace(agents.Phone)) errors.AppendLine("Укажите телефон!");
            if (string.IsNullOrWhiteSpace(agents.Address)) errors.AppendLine("Укажите адрес!");
            if (agents.Priority < 0) errors.AppendLine("Укажите приоритет!");
            if (string.IsNullOrWhiteSpace(agents.Director)) errors.AppendLine("Укажите ФИО директора!");
            if (string.IsNullOrWhiteSpace(agents.TaxNumber)) errors.AppendLine("Укажите ИНН!");
            if (string.IsNullOrWhiteSpace(agents.CodeRegistration)) errors.AppendLine("Укажите КПП!");
            if (Photo.Source == null) agents.Logo = "\\agents\\picture.png";

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (agents.ID == 0)
            {
                int maxID = PleasantRustleEntities.GetContext().Agents.Select(x => x.ID).ToList().Max();
                agents.ID = maxID + 1;

                PleasantRustleEntities.GetContext().Agents.Add(agents);
            }

            try
            {
                await PleasantRustleEntities.GetContext().SaveChangesAsync();
                MessageBox.Show("Информация сохранена!");
            }
            catch (Exception)
            {
                MessageBox.Show("Возникла ошибка!");
            }
        }

        private void SelectImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Изображение (*.png)|*.png";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            if (openFileDialog.ShowDialog() == true)
            {
                Photo.Source = new BitmapImage(new Uri(openFileDialog.FileName));

                string selectedImage = openFileDialog.FileName;
                string targetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

                Directory.CreateDirectory(targetPath);

                string newFileName = Path.Combine(targetPath, Path.GetFileName(selectedImage));

                File.Copy(selectedImage, newFileName);

                agents.Logo = newFileName;
            }
        }
    }
}
