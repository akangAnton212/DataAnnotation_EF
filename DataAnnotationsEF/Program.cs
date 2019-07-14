using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading;

namespace DataAnnotationsEF
{
    [Table("Author")]
    class Author
    {
        [Key]
        public int AuthorID { get; set; }

        [MaxLength(20), Required]
        public string FirstName { get; set; }

        [MaxLength(20), Required]
        public string LastName { get; set; }

        [NotMapped]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        public List<Book> Books { get; set; }
    }

    [Table("Book")]
    class Book
    {
        [Key, Column(Order = 0)]
        public int BookID { get; set; }

        [Column(Order = 1)]
        [Required]
        public string BookName { get; set; }


        [Column(Order = 2)]
        [ConcurrencyCheck]
        [Required]
        public double PriceUnit { get; set; }

        //[Column(Order = 3)]
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //public DateTime CreatedOn { get; set; }

        public DateTime CreatedOn
        {
            get
            {
                return this.dateCreated.HasValue
                   ? this.dateCreated.Value
                   : DateTime.Now;
            }

            set { this.dateCreated = value; }
        }

        private DateTime? dateCreated = null;

        [Column(Order = 4)]
        [ForeignKey("Author")]  
        public int AuthorID { get; set; }

        public Author Author { get; set; }
    }

    class ShopDbContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=EFCoreShop;Trusted_Connection=True;");

            //use lazy load
            //install Microsoft.EntityFrameworkCore.Proxies
            //optionsBuilder.UseLazyLoadingProxies();
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {

            ////METODE INSERT

            ////ShopDbContext shopDbContext = new ShopDbContext();
            ////Author author = new Author()
            ////{
            ////    FirstName = "Anton",
            ////    LastName = "Sujarwo"
            ////};

            ////shopDbContext.Add<Author>(author);
            ////shopDbContext.SaveChanges();
            ////shopDbContext.Dispose();

            ////using (ShopDbContext shopDbContext = new ShopDbContext())
            ////{
            ////    Author author = new Author()
            ////    {
            ////        FirstName = "Kang Anton",
            ////        LastName = "Sujarwo Pratama"
            ////    };

            ////    shopDbContext.Add<Author>(author);
            ////    shopDbContext.SaveChanges();
            ////    shopDbContext.Dispose();
            ////}

            ////END METODE INSERT


            //======================

            ////HAPUS DATA

            //using (ShopDbContext shopDbContext = new ShopDbContext())
            //{
            //    Author author = shopDbContext.Authors.Find(7);
            //    //shopDbContext.Authors.Remove(author);
            //    //shopDbContext.SaveChanges();

            //    shopDbContext.Remove<Author>(author);
            //    shopDbContext.SaveChanges();

            //}

            ////END HAPUS DATA

            //===========================

            ////UPDATE DATA
            //using (ShopDbContext shopDbContext = new ShopDbContext())
            //{
            //    Author author = shopDbContext.Authors.Find(5);
            //    author.LastName = "Duntz";

            //    shopDbContext.Update<Author>(author);
            //    shopDbContext.SaveChanges();

            //}
            ////END UPDATE DATA

            //==============================
            ////Console.Write("masukan Author ID : ");
            ////int authorID = int.Parse(Console.ReadLine());

            ////using (var shopDBContext = new ShopDbContext())
            ////{
            ////    Author author;
            ////    //Author author = shopDBContext.Authors.Find(authorID);

            ////    author = shopDBContext.Authors.Where(x => x.AuthorID == authorID).FirstOrDefault();

            ////    if (author != null)
            ////    {
            ////        Console.WriteLine("AuthorID:{0} Nama:{1}", author.AuthorID, author.FullName);
            ////    }
            ////    else
            ////    {
            ////        Console.WriteLine("Author Tidak ada");
            ////    }

            ////    Console.ReadLine();
            ////}
            ////====================================
            //ShopDbContext shopDbContext = new ShopDbContext();
            ////List<Book> listBooks = shopDbContext.Books.Where(x => x.PriceUnit > 2000 && x.PriceUnit < 5000).ToList();

            //List<Book> listBooks = shopDbContext.Books.Where(x => x.Author.FirstName == "Anton").ToList();

            //foreach (var books in listBooks)
            //{
            //    Console.WriteLine("Book ID:{0}, Book Name:{1}, Price:{2}", books.BookID, books.BookName, books.PriceUnit);
            //}

            //Console.ReadLine();

            ////=========================================

            ////INSERT KROYOKAN
            //string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //ShopDbContext shopDbContext = new ShopDbContext();
            //for (int i = 0; i < 100; i++)
            //{

            //    try
            //    {
            //        Random r = new Random();

            //        Book book = new Book()
            //        {
            //            AuthorID = r.Next(1, 6),
            //            BookName = "Belajar C# Itu Mudah Part " + i,
            //            PriceUnit = r.Next(2000, 10000),
            //            CreatedOn = Convert.ToDateTime(today)
            //        };

            //        shopDbContext.Add<Book>(book);
            //        shopDbContext.SaveChanges();

            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e);
            //        break;
            //    }

            //    Thread.Sleep(1000);
            //}
            //shopDbContext.Dispose();
            //Console.ReadLine();

            ////END INSERT KROYOKAN =====================



            //EPISODE EAGER LOADING, LAZY LOADING, EXPLICIT LOADING

            using (ShopDbContext shopDbContext = new ShopDbContext())
            {
                //EAGER LOADING ================================================
                //IEnumerable<Author> authors = shopDbContext.Authors.Include(x => x.Books).ToList();

                //foreach (var author in authors)
                //{
                //    Console.WriteLine("Name Of Author {0} is : ", author.FullName);
                //    foreach (var book in author.Books)
                //    {
                //        Console.WriteLine("Books Name {0}", book.BookName);
                //    }
                //}

                //Book myBooks = shopDbContext.Books.Include(x => x.Author).Where(y => y.AuthorID == 2).FirstOrDefault();

                //END EAGER LOADING ================================================


                //EXPLICIT LOADING ================================================
                //IEnumerable<Author> authors = shopDbContext.Authors.ToList();

                //foreach (var author in authors)
                //{
                //    Console.WriteLine("Name Of Author {0} is : ", author.FullName);

                //    shopDbContext.Entry(author).Collection(x => x.Books).Load();

                //    foreach (var book in author.Books)
                //    {
                //        Console.WriteLine("Books Name {0}", book.BookName);
                //    }
                //}

                //Book myBooks = shopDbContext.Books.Include(x => x.Author).Where(y => y.AuthorID == 2).FirstOrDefault();

                //END EXPLICIT LOADING ================================================

                //LAZY LOADING ================================================
                //IEnumerable<Author> authors = shopDbContext.Authors.Include(x => x.Books).ToList();

                //foreach (var author in authors)
                //{
                //    Console.WriteLine("Name Of Author {0} is : ", author.FullName);
                //    foreach (var book in author.Books)
                //    {
                //        Console.WriteLine("Books Name {0} is : ", book.BookName);
                //    }
                //}

                //Book myBooks = shopDbContext.Books.Where(y => y.BookID == 50).FirstOrDefault();
                //string authorName = myBooks.Author.FullName;
                //Console.WriteLine(authorName);

                //END LAZY LOADING ================================================



                Console.ReadLine();
            }


            //END EPISODE EAGER LOADING, LAZY LOADING, EXPLICIT LOADING
        }
    }
}
