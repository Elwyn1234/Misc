import sqlite3

con = sqlite3.connect("bookingApp.db")
cursor = con.cursor()

cursor.execute('''
    CREATE TABLE admins(
        username TEXT PRIMARY KEY,
        password TEXT NOT NULL
    )
    ''')

cursor.execute('''
    CREATE TABLE users (
        username TEXT PRIMARY KEY,
        password TEXT NOT NULL,
        firstName TEXT NOT NULL,
        lastName TEXT NOT NULL,
        email TEXT NOT NULL
    )
''')

cursor.execute('''
    CREATE TABLE bookings (
        user TEXT NOT NULL,
        dates TEXT NOT NULL,
        days INTEGER NOT NULL,
        accomodation TEXT NOT NULL,
        totalPrice INTEGER NOT NULL,
        FOREGIN KEY (user) REFERENCES users (id))
''')

con.commit()
con.close()