using Library.TaskManagement.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApplication.API.Database
{
    public class Filebase
    {
        private string _root;
        private string _CartRoot;
        private string _InventoryRoot;
        private string _CartsRoot;
        private string _tempI;

        private static Filebase _instance;
        public static Filebase Current
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new Filebase();
                }

                return _instance;
            }
        }

        private Filebase()
        {
            _root = "C:\\temp";
            _CartRoot = $"{_root}\\Cart";
            _InventoryRoot = $"{_root}\\Inventory";

            _CartsRoot = $"{_root}\\CartList";
            _tempI = $"{_root}\\SavedInventory";

            if (!Directory.Exists(_InventoryRoot))
                Directory.CreateDirectory(_InventoryRoot);

            if (!Directory.Exists(_CartRoot))
                Directory.CreateDirectory(_CartRoot);

            if (!Directory.Exists(_tempI))
                Directory.CreateDirectory(_tempI);

            if (!Directory.Exists(_CartsRoot))
                Directory.CreateDirectory(_CartsRoot);
        }

        public Product IAddOrUpdate(Product p)
        {
            //set up a new Id if one doesn't already exist
            if (p.Id <= 0) 
            {
                p.Id = NextId; 
            }

            //go to the right place]
            string pathI = null;
            
            var InventoryP = Inventory.FirstOrDefault(prod => prod.Id == p.Id);
            pathI = $"{_InventoryRoot}/{p.Id}.json";

            //if the item has been previously persisted
            if (File.Exists(pathI))
            {
                //blow it up
                File.Delete(pathI);
            }

            //write the file
            File.WriteAllText(pathI, JsonConvert.SerializeObject(p));
            //return the item, which now has an id
            return p;
        }

        public Product CAddOrUpdate(Product p)
        {
            //set up a new Id if one doesn't already exist
            if (p.Id <= 0)
            {
                p.Id = NextId;
            }

            //go to the right place]
            string pathC = null;
            
            pathC = $"{_CartRoot}/{p.Id}.json";

            //if the item has been previously persisted
            if (File.Exists(pathC))
            {
                //blow it up
                File.Delete(pathC);
            }

            //write the file
            File.WriteAllText(pathC, JsonConvert.SerializeObject(p));
            //return the item, which now has an id
            return p;
        }

        public bool Delete(int id)
        {
            //refer to AddOrUpdate for an idea of how you can implement this.
            var IPD = Inventory.FirstOrDefault(i => i.Id == id);
            var CPD = Cart.FirstOrDefault(i => i.Id == id);

            string pathI = null, pathC = null;

            if (IPD != null)
            {
                pathI = $"{_InventoryRoot}/{id}.json";
            }
            
            if (CPD != null)
            {
                pathC = $"{_CartRoot}/{id}.json";
            }

            if (File.Exists(pathI) && File.Exists(pathC))
            {
                File.Delete(pathI);
                File.Delete(pathC);
            }
            else
            if (File.Exists(pathI))
            {
                //blow it up
                File.Delete(pathI);
                return true;
            }
            else
            if (File.Exists(pathC))
            {
                //blow it up
                File.Delete(pathC);
                return true;
            }
            return false;
        }

        public List<Product> SaveI()
        {
            bool isEmpty = !Directory.EnumerateFiles(_tempI).Any();

            if (Directory.Exists(_tempI))
            {
                if (!isEmpty)
                {
                    Directory.Delete(_tempI, true);
                    Directory.CreateDirectory(_tempI);
                }
            }

            foreach (var p in Inventory) 
            {
                string pathTI = $"{_tempI}/{p.Id}.json";

                //if the item has been previously persisted
                if (File.Exists(pathTI))
                {
                    //blow it up
                    File.Delete(pathTI);
                }
                //write the file
                File.WriteAllText(pathTI, JsonConvert.SerializeObject(p));
            }
                return SavedI;
        }

        public List<Product> LoadI()
        {

            bool isEmpty = !Directory.EnumerateFiles(_InventoryRoot).Any();

            if (Directory.Exists(_InventoryRoot))
            {
                if (!isEmpty)
                {
                    Directory.Delete(_InventoryRoot, true);
                    Directory.CreateDirectory(_InventoryRoot);
                }
            }

            foreach (var p in SavedI) 
            {
                string pathI = $"{_InventoryRoot}/{p.Id}.json";

                //if the item has been previously persisted
                if (File.Exists(pathI))
                {
                    //blow it up
                    File.Delete(pathI);
                }
                //write the file
                File.WriteAllText(pathI, JsonConvert.SerializeObject(p));
            }

            return Inventory;
        }

        public List<Product> SaveC(string name)
        {
            var _CurrentRoot = $"{ _CartsRoot}/{name}"; //set and initialize subdirectory for current cart

            if (!Directory.Exists(_CurrentRoot))
                Directory.CreateDirectory(_CurrentRoot);

            bool isEmpty = !Directory.EnumerateFiles(_CurrentRoot).Any();

            if (Directory.Exists(_CurrentRoot))
            {
                if (!isEmpty)
                {
                    Directory.Delete(_CurrentRoot, true);
                    Directory.CreateDirectory(_CurrentRoot);
                }
            }

            foreach (var p in Cart) 
            {
                string pathC = $"{_CurrentRoot}/{p.Id}.json";

                //if the item has been previously persisted
                if (File.Exists(pathC))
                {
                    //blow it up
                    File.Delete(pathC);
                }
                //write the file
                File.WriteAllText(pathC, JsonConvert.SerializeObject(p));
            }

            var savedcart = CurrentCart(name);
            return savedcart;
        }

        public List<Product> LoadC(string name)
        {
            bool isEmpty = !Directory.EnumerateFiles(_CartRoot).Any();

            if (Directory.Exists(_CartRoot))
            {
                if (!isEmpty)
                {
                    Directory.Delete(_CartRoot, true);
                    Directory.CreateDirectory(_CartRoot);
                }
            }

            var loadcart = CurrentCart(name);
            foreach (var p in loadcart)
            {
                string pathC = $"{_CartRoot}/{p.Id}.json";

                //if the item has been previously persisted
                if (File.Exists(pathC))
                {
                    //blow it up
                    File.Delete(pathC);
                }
                //write the file
                File.WriteAllText(pathC, JsonConvert.SerializeObject(p));
            }

            return Cart;
        }


        public List<Product> CurrentCart(string name) 
        {
            var _CurrentRoot = $"{_CartsRoot}/{name}";
            var root = new DirectoryInfo(_CurrentRoot);
            var _Cart = new List<Product>();
            if (root.Exists)
            {
                foreach (var CFile in root.GetFiles())
                {
                    var p = JsonConvert.DeserializeObject<Product>(File.ReadAllText(CFile.FullName));
                    _Cart.Add(p);
                }
            }
            return _Cart;
        }

        public List<string> LoadCarts() 
        {
            var cdirectory = Directory.GetDirectories(_CartsRoot).Select(d => new DirectoryInfo(d).Name).ToList() ;
            //var cdirectory = Directory.EnumerateDirectories(_CartsRoot).ToList();
            return cdirectory;
        }
        public List<Product> Inventory
        {
            get
            {
                var root = new DirectoryInfo(_InventoryRoot);
                var _Inventory = new List<Product>();
                if (root.Exists)
                {
                    foreach (var IFile in root.GetFiles())
                    {
                        var p = JsonConvert.DeserializeObject<Product>(File.ReadAllText(IFile.FullName));
                        _Inventory.Add(p);
                    }
                }

                return _Inventory;
            }
        }

        public List<Product> Cart
        {
            get
            {
                var root = new DirectoryInfo(_CartRoot);
                var _Cart = new List<Product>();
                if (root.Exists) 
                {
                    foreach (var CFile in root.GetFiles())
                    {
                        var p = JsonConvert.DeserializeObject<Product>(File.ReadAllText(CFile.FullName));
                        _Cart.Add(p);
                    }
                }
                return _Cart;
            }
        }

        public List<Product> SavedI
        {
            get
            {
                var root = new DirectoryInfo(_tempI);
                var _tI = new List<Product>();
                if (root.Exists)
                {
                    foreach (var tempI in root.GetFiles())
                    {
                        var p = JsonConvert.DeserializeObject<Product>(File.ReadAllText(tempI.FullName));
                        _tI.Add(p);
                    }
                }
                return _tI;
            }
        }

        public int NextId
        {
            get
            {
                if (!Inventory.Any()) 
                {
                    return 1;
                }
                return Inventory.Select(i => i.Id).Max() + 1;
            }
        }
    }
}
