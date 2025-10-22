using NPOI.SS.UserModel;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Comfort
{
    public class ProductionViewModel
    {
        Production production = new Production();
        public List<Production> productions = new List<Production>();
        public ICommand SaveCommand { get; set; }
        public ICommand GetCommand { get; set; }
        public ICommand ImportExcelCommand { get; set; }
        

        SQLiteAsyncConnection database;

        async Task Init()
        {
            if (database is not null)
                return;

            database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await database.CreateTableAsync<Production>();
        }

        public string Name
        {
            get => production.Name;
            set
            {
                if (production.Name != value)
                {
                    production.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Article
        {
            get => production.Article;
            set
            {
                if (production.Article != value)
                {
                    production.Article = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Type
        {
            get => production.Type;
            set
            {
                if (production.Type != value)
                {
                    production.Type = value;
                    OnPropertyChanged();
                }
            }
        }

        public double MinValue
        {
            get => production.MinValue;
            set
            {
                if (production.MinValue != value)
                {
                    production.MinValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Material
        {
            get => production.Material;
            set
            {
                if (production.Material != value)
                {
                    production.Material = value;
                    OnPropertyChanged();
                }
            }
        }

        public ProductionViewModel()
        {

            SaveCommand = new Command(() =>
            {
                Production product = new Production(Type, Name, Article, MinValue, Material);
                SaveItemAsync(product);

            });
            GetCommand = new Command(async () =>
            {
                productions = await GetItemsAsync();
            });
            ImportExcelCommand = new Command(async () =>
            {
                if (GetItemAsync(0) != null)
                {
                    string path = @"C:\Users\nikit\source\repos\Comfort\Comfort\Resources\Raw\Products_import.xlsx";

                    using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        IWorkbook workbook = WorkbookFactory.Create(fileStream);
                        ISheet sheet = workbook.GetSheetAt(0);
                        for (int rowIndex = 1; rowIndex <= sheet.LastRowNum; rowIndex++)
                        {
                            IRow row = sheet.GetRow(rowIndex);
                            if (row == null) continue; //пропускаем пустые ячейки

                            string typeOutput = " ";
                            string nameOutput = " ";
                            string articleOutput = " ";
                            double minValueOutput = 0;
                            string materialOutput = " ";

                            //Просматриваем все ячейки в текущей строке
                            for (int cellIndex = 0; cellIndex < row.LastCellNum; cellIndex++)
                            {

                                ICell cell = row.GetCell(cellIndex);
                                //Получаем значения ячеек, пустые ячейки записываем как пустые строки
                                string cellValue = cell?.ToString() ?? "";

                                if (cellIndex == 0) { typeOutput = cellValue; }  //если первая ячейка строки, то её значение = тип
                                if (cellIndex == 1) { nameOutput = cellValue; }  //если вторая ячейка строки, то её значение = имя и т.д.
                                if (cellIndex == 2) { articleOutput = cellValue; }
                                if (cellIndex == 3)
                                {
                                    bool b = double.TryParse(cellValue, out minValueOutput);
                                    minValueOutput = b == false ? 0.0 : minValueOutput;
                                }
                                if (cellIndex == 4) { materialOutput = cellValue; }
                            }
                            Production product = new Production(typeOutput, nameOutput, articleOutput, minValueOutput, materialOutput);
                            await SaveItemAsync(product);
                        }
                    }   //следующий код вызывает вечную загрузку в окне
                    productions = GetItemsAsync().Result;
                }
                else { return; }
            });
        }

        public async Task<Production> GetItemAsync(int id)  //Получить по ID
        {
            await Init();
            return await database.Table<Production>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public async Task<List<Production>> GetItemsAsync()
        {
            await Init();
            return await database.Table<Production>().ToListAsync();
        }

        public async Task<int> SaveItemAsync(Production product)    //Сохранить через конструктор
        {
            await Init();
            if (product.ID != 0)
                return await database.UpdateAsync(product);

            else
                return await database.InsertAsync(product);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

}
