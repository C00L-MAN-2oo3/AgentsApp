using System.Collections.Generic;
using System.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using Agents.Windows;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;

namespace Agents
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Table.ItemsSource = PleasantRustleEntities.GetContext().Agents.ToList();
            var filter = PleasantRustleEntities.GetContext().Agents.Select(type => type.Type).Distinct().ToList();

            filter.Insert(0, "Все типы");
            ListFilter.ItemsSource = filter;
        }

        private void ClearSearchText(object sender, RoutedEventArgs e)
        {
            SearchText.Clear();
            SearchText.Focus();
        }

        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchText.Text))
                Placeholder.Visibility = Visibility.Visible;
            else
                Placeholder.Visibility = Visibility.Hidden;

            Operations();
        }

        private void ListSortSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListSort.SelectedItem != null)
                TextSort.Visibility = Visibility.Hidden;

            Operations();
        }

        private void ListFilterSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListFilter.SelectedItem != null)
                TextFilter.Visibility = Visibility.Hidden;

            Operations();
        }

        private void Operations()
        {
            List<Data.Agents> date = PleasantRustleEntities.GetContext().Agents.ToList();

            try
            {
                if (ListFilter.SelectedIndex > 0)
                {
                    string filter = ListFilter.SelectedItem.ToString();
                    date = date.Where(type => type.Type == filter).ToList();
                }
                else
                    date = PleasantRustleEntities.GetContext().Agents.ToList();

                switch (ListSort.SelectedIndex)
                {
                    case 0:
                        date = date.OrderBy(n => n.Name).ToList();
                        break;
                    case 1:
                        date = date.OrderBy(n => n.Priority).ToList();
                        break;
                    case 2:
                        date = date.OrderBy(n => n.Discount).ToList();
                        break;
                    default:
                        break;
                }

                if (SearchText.Text != String.Empty)
                    date = date.Where(info => info.Name == SearchText.Text
                                      || info.Name.Contains(SearchText.Text)
                                      || info.Type == SearchText.Text
                                      || info.Type.Contains(SearchText.Text)
                                      || info.Phone == SearchText.Text
                                      || info.Phone.Contains(SearchText.Text)).ToList();

                Table.ItemsSource = date.ToList();
            }
            catch (Exception)
            {
                MessageBox.Show("Возникла ошибка!");
            }
        }

        private async void DeleteWrite(object sender, RoutedEventArgs e)
        {
            var delete = Table.SelectedItems.Cast<Data.Agents>().ToList();

            if (Table.SelectedItem != null)
                if (MessageBox.Show($"Вы уверены, что хотите удалить записи: {Table.SelectedItems.Count}?", "Внимание!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        PleasantRustleEntities.GetContext().Agents.RemoveRange(delete);
                        await PleasantRustleEntities.GetContext().SaveChangesAsync();

                        MessageBox.Show("Операция выполнена!", "Внимание!", MessageBoxButton.OK);

                        Table.ItemsSource = PleasantRustleEntities.GetContext().Agents.ToList();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Возникла ошибка!");
                    }
                }
        }

        private void AddWrite(object sender, RoutedEventArgs e)
        {
            try
            {
                WindowAdd addWrite = new WindowAdd(null);
                addWrite.ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("Возникла ошибка!");
            }
        }

        private void EditWrite(object sender, RoutedEventArgs e)
        {
            if (Table.SelectedItem != null)
                try
                {
                    var selectedAgent = Table.SelectedItem as Data.Agents;

                    WindowAdd editWrite = new WindowAdd(selectedAgent);
                    editWrite.ShowDialog();
                }
                catch (Exception)
                {
                    MessageBox.Show("Возникла ошибка!");
                }
        }

        private void ExportToPdf(object sender, RoutedEventArgs e)
        {
            var agents = PleasantRustleEntities.GetContext().Agents.ToList();
            var realisation = PleasantRustleEntities.GetContext().Realisation.ToList();
            var joinTable = (from a in agents
                             join r in realisation on a.ID equals r.AgentID
                             select new
                             {
                                 name = a.Name,
                                 phone = a.Phone,
                                 date = r.Date,
                                 count = r.Quantity,
                                 discount = a.Discount,
                                 product = r.Products.Name,                                        
                             }).ToList();

            SaveFileDialog fileDialog = new SaveFileDialog();

            fileDialog.Title = "Отчет";
            fileDialog.Filter = "PDF файл (*.pdf)|*.pdf";

            if (fileDialog.ShowDialog() == true)
            {
                try
                {
                    Document document = new Document();
                    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(fileDialog.FileName, FileMode.Create));

                    document.Open();

                    PdfPTable table = new PdfPTable(6);

                    BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\Arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    Font font = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL);

                    PdfPCell cell = new PdfPCell(new Phrase("  ", font));

                    cell.Colspan = 6;
                    cell.Border = 0;

                    cell = new PdfPCell(new Phrase(new Phrase("Компания", font)));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(new Phrase("Контактный телефон", font)));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);


                    cell = new PdfPCell(new Phrase(new Phrase("Дата", font)));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(new Phrase("Кол-во товара", font)));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(new Phrase("Скидка", font)));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(new Phrase("Товар", font)));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    foreach (var item in joinTable)
                    {
                        string cellValue1 = item.name.ToString();
                        table.AddCell(new Phrase(cellValue1, font));

                        string cellValue2 = item.phone.ToString();
                        table.AddCell(new Phrase(cellValue2, font));

                        string cellValue3 = item.date.ToString();
                        table.AddCell(new Phrase(cellValue3, font));

                        string cellValue4 = item.count.ToString();
                        table.AddCell(new Phrase(cellValue4, font));

                        string cellValue5 = item.discount.ToString();
                        table.AddCell(new Phrase(cellValue5, font));

                        string cellValue6 = item.product.ToString();
                        table.AddCell(new Phrase(cellValue6, font));
                    }

                    document.Add(table);
                    document.Close();
                    writer.Close();

                    MessageBox.Show("Файл сохранен!");
                }
                catch (Exception)
                {
                    MessageBox.Show("Возникла ошибка!");
                }
            }

        }
    }
}
