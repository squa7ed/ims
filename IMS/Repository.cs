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

        private static IList<T> repo = Register();

        private static bool isDirty = false;
        #endregion

        #region Public fields
        public delegate void RepositoryChangedEventhandler(T entity = null);

        public static event RepositoryChangedEventhandler RepositoryChanged;
        #endregion

        #region Public methods
        public static IList<T> Get()
        {
            return repo;
        }

        public static void Add(T entity)
        {
            repo.Add(entity);
            isDirty = true;
        }

        public static void Remove(T entity)
        {
            repo.Remove(entity);
            isDirty = true;
        }

        public static void Update(T entity)
        {
            Remove(entity);
            Add(entity);
        }

        public static void SaveChanges()
        {
            if (isDirty)
            {
                string file = Path.Combine(dir, typeof(T).ToString());
                try
                {
                    {
                        using (var fs = File.Create(file))
                        {
                            new BinaryFormatter().Serialize(fs, repo);
                            fs.Flush();
                        }
                    }
                    isDirty = false;
                    NotifyRepositoryChanged();
                    repo = ReadFromFile();
                }
                catch (Exception e)
                {
                    Log.Warning("Error while saving repository to file {0}.", file);
                    Log.Warning(e.Message);
                }
            }
        }

        public async static void SaveChangesAsync()
        {
            if (isDirty)
            {
                await Task.Run(() =>
                    {
                        string file = Path.Combine(dir, typeof(T).ToString());
                        try
                        {
                            {
                                using (var fs = File.Create(file))
                                {
                                    new BinaryFormatter().Serialize(fs, repo);
                                    fs.Flush();
                                }
                            }
                            isDirty = false;
                            NotifyRepositoryChanged();
                            repo = ReadFromFile();
                        }
                        catch (Exception e)
                        {
                            Log.Warning("Error while saving repository to file {0}.", file);
                            Log.Warning(e.Message);
                        }
                    });

            }
        }

        #endregion

        #region Private methods
        private static void NotifyRepositoryChanged(T entity = null)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)delegate { RepositoryChanged?.Invoke(entity); });
        }

        private static IList<T> Register()
        {
            Application.Current.Exit += (o, e) => { if (isDirty) { SaveChanges(); } };
            IList<T> list = null;
            list = ReadFromFile();
            if (list == null)
            {
                list = new List<T>();
            }
            return list;
        }

        private static IList<T> ReadFromFile()
        {
            IList<T> list = null;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string file = Path.Combine(dir, typeof(T).ToString());
            if (File.Exists(file))
            {
                try
                {
                    using (var ms = new MemoryStream(File.ReadAllBytes(file)))
                    {
                        list = new BinaryFormatter().Deserialize(ms) as List<T>;
                    }
                }
                catch (Exception e)
                {
                    Log.Warning("An error happend while reading repository {0} from file {1}.", typeof(T).ToString(), file);
                    Log.Warning(e.Message);
                }
            }
            return list;
        }
        #endregion
    }
}
