using IMS.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IMS
{

    public static class Repository<T> where T : BaseEntity
    {
        #region Private fields
        private const string dir = "MockData";

        private static ObservableCollection<T> repo = Register();

        private static bool isDirty = false;
        #endregion

        #region Public fields
        public delegate void RepositoryChangedEventhandler(T entity);

        public static event RepositoryChangedEventhandler RepositoryChanged;
        #endregion

        #region Public methods
        public static ObservableCollection<T> Get()
        {
            return repo;
        }

        public static void Add(T entity)
        {
            if (!repo.Any(x => x.Id == entity.Id))
            {
                repo.Add(entity);
                NotifyRepositoryChange(entity);
            }
        }

        public static void Update(T entity)
        {
            if (repo.Any(x => x.Id == entity.Id))
            {
                var orig = repo.Single(x => x.Id == entity.Id);
                int index = repo.IndexOf(orig);
                repo.RemoveAt(index);
                repo.Insert(index, entity);
                NotifyRepositoryChange(entity);
            }
        }

        public static void Remove(T entity)
        {
            if (repo.Any(x => x.Id == entity.Id))
            {
                var orig = repo.Single(x => x.Id == entity.Id);
                repo.Remove(orig);
                NotifyRepositoryChange(entity);
            }
        }

        #endregion

        #region Private methods
        private static void NotifyRepositoryChange(T entity)
        {
            RepositoryChanged?.Invoke(entity);
            isDirty = true;
        }

        private static void SaveChanges()
        {
            string file = Path.Combine(dir, typeof(T).ToString());
            try
            {
                {
                    using (var fs = File.Create(file))
                    {
                        using (var ms = new MemoryStream())
                        {
                            new BinaryFormatter().Serialize(ms, repo);
                            fs.Write(ms.ToArray(), 0, (int)ms.Length);
                            fs.Flush();
                        }
                    }
                }
            }
            catch (Exception)
            {
                Log.Warning("Error while saving repository to file {0}.", file);
            }
        }

        private static ObservableCollection<T> Register()
        {
            Application.Current.Exit += OnApplicationExit;
            ObservableCollection<T> list = null;
            list = ReadFromFile();
            if (list == null)
            {
                list = new ObservableCollection<T>();
            }
            return list;
        }

        private static void OnApplicationExit(object sender, ExitEventArgs e)
        {
            if (isDirty)
            {
                SaveChanges();
            }
        }

        private static ObservableCollection<T> ReadFromFile()
        {
            ObservableCollection<T> list = null;
            if (Directory.Exists(dir))
            {
                string file = Path.Combine(dir, typeof(T).ToString());
                if (File.Exists(file))
                {
                    try
                    {
                        using (var ms = new MemoryStream(File.ReadAllBytes(file)))
                        {
                            list = new BinaryFormatter().Deserialize(ms) as ObservableCollection<T>;
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Warning("An error happend while reading repository {0} from file {1}.", typeof(T).ToString(), file);
                        Log.Warning(e.Message);
                    }
                }
            }
            return list;
        }
        #endregion
    }
}
