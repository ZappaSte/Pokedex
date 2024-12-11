from LoginFlask import app, db, User

# Crea una sessione del database utilizzando il contesto dell'app Flask
with app.app_context():  # Usa l'app Flask per creare il contesto
    # Creazione di un nuovo utente con username "admin"
    new_user = User(username="admin")

    # Imposta la password per il nuovo utente (memorizzata come hash nel database)
    new_user.set_password("admin")

    # Aggiunge il nuovo utente alla sessione del database
    db.session.add(new_user)

    # Conferma le modifiche e salva l'utente nel database
    db.session.commit()

    print("Utente aggiunto con successo!")
