# poo-demo
Demostracion de Teoremas de Programacion Orientada a Objetos

# Programación Orientada a Objetos en ASP.NET Core (API + Swagger + Postman)

Este proyecto implementa los **4 principios de la Programación Orientada a Objetos (POO)** usando un **API ASP.NET Core** con endpoints visibles en **Swagger** y consultables mediante **Postman**.

> **Base URL** (ajusta según tu proyecto):  
> `https://localhost:5001` (HTTPS) o `http://localhost:5000` (HTTP)

---

## 🔹 1) Abstracción
La **abstracción** define solo lo esencial y oculta detalles de implementación.  
En el ejemplo, `Vehiculo` es **abstracta** y declara el contrato `Encender()`.

- **Endpoint:** `GET /api/poo/abstraccion`

**Ejemplo de respuesta**
```json
{
  "mensaje": "Auto Toyota: motor encendido (llave o botón)."
}
```

**Postman / curl**
```bash
curl -X GET "https://localhost:5001/api/poo/abstraccion"
```

---

## 🔹 2) Encapsulación
La **encapsulación** protege el estado interno y expone operaciones controladas.  
`CuentaBancaria` oculta `_saldo` y solo permite `Depositar` y `Retirar`.

- **Endpoints:**
  - `POST /api/poo/encapsulacion/depositar?monto=150`
  - `POST /api/poo/encapsulacion/retirar?monto=50`

**Ejemplo de respuesta — Depositar**
```json
{
  "codigo": "PE01",
  "titular": "Miguel Torres",
  "saldo": 250
}
```

**Ejemplo de respuesta — Retirar**
```json
{
  "codigo": "PE01",
  "titular": "Miguel Torres",
  "saldo": 200
}
```

**Ejemplo de error — Fondos insuficientes**
```json
{
  "title": "One or more validation errors occurred.",
  "status": 400,
  "detail": "Fondos insuficientes."
}
```

**Postman / curl**
```bash
# Depositar
curl -X POST "https://localhost:5001/api/poo/encapsulacion/depositar?monto=150"

# Retirar
curl -X POST "https://localhost:5001/api/poo/encapsulacion/retirar?monto=50"
```

---

## 🔹 3) Herencia
La **herencia** permite reutilizar y especializar comportamiento.  
`Auto` y `Moto` **heredan** de `Vehiculo` y personalizan `Describir()`.

- **Endpoint:** `GET /api/poo/herencia`

**Ejemplo de respuesta**
```json
[
  "Auto de marca Ford.",
  "Moto de marca Yamaha."
]
```

**Postman / curl**
```bash
curl -X GET "https://localhost:5001/api/poo/herencia"
```

---

## 🔹 4) Polimorfismo
El **polimorfismo** hace que la **misma** operación tenga resultados distintos según el tipo concreto.  
`Encender()` se implementa diferente en `Auto` y `Moto`.

- **Endpoint:** `GET /api/poo/polimorfismo`

**Ejemplo de respuesta**
```json
[
  "Auto Mazda: motor encendido (llave o botón).",
  "Moto Honda: motor encendido (patada o switch).",
  "Auto Kia: motor encendido (llave o botón).",
  "Moto Suzuki: motor encendido (patada o switch)."
]
```

**Postman / curl**
```bash
curl -X GET "https://localhost:5001/api/poo/polimorfismo"
```

---

## 🚀 Guía rápida para Postman
1. Crea una **colección**: `POO ASP.NET`.
2. Agrega estos **requests** (puedes usar una variable `base_url`):
   - `GET {{base_url}}/api/poo/abstraccion`
   - `POST {{base_url}}/api/poo/encapsulacion/depositar?monto=150`
   - `POST {{base_url}}/api/poo/encapsulacion/retirar?monto=50`
   - `GET {{base_url}}/api/poo/herencia`
   - `GET {{base_url}}/api/poo/polimorfismo`
3. Define `base_url = https://localhost:5001` (o tu URL).

## 📖 Swagger
- `https://localhost:5001/swagger`  
- `http://localhost:5000/swagger`

Desde Swagger puedes ejecutar cada endpoint con **Try it out** y ver el **schema** de respuesta.

