package main

import (
	"database/sql"
	"fmt"
	"log"

	"github.com/fatih/color"
	_ "github.com/go-sql-driver/mysql"
)

func main() {
  var errorWriter ErrorWriter
  logError = log.New(errorWriter, "ERROR: ", log.Ldate|log.Ltime|log.Lshortfile)

  dropDatabase()
  createdb()
  // addTestData()
}

func rootOpenDatabase(dbname string) (pool *sql.DB) {
  pool, err := sql.Open("mysql", fmt.Sprintf("root:o1M@2UO4ngwg!i9R$3hvLSVpt@(localhost:3307)/%v", dbname)) // TODO: get the password from a file
  if (err != nil) { logError.Fatal(err.Error()) } // TODO: error handling

  pool.SetConnMaxLifetime(0)
  pool.SetMaxIdleConns(1)
  pool.SetMaxOpenConns(1)
  if err := pool.Ping(); err != nil { logError.Fatal(err.Error()) }

  return pool
}

func dropDatabase() {
  pool := rootOpenDatabase("")
  log.Print("Dropping webui Database.")
  pool.Exec("DROP DATABASE webui")
}

func createdb() {
  pool := rootOpenDatabase("")

  log.Print("Creating webui Database.")
  _, err := pool.Exec(`CREATE DATABASE webui;`)
  if (err != nil) { logError.Fatal(err.Error()) }
  log.Print("webui database successfully created!")
  _, err = pool.Exec(`USE webui;`)
  if (err != nil) { logError.Fatal(err.Error()) }
  log.Print("webui database selected.")
    
  _, err = pool.Exec(`CREATE TABLE categories (
    id VARCHAR(64) NOT NULL,
    category VARCHAR(20) NOT NULL,
    PRIMARY KEY (id)
  );`)
  if (err != nil) { logError.Fatal(err.Error()) }
  log.Print("categories table created!")
    
  _, err = pool.Exec(`CREATE TABLE products (
    id VARCHAR(64) NOT NULL,
    name VARCHAR(32) NOT NULL,
    description VARCHAR(200) NOT NULL,
    price VARCHAR(32) NOT NULL,
    category VARCHAR(64) NOT NULL,
    imageFileName VARCHAR(64) NOT NULL,
    PRIMARY KEY (id),
    FOREIGN KEY (category)
      REFERENCES categories(id)
      ON DELETE CASCADE
      ON UPDATE CASCADE
  )`)
  if (err != nil) { logError.Fatal(err.Error()) }
  log.Print("products table created!")

  // _, err = pool.Exec(`CREATE TABLE user (
  //   username VARCHAR(32) NOT NULL,
  //   password VARCHAR(32) NOT NULL,
  //   role ENUM('user', 'editor', 'creator', 'admin'),
  //   PRIMARY KEY (username)
  // );`)
  // if (err != nil) { logError.Fatal(err.Error()) }
  // log.Print("user table created!")
}

func addAdminUser() {
  // pool := rootOpenDatabase("charityshowcase")
  // _, err := pool.Exec(`INSERT INTO user (username, password, role) VALUES ("admin", "pass", "admin");`) // TODO: more secure credentials
  // if (err != nil) { logError.Fatal(err.Error()) }
  // log.Print("Admin user created!")
}

// func addTestData() {
//   pool := rootOpenDatabase("charityshowcase")
  
//   testdata, err := os.ReadFile("./testdata.json")
//   if (err != nil) {
//     logError.Fatal("Failed to read file testdata.json")
//   }
//   var charityShowcase CharityShowcase
//   err = json.Unmarshal(testdata, &charityShowcase)
//   if (err != nil) {
//     logError.Fatal(err)
//   }

//   for i := 0; i < len(charityShowcase.Technologies); i++ {
//     _, err = pool.Exec(`INSERT INTO technology (name, imageFileName) VALUES (?, ?);`, charityShowcase.Technologies[i].Name, charityShowcase.Technologies[i].SVG) // TODO: more secure credentials
//     if (err != nil) { logError.Fatal(err.Error()) }
//   }
//   log.Print("Test data created for the technology table!")

//   for i := 0; i < len(charityShowcase.CharityProjects); i++ {
//     charityProject := charityShowcase.CharityProjects[i]
//     _, err = pool.Exec(`INSERT INTO charityproject (name, shortDescription, longDescription, archived) VALUES (?, ?, ?, ?);`, charityProject.Name, charityProject.ShortDescription, charityProject.LongDescription, charityProject.Archived) // TODO: more secure credentials
//     if (err != nil) { logError.Fatal(err.Error()) }
//     for technologyIndex := 0; technologyIndex < len(charityProject.Technologies); technologyIndex++ {
//       technology := charityProject.Technologies[technologyIndex]
//       _, err = pool.Exec(`INSERT INTO technologytocharityproject (technology, charityproject) VALUES (?, ?);`, technology.Name, charityProject.Name) // TODO: more secure credentials
//       if (err != nil) { logError.Fatal(err.Error()) }
//     }
//   }
//   log.Print("Test data created for the charityproject table!")

//   for i := 0; i < len(charityShowcase.Users); i++ {
//     user := charityShowcase.Users[i]
//     _, err = pool.Exec(`INSERT INTO user (username, password, role) VALUES (?, ?, ?);`, user.Username, user.Password, user.Role) // TODO: more secure credentials
//     if (err != nil) { logError.Fatal(err.Error()) }
//   }
//   log.Print("Test data created for the user table!")
//   addAdminUser()
// }

type Category struct {
  Id int
  Category string
}
type Product struct {
  Id int
  Name string
  Description string
  Price string
  Category string
  ImageFileName string
}
// type User struct {
//   Username string
//   Password string
//   Role string
// }

func (errorWriter ErrorWriter) Write(p []byte) (n int, err error) { // TODO: fix common code across modules
  color.Red(string(p))
  return 0, nil
}
type ErrorWriter struct {}
var logError *log.Logger
