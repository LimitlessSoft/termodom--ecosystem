using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using System.Collections;

namespace TDBrain_v3.DB
{
    /// <summary>
    /// 
    /// </summary>
    public class ConnectionStringCollection
    {
        /// <summary>
        /// godina, magacinid, connectionString
        /// </summary>
        private Dictionary<int, Dictionary<int, string>> _putanjeDoBaza = new Dictionary<int, Dictionary<int, string>>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="magacinId"></param>
        /// <param name="godina"></param>
        /// <returns></returns>
        public string this[int magacinId, int godina]
        {
            get
            {
                if (!_putanjeDoBaza.ContainsKey(godina) || !_putanjeDoBaza[godina].ContainsKey(magacinId))
                    throw new Exceptions.PathToMainDatabaseNotFoundException(magacinId, godina);

                string putanjaDoBaze = _putanjeDoBaza[godina][magacinId];
                return $"data source={Settings.ServerName}; initial catalog = {putanjaDoBaze}; user={Settings.User}; password={Settings.Password}";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="magacinId"></param>
        /// <param name="godina"></param>
        /// <param name="putanjaDoBaze"></param>
        public void AddOrUpdate(int magacinId, int godina, string putanjaDoBaze)
        {
            if (!_putanjeDoBaza.ContainsKey(godina))
                _putanjeDoBaza.Add(godina, new Dictionary<int, string>());

            if (!_putanjeDoBaza[godina].ContainsKey(magacinId))
                _putanjeDoBaza[godina].Add(magacinId, putanjaDoBaze);
            else
                _putanjeDoBaza[godina][magacinId] = putanjaDoBaze;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="magacinId"></param>
        /// <param name="godina"></param>
        public void Remove(int magacinId, int godina)
        {
            if (!_putanjeDoBaza.ContainsKey(godina))
                return;

            _putanjeDoBaza[godina].Remove(magacinId);
        }
        /// <summary>
        /// 
        /// </summary>
        public void RemoveAll()
        {
            _putanjeDoBaza = new Dictionary<int, Dictionary<int, string>>();
        }
        /// <summary>
        /// Vraca listu magacina za koje je insertovan connection string za datu godinu
        /// </summary>
        /// <returns></returns>
        public int[] GetMagacini(int godina)
        {
            return _putanjeDoBaza[godina].Keys.ToArray();
        }
        /// <summary>
        /// Vraca listu magacina koji imaju dati connection string
        /// </summary>
        /// <param name="godina"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public int[] GetMagacini(int godina, string connectionString)
        {
            List<int> list = new List<int>();
            foreach(int m in _putanjeDoBaza[godina].Keys)
                if (connectionString.ToLower().Contains(_putanjeDoBaza[godina][m].ToLower()))
                    list.Add(m);
            return list.ToArray();
        }
        /// <summary>
        /// Vraca unikatne connection stringove ka bazama za datu godinu
        /// </summary>
        /// <returns></returns>
        public string[] GetConnectionStringsDistinct(int godina)
        {
            string[] paths = _putanjeDoBaza[godina].Values.Distinct().ToArray();
            string[] output = new string[paths.Length];

            for(int i = 0; i < output.Length; i++)
                output[i] = $"data source={Settings.ServerName}; initial catalog = {paths[i]}; user={Settings.User}; password={Settings.Password}";

            return output;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TDBrain_v3.Settings.DTO.DBSettingsDTO.PutanjeDoBazaDTO ToPutanjeDoBazaDTO()
        {
            TDBrain_v3.Settings.DTO.DBSettingsDTO.PutanjeDoBazaDTO dto = new TDBrain_v3.Settings.DTO.DBSettingsDTO.PutanjeDoBazaDTO();

            foreach(int godina in _putanjeDoBaza.Keys)
            {
                foreach(int magacinid in _putanjeDoBaza[godina].Keys)
                {
                    dto.Items.Add(new TDBrain_v3.Settings.DTO.DBSettingsDTO.PutanjeDoBazaDTO.Item()
                    {
                        MagacinID = magacinid,
                        Godina = godina,
                        PutanjaDoBaze = _putanjeDoBaza[godina][magacinid]
                    });
                }
            }
            return dto;
        }
    }
}
