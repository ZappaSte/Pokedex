# -*- coding: utf-8 -*-

# Importazione dei moduli necessari
from flask import Flask, request, jsonify
from flask_sqlalchemy import SQLAlchemy
from flask_jwt_extended import JWTManager, create_access_token, jwt_required, get_jwt_identity
from werkzeug.security import generate_password_hash, check_password_hash
from flask_cors import CORS

# Inizializzazione dell'app Flask
app = Flask(__name__)

# Configurazione della chiave segreta per JWT
# definisce la chiave segreta usata per generare e verificare i token JWT
app.config['JWT_SECRET_KEY'] = 'your_jwt_secret_key'
jwt = JWTManager(app)

# Abilita CORS per tutte le rotte
CORS(app)  

# Configurazione dell'app per il database SQLite
app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:////Users/szappasodi/Projects/Pokedex/Pokedex/site.db'
app.config['SQLALCHEMY_TRACK_MODIFICATIONS'] = False
app.config['JWT_SECRET_KEY'] = 'your_jwt_secret_key'  # Usa una chiave segreta per JWT

# Inizializza il database e JWT
db = SQLAlchemy(app)
jwt = JWTManager(app)

# Definizione del modello per la tabella User nel database
class User(db.Model):
    id = db.Column(db.Integer, primary_key=True) # Identificativo univoco dell'utente
    username = db.Column(db.String(120), unique=True, nullable=False) # Nome utente univoco
    password_hash = db.Column(db.String(128), nullable=False) # Hash della password dell'utente

    # Metodo per impostare la password (salvata come hash)
    def set_password(self, password):
        self.password_hash = generate_password_hash(password, method='pbkdf2:sha256')

    # Metodo per verificare la password
    def check_password(self, password):
        return check_password_hash(self.password_hash, password)
    
#Definizione del modello per la tabella Preferiti nel database
class Favorite(db.Model):
    id = db.Column(db.Integer, primary_key=True) # Identificativo univoco
    user_id = db.Column(db.Integer, db.ForeignKey('user.id'), nullable=False) # Riferimento all'utente
    pokemon_id = db.Column(db.Integer, nullable=False) # Riferimento al pokemon

    user = db.relationship('User', backref=db.backref('favorites', lazy=True))

# Gestire Errore per l'icona del sito
@app.route('/favicon.ico')
def favicon():
    return '', 204  # Restituisce un'icona vuota senza errori

# Rotta principale di benvenuto
@app.route('/')
def home():
    return "Welcome to the Pokemon Login API!"

# Rotta per verificare la validit� di un token JWT
@app.route('/verify-token', methods=['GET'])
@jwt_required()

def verify_token():
    current_user = get_jwt_identity() # Ottiene l'identit� dell'utente dal token JWT
    return jsonify({"msg": "Token is valid", "user": current_user}), 200 # Risposta positiva

# Rotta per la registrazione di un nuovo utente
@app.route('/register', methods=['GET', 'POST'])
def register():
    if request.method == 'GET': # Gestisce richieste GET
        return "This endpoint requires a POST request with username and password in JSON format.", 400
    
    # Gestisce richieste POST
    data = request.get_json() # Recupera i dati JSON dalla richiesta
    username = data.get('username')
    password = data.get('password')
    
    # Controlla se l'utente esiste gi�
    if User.query.filter_by(username=username).first():
        return jsonify({"msg": "User already exists"}), 400

    # Crea un nuovo utente e lo salva nel database
    new_user = User(username=username)
    new_user.set_password(password) # Imposta l'hash della password
    db.session.add(new_user) # Aggiunge il nuovo utente alla sessione del database
    db.session.commit() # Salva le modifiche nel database

    return jsonify({"msg": "User registered successfully"}), 201

# Rotta per il login di un utente
@app.route('/login', methods=['GET', 'POST'])
def login():
    if request.method == 'GET': # Gestisce richieste GET
        return "This endpoint requires a POST request with username and password in JSON format.", 400

    # Gestisce richieste POST
    data = request.get_json() # Recupera i dati JSON dalla richiesta
    username = data.get('username')
    password = data.get('password')

    # Verifica le credenziali dell'utente
    user = User.query.filter_by(username=username).first()
    if user and user.check_password(password): # Controlla se l'utente esiste e la password � corretta
        access_token = create_access_token(identity=user.username) # Crea un token di accesso
        return jsonify({"access_token": access_token}), 200 # Restituisce il token

    return jsonify({"msg": "Invalid username or password"}), 401 # Credenziali non valide

# Rotta per aggiungere un preferito
@app.route('/add-favorite', methods=['POST'])
@jwt_required()
def add_favorite():
    # Gestisce richieste POST
    data = request.get_json() # Recupera i dati JSON dalla richiesta
    pokemon_id = data.get('pokemon_id')

    current_user = get_jwt_identity() # Ottienil'utente dal JWT
    user = User.query.filter_by(username=current_user).first()

    if not user:
        return jsonify({"msg": "User not found"}), 404

    #Verifico se il preferito è già stato aggiunto
    existing_favorite = Favorite.query.filter_by(user_id=user.id, pokemon_id=pokemon_id).first()
    if existing_favorite:
        return jsonify({"msg": "Pokemon is already in favorites"}), 400
    
    # Aggiungi il preferito
    favorite = Favorite(user_id=user.id, pokemon_id=pokemon_id)
    db.session.add(favorite)
    db.session.commit()

    return jsonify({"msg": "Favorite added successfully"}), 200

# Rotta per rimuovere un preferito
@app.route('/remove-favorite', methods=['POST'])
@jwt_required()
def remove_favorite():
    # Gestisce richieste POST
    data = request.get_json() # Recupera i dati JSON dalla richiesta
    pokemon_id = data.get('pokemon_id')

    current_user = get_jwt_identity() # Ottienil'utente dal JWT
    user = User.query.filter_by(username=current_user).first()

    if not user:
        return jsonify({"msg": "User not found"}), 404

    #Rimuovi il preferito
    favorite = Favorite.query.filter_by(user_id=user.id, pokemon_id=pokemon_id).first()
    if not favorite:
        return jsonify({"msg": "Pokemon not found favorites"}), 404
    
    db.session.delete(favorite)
    db.session.commit()

    return jsonify({"msg": "Favorite removed successfully"}), 200

# Rotta per ottenere i preferiti di un utente
@app.route('/favorites', methods=['GET'])
@jwt_required()
def favorites():
    current_user = get_jwt_identity() # Ottienil'utente dal JWT
    user = User.query.filter_by(username=current_user).first()

    favorites = Favorite.query.filter(Favorite.user_id == user.id).order_by(Favorite.pokemon_id).all()
    favorite_pokemon = [favorite.pokemon_id for favorite in favorites]

    return jsonify(favorite_pokemon), 200

# Punto di avvio dell'applicazione
if __name__ == '__main__':
    with app.app_context():  # Crea il contesto dell'app
        db.create_all()  # Crea le tabelle nel database se non esistono
    app.run(host="0.0.0.0", port=80, debug=True) # Avvia il server Flask
