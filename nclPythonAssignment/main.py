import sqlite3
import sys
from dataclasses import dataclass


# UTIL FUNCTIONS

# pricing is calculated using pennies
def totalPrice(days, accomodation):
    accomodationPrices={
        "Paradise Villa": 37000,
        "Sunshine Hut": 28000,
        "Standard Cabin": 20000,
        "Treehouse": 16000,
        "Classic Tipi": 12000,
    }
    cardFee={
        "Paradise Villa": 400,
        "Sunshine Hut": 350,
        "Standard Cabin": 300,
        "Treehouse": 250,
        "Classic Tipi": 200,
    }
    # discounts and vat is the number of five percents 
    discounts={
        "Paradise Villa": 0.90,
        "Sunshine Hut": 0.90,
        "Standard Cabin": 0.95,
        "Treehouse": 0.95,
        "Classic Tipi": 0.95,
    }
    vat = 1.15

    subtotal = accomodationPrices.get(accomodation) * days
    if days >= 14:
        subtotal = subtotal * discounts.get(accomodation)
    total = subtotal * vat + cardFee.get(accomodation)
    return round(total)

# This function returns a more readable format for the price (eg. £2050.10 for an input of 205010)
def penniesToPounds(pennies):
    toString = '£' + str(pennies / 100)
    if pennies % 10 == 0: # Append a 0 if toString only has one digit after the . (eg. 12.5 turns into 12.50)
        toString += '0'
    return toString

# Used to store which user is currently logged in
@dataclass
class LoginDetails:
    username: str
    password: str


# MAIN LOOP

running = True
while (running):
    userChoice = input("\na - Admin login | u - User login | r - Registration | e - Exit: ")

    if userChoice == "e": # Exit app functionality
        sys.exit()

    if userChoice == "r": # Register functionality
        username = input("Enter username: ")
        password = input("Enter password: ")
        firstName = input("Enter first name: ")
        lastName = input("Enter last name: ")
        email = input("Enter email: ")

        con = sqlite3.connect("bookingApp.db")
        cursor = con.cursor()
        cursor.execute(
            '''INSERT INTO users VALUES (?,?,?,?,?)''', (
            username,
            password,
            firstName,
            lastName,
            email
        ))
        con.commit()
        con.close()
        continue

    # If we get here, the user wants to login.
    # Login logic for user and admin is identical except they read from different databases
    enteredUsername = input("Username: ")
    enteredPassword = input("Password: ")

    con = sqlite3.connect("bookingApp.db")
    cursor = con.cursor()
    user = LoginDetails("","")
    if userChoice == "a":
        cursor.execute('''SELECT username, password FROM admins WHERE username = ?''', (enteredUsername,))
    else:
        cursor.execute('''SELECT username, password FROM users WHERE username = ?''', (enteredUsername,))

    data = cursor.fetchall()
    con.close()

    # If the username was not found, the length of the data variable will be 0 and we will continue to the next iteration of the main loop.
    if len(data) != 1:
        print("Login failed. Please try again.")
        continue

    user.username = data[0][0]
    user.password = data[0][1]

    if enteredPassword != user.password:
        print("Login failed. Please try again.")
        continue

    # If we get here, the user logged in successfully. This if else statement seperates admin and user functionality
    if userChoice == "a": # Admin functionality
        con = sqlite3.connect("bookingApp.db")
        cursor = con.cursor()
        cursor.execute('''SELECT * FROM bookings''')
        bookings = cursor.fetchall()
        print("\nDetails of all bookings:\n")
        total = 0

        for i in range(len(bookings)):
            booking = bookings[i]
            username = booking[0]
            cursor.execute('''SELECT * FROM users WHERE username = ?''', (username,))
            userDetails = cursor.fetchall()
            print(f"Username: {username}")
            print(f"Name: {userDetails[0][2] + ' ' + userDetails[0][3]}")
            print(f"Email: {userDetails[0][4]}")
            print(f"Date: {booking[1]}")
            print(f"Days: {booking[2]}")
            print(f"Accomodation: {booking[3]}")
            print(f"Price Paid: {penniesToPounds(booking[4])}\n")
            total = total + booking[4]
        print(f"Total paid: {penniesToPounds(total)}")
        con.close()
    else: # User functionality
        date = input("What date would you like to start your stay (dd-mm-yyyy)? ")
        days = int(input("How many days would you like to stay for? "))
        accomodation = input("Paradise Villa | Sunshine Hut | Standard Cabin | Treehouse | Classic Tipi: ")
        total = totalPrice(days, accomodation)
        print(f"Total: {penniesToPounds(total)}")

        confirm = input("Do you want to confirm your payment? (y/n) ")
        if confirm == "n":
            continue

        con = sqlite3.connect("bookingApp.db")
        cursor = con.cursor()
        cursor.execute(
            '''INSERT INTO bookings VALUES (?,?,?,?,?)''', (
            user.username,
            date,
            days,
            accomodation,
            totalPrice(int(days), accomodation)
        ))
        con.commit()
        con.close()
 
