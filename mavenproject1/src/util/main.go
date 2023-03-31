package main

import (
	"database/sql"
	"encoding/json"
	"log"
	"os"

	"github.com/fatih/color"
	_ "github.com/mattn/go-sqlite3"
)

func main() {
  var errorWriter ErrorWriter
  logError = log.New(errorWriter, "ERROR: ", log.Ldate|log.Ltime|log.Lshortfile)

  dropDatabase()
  createdb()
  addTestData()
}

func openDatabase() (pool *sql.DB) {
  pool, err := sql.Open("sqlite3", "../../assets/mcec.db") // TODO: get the password from a file
  if (err != nil) { logError.Fatal(err.Error()) } // TODO: error handling

  pool.SetConnMaxLifetime(0)
  pool.SetMaxIdleConns(1)
  pool.SetMaxOpenConns(1)
  if err := pool.Ping(); err != nil { logError.Fatal(err.Error()) }

  return pool
}

func dropDatabase() {
  log.Print("Dropping mcec Database.")
  err := os.Remove("../../assets/mcec.db")
  if (err != nil && !os.IsNotExist(err)) { logError.Fatal(err.Error()) }
  log.Print("Database dropped!")
}

func createdb() {
  pool := openDatabase()

  _, err := pool.Exec(`CREATE TABLE modelCars (
    name VARCHAR(32) NOT NULL,
    description VARCHAR(1000) NOT NULL,
    category VARCHAR(64) NOT NULL,
    price VARCHAR(10) NOT NULL,
    imageFilePath VARCHAR(128) NOT NULL
  );`)
  if (err != nil) { logError.Fatal(err.Error()) }
  log.Print("modelCars table created!")

  _, err = pool.Exec(`CREATE TABLE users (
    username VARCHAR(32) NOT NULL,
    password VARCHAR(32) NOT NULL,
    PRIMARY KEY (username)
  );`)
  if (err != nil) { logError.Fatal(err.Error()) }
  log.Print("users table created!")

  _, err = pool.Exec(`CREATE TABLE ownedModelCars (
    modelCarId VARCHAR(64) NOT NULL,
    username VARCHAR(32) NOT NULL,
    dateCollected CHAR(10) NOT NULL,
    PRIMARY KEY (modelCarId, username)
    FOREIGN KEY (modelCarId)
      REFERENCES modelCars(id)
      ON DELETE CASCADE
      ON UPDATE CASCADE
    FOREIGN KEY (username)
      REFERENCES users(username)
      ON DELETE CASCADE
      ON UPDATE CASCADE
  );`)
  if (err != nil) { logError.Fatal(err.Error()) }
  log.Print("ownedModelCars table created!")

  _, err = pool.Exec(`CREATE TABLE wishedModelCars (
    modelCarId VARCHAR(64) NOT NULL,
    username VARCHAR(32) NOT NULL,
    wishFactor CHAR(10) NOT NULL,
    PRIMARY KEY (modelCarId, username)
    FOREIGN KEY (modelCarId)
      REFERENCES modelCars(id)
      ON DELETE CASCADE
      ON UPDATE CASCADE
    FOREIGN KEY (username)
      REFERENCES users(username)
      ON DELETE CASCADE
      ON UPDATE CASCADE
  );`)
  if (err != nil) { logError.Fatal(err.Error()) }
  log.Print("ownedModelCars table created!")
}

func addTestData() {
  pool := openDatabase()
  
  testdata, err := os.ReadFile("./testdata.json")
  if (err != nil) {
    logError.Fatal("Failed to read file testdata.json")
  }
  var mcec Mcec
  err = json.Unmarshal(testdata, &mcec)
  if (err != nil) {
    logError.Fatal(err)
  }

  for i := 0; i < len(mcec.ModelCars); i++ {
    car := mcec.ModelCars[i]
    _, err = pool.Exec(`INSERT INTO modelCars (name, description, category, price, imageFilePath) VALUES (?, ?, ?, ?, ?);`, car.Name, car.Description, car.Category, car.Price, car.ImageFilePath)
    if (err != nil) { logError.Fatal(err.Error()) }
  }
  log.Print("Test data created for the cars table!")

  for userIndex := 0; userIndex < len(mcec.Users); userIndex++ {
    user := mcec.Users[userIndex]
    _, err = pool.Exec(`INSERT INTO users (username, password) VALUES (?, ?);`, user.Username, user.Password)
    if (err != nil) { logError.Fatal(err.Error()) }

    for ownedIndex := 0; ownedIndex < len(user.OwnedModelCars); ownedIndex++ {
      car := user.OwnedModelCars[ownedIndex]

      var modelCarId int;
      result := pool.QueryRow(`SELECT rowid FROM modelCars WHERE name=?;`, car.ModelCarName)
      err = result.Scan(&modelCarId)
      if (err != nil) { logError.Fatal(err.Error()) }
      log.Print("ModelCarId: ", modelCarId)

      _, err = pool.Exec(`INSERT INTO ownedModelCars (modelCarId, username, dateCollected) VALUES (?, ?, ?);`, modelCarId, user.Username, car.DateCollected)
      if (err != nil) { logError.Fatal(err.Error()) }
    }

    for wishedIndex := 0; wishedIndex < len(user.WishedModelCars); wishedIndex++ {
      car := user.WishedModelCars[wishedIndex]

      var modelCarId int;
      result := pool.QueryRow(`SELECT rowid FROM modelCars WHERE name=?;`, car.ModelCarName)
      err = result.Scan(&modelCarId)
      if (err != nil) { logError.Fatal(err.Error()) }
      log.Print("ModelCarId: ", modelCarId)

      _, err = pool.Exec(`INSERT INTO wishedModelCars (modelCarId, username, wishFactor) VALUES (?, ?, ?);`, modelCarId, user.Username, car.WishFactor)
      if (err != nil) { logError.Fatal(err.Error()) }
    }
  }
  log.Print("Test data created for the user, ownedModelCars, and wishedModelCars tables!")
}

type User struct {
  Username string
  Password string
  OwnedModelCars []OwnedModelCar
  WishedModelCars []WishedModelCar
}
type Mcec struct {
  Users []User
  ModelCars []ModelCar
}
type ModelCar struct {
  Name string
  Description string
  Category string
  Price int
  ImageFilePath string
}
type OwnedModelCar struct {
  ModelCarName string
  DateCollected string
}
type WishedModelCar struct {
  ModelCarName string
  WishFactor int
}

func (errorWriter ErrorWriter) Write(p []byte) (n int, err error) { // TODO: fix common code across modules
  color.Red(string(p))
  return 0, nil
}
type ErrorWriter struct {}
var logError *log.Logger



//TODO:
//  Homepage
//    Display basic blurb
 // Model Car Catalogue
//    View for showing a list of cars
//      Create button at the top of the page
//      Edit button on each car that will forward the user to the edit view
//      Delete button on each car that will remove the car from the list
//      Add to Owned list button on each car that will forward the user to the 'Create Owned List Item' View
//      Add to Wishlist button on each car that will forward the user to the 'Create Wishlist Item' View
//    View for creating a new car
//    View for editing a new car
//  Model Cars Owned List
//    View for showing a list of owned cars
//      Create button at the top of the page
//      Edit button on each car that will forward the user to the edit view
//      Delete button on each car that will remove the car from the list
//    View for creating an owned list item
//    View for editing an owned list item
//  Model Cars Wishlist
//    View for showing a list of wished cars
//      Create button at the top of the page
//      Edit button on each car that will forward the user to the edit view
//      Delete button on each car that will remove the car from the list
//    View for creating a wishlist item
//    View for editing a wishlist item
//  Login
//    A Login View for authing the user
//    Multiple users and see personalised lists for owned and wished
//  Misc
//    GitHub repo
//    Navbar displaying entries for Home, Model Cars, Owned, Wished, Login/Logon
//    UML Diagrams
//    Demo of app and code. Highlight where OOP techniques have been used

//DONE:
//  Homepage
//  Model Car Catalogue
//    View for showing a list of cars
//    Decide what data fields should be stored for each entry
//  Model Cars Owned List
//    View for showing a list of owned cars
//    Decide what data fields should be stored for each entry
//  Model Cars Wishlist
//    View for showing a list of wished cars
//    Decide what data fields should be stored for each entry
//  Misc
//    Write a command to 
//      create the database
//      populate it with test data

//Dependencies:
//  go
//    can be installed using scoop or by installing from the web
//    may not be needed but set the environment variable CGO_ENABLED to 1
//      if using powershell, this can be done using "Set-Variable CGO_ENABLED 1"
//    the go program creates the database and adds test data and can be run using "go run ." from the directory of the go project (src/util)
//    sqlite3 needs to be installed and can be done using "go install github.com/mattn/go-sqlite3"
//  gcc
//    can be installed easily using scoop
//
