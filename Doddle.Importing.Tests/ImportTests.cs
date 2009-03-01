using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;

namespace Doddle.Importing.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ImportTests
    {
        public ImportTests()
        {
  
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod]
        public void ImporterWorks()
        {
            IImportSource source = GetValidImportSource();

            List<Product> destinationList = new List<Product>();
            IImportDestination destination = new ImportableCollection<Product>(destinationList);

            Importer importer = new Importer();
            importer.Import(source, destination, ImportValidationMode.BypassValidation);


            Assert.AreEqual(24, destinationList.Count);
        }


        [TestMethod]
        public void Adding_FieldError_To_RowValidation_Marks_Invalid()
        {
            IImportSource source = GetCategorySource();

            List<Product> destinationList = new List<Product>();
            IImportDestination destination = new ImportableCollection<Product>(destinationList);

            ImportValidator validator = new ImportValidator();
            var validationResult = validator.Validate(source, destination);

            Assert.IsFalse(validationResult.IsSourceValid);
        }

        [TestMethod]
        public void DataTypeValidation()
        {
            ImportableDictionary source = new ImportableDictionary();
            source.Fields.Add("ProductID", typeof(int));
            source.Fields.Add("ProductName", typeof(string));

            IDictionary row1 = source.AddRow();
            row1["ProductID"] = "A";
            row1["ProductName"] = "My Product";


            IImportDestination destination = new ImportableCollection<Product>(new List<Product>());

            ImportValidator validator = new ImportValidator();
            validator.Rules.Clear();
            validator.Rules.Add("DataTypeValidationRule", new DataTypeValidationRule());

            var result = validator.Validate(source, destination);

    
            Assert.IsTrue(result.RowResults[0].FieldErrors[0].Message.Contains("Unable to convert the source"));
        }



        [TestMethod]
        public void RequiredFieldsRule()
        {
            ImportableDictionary source = new ImportableDictionary();
            source.Fields.Add("ProductName", typeof(string));

            IDictionary row1 = source.AddRow();
            row1["ProductID"] = "A";
            row1["ProductName"] = "My Product";

            IImportDestination destination = new ImportableCollection<Product>(new List<Product>());

            ImportValidator validator = new ImportValidator();
            validator.Rules.Clear();
            validator.Rules.Add("RequiredFieldsRule", new RequiredFieldsRule());

            var result = validator.Validate(source, destination);

            Assert.IsFalse(result.IsSourceValid);
            Assert.IsTrue(result.RowResults[0].FieldErrors[0].Message.Contains("Required field"), result.RowResults[0].FieldErrors[0].Message);
        }

        [TestMethod]
        public void MissingHeadersRule()
        {
            ImportableDictionary source = new ImportableDictionary();
            source.Fields.Add("ProductName", typeof(string));

            IDictionary row1 = source.AddRow();
            row1["ProductID"] = "A";
            row1["ProductName"] = "My Product";

            IImportDestination destination = new ImportableCollection<Product>(new List<Product>());
            
            ImportValidator validator = new ImportValidator();
            validator.Rules.Clear();
            validator.Rules.Add("MissingHeadersRule", new MissingHeadersRule());

            var result = validator.Validate(source, destination);

            Assert.IsFalse(result.IsSourceValid);
            Assert.IsTrue(result.RowResults[0].FieldErrors[0].Message.Contains("Required field"), result.RowResults[0].FieldErrors[0].Message);
        }


        [TestMethod]
        public void Validation_Messages_Working()
        {
            IImportSource source = GetCategorySource();
            IImportDestination destination = new ImportableCollection<Product>(new List<Product>());

            ImportValidator validator = new ImportValidator();
            var validationResult = validator.Validate(source, destination);

            Assert.IsNotNull(validationResult.RowResults[0].FieldErrors[0].Message);
        }

        [TestMethod]
        public void RowImporting_Event_Allows_Modification_Of_Field_Data()
        {
            IImportSource source = GetValidImportSource();

            List<Product> destinationList = new List<Product>();
            IImportDestination destination = new ImportableCollection<Product>(destinationList);

            Importer importer = new Importer();
            importer.RowImporting += importer_RowImporting;

            importer.Import(source, destination);


            Assert.AreEqual("Overriden", destinationList[0].ProductName);
        }

        private void importer_RowImporting(object sender, ImportRowEventArgs e)
        {
            e.Row["ProductName"] = "Overriden";
        }



        private IImportSource GetValidImportSource()
        {
            List<Product> products = new List<Product>();
            for (int i = 1; i < 25; i++)
            {
                Product p = new Product();
                p.ProductID = i;
                p.ProductName = string.Format("Product {0}", i);
                products.Add(p);
            }

            return new ImportableCollection<Product>(products);
        }

        private IImportSource GetCategorySource()
        {
            List<Category> categories = new List<Category>();
            for (int i = 1; i < 25; i++)
            {
                Category c = new Category();
                c.CategoryName = string.Format("Category {0}", i);
                categories.Add(c);
            }

            return new ImportableCollection<Category>(categories);
        }

        private IImportSource GetInvalidImportSource()
        {
            List<Product> products = new List<Product>();
            for (int i = 1; i < 25; i++)
            {
                Product p = new Product();
                p.ProductName = string.Format("Product {0}", i);
                products.Add(p);
            }

            return new ImportableCollection<Product>(products);
        }

    }
}
