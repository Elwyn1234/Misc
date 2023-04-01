import sqlite3

con = sqlite3.connect("bookingApp.db")
cursor = con.cursor()

cursor.execute('''
    INSERT INTO admins VALUES (
        'admin',
        'pass')
    ''')
cursor.execute('''
    INSERT INTO admins VALUES (
        'elwyn',
        '123')
    ''')

con.commit()
con.close()
